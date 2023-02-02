using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Dalamud.Updater;
using Newtonsoft.Json;
using Serilog;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;
using SharpCompress.Readers;
//using XIVLauncher.Common.PlatformAbstractions;
using XIVLauncher.Common.Util;

namespace XIVLauncher.Common.Dalamud
{
    public interface IDalamudLoadingOverlay
    {
        public enum DalamudUpdateStep
        {
            Dalamud,
            Assets,
            Runtime,
            Unavailable
        }

        public void SetStep(DalamudUpdateStep step);

        public void SetVisible();

        public void SetInvisible();

        public void ReportProgress(long? size, long downloaded, double? progress);
    }

    public class DalamudUpdater
    {
        private readonly DirectoryInfo addonDirectory;
        private readonly DirectoryInfo runtimeDirectory;
        private readonly DirectoryInfo assetDirectory;
        private readonly DirectoryInfo configDirectory;
        //private readonly IUniqueIdCache? cache;
        public const string REMOTE_BASE = "https://aonyx.ffxiv.wang/";
        public const string REMOTE_VERSION = REMOTE_BASE + "Dalamud/Release/VersionInfo?track=";
        public const string REMOTE_DOTNET = REMOTE_BASE + "Dalamud/Release/Runtime/DotNet/{0}";
        public const string REMOTE_DESKTOP = REMOTE_BASE + "Dalamud/Release/Runtime/WindowsDesktop/{0}";
        private readonly TimeSpan defaultTimeout = TimeSpan.FromMinutes(25);
        private static string onlineHash = string.Empty;

        private DownloadState _state;
        public DownloadState State
        {
            get { return _state; }
            
            private set
            {
                _state = value;
                OnUpdateEvent?.Invoke(this._state);
            }
        }

        public bool IsStaging { get; private set; } = false;

        private FileInfo runnerInternal;

        public FileInfo Runner
        {
            get
            {
                if (RunnerOverride != null)
                    return RunnerOverride;

                return runnerInternal;
            }
            private set => runnerInternal = value;
        }

        public DirectoryInfo Runtime => this.runtimeDirectory;

        public FileInfo RunnerOverride { get; set; }

        public DirectoryInfo AssetDirectory { get; private set; }

        public IDalamudLoadingOverlay Overlay { get; set; }

        public enum DownloadState
        {
            Checking,
            Unknown,
            Done,
            Failed,
            NoIntegrity
        }

        public DalamudUpdater(DirectoryInfo addonDirectory, DirectoryInfo runtimeDirectory, DirectoryInfo assetDirectory, DirectoryInfo configDirectory)
        {
            this.State = DownloadState.Unknown;
            this.addonDirectory = addonDirectory;
            this.runtimeDirectory = runtimeDirectory;
            this.assetDirectory = assetDirectory;
            this.configDirectory = configDirectory;
            //this.cache = cache;
        }

        public void SetOverlayProgress(IDalamudLoadingOverlay.DalamudUpdateStep progress)
        {
            Overlay.SetStep(progress);
        }

        public void ShowOverlay()
        {
            Overlay.SetVisible();
        }

        public void CloseOverlay()
        {
            Overlay.SetInvisible();
        }

        private void ReportOverlayProgress(long? size, long downloaded, double? progress)
        {
            Overlay.ReportProgress(size, downloaded, progress);
        }
        public delegate void UpdateEvent(DownloadState value);
        public event UpdateEvent OnUpdateEvent;
        private readonly static object Mutex = new object();
        public void Run()
        {
            //lock (Mutex)
            //{
                this.State = DownloadState.Checking;
                Log.Information("[DUPDATE] Starting...");
                Task.Run(async () =>
                {
                    const int MAX_TRIES = 3;

                    for (var tries = 0; tries < MAX_TRIES; tries++)
                    {
                        try
                        {
                            await UpdateDalamud().ConfigureAwait(true);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, "[DUPDATE] Update failed, try {TryCnt}/{MaxTries}...", tries, MAX_TRIES);
                        }
                    }

                    if (this.State != DownloadState.Done) this.State = DownloadState.Failed;
                    //Mutex.Close();
                    OnUpdateEvent?.Invoke(this.State);
                });
            //}

        }

        private static string GetBetaTrackName(DalamudSettings settings) =>
            string.IsNullOrEmpty(settings.DalamudBetaKind) ? "staging" : settings.DalamudBetaKind;

        private async Task<(DalamudVersionInfo release, DalamudVersionInfo? staging)> GetVersionInfo(DalamudSettings settings)
        {
            using var client = new HttpClient
            {
                Timeout = this.defaultTimeout,
            };

            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
            };

            var versionInfoJsonRelease = await client.GetStringAsync(REMOTE_VERSION + "release").ConfigureAwait(false);

            DalamudVersionInfo versionInfoRelease = JsonConvert.DeserializeObject<DalamudVersionInfo>(versionInfoJsonRelease);

            DalamudVersionInfo? versionInfoStaging = null;

            if (!string.IsNullOrEmpty(settings.DalamudBetaKey))
            {
                var versionInfoJsonStaging = await client.GetAsync(REMOTE_VERSION + GetBetaTrackName(settings)).ConfigureAwait(false);

                if (versionInfoJsonStaging.StatusCode != HttpStatusCode.BadRequest)
                    versionInfoStaging = JsonConvert.DeserializeObject<DalamudVersionInfo>(await versionInfoJsonStaging.Content.ReadAsStringAsync().ConfigureAwait(false));
            }

            return (versionInfoRelease, versionInfoStaging);
        }

        private async Task UpdateDalamud()
        {
            var settings = DalamudSettings.GetSettings(this.configDirectory);

            // GitHub requires TLS 1.2, we need to hardcode this for Windows 7
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var (versionInfoRelease, versionInfoStaging) = await GetVersionInfo(settings).ConfigureAwait(false);

            var remoteVersionInfo = versionInfoRelease;

            if (versionInfoStaging?.Key != null && versionInfoStaging.Key == settings.DalamudBetaKey)
            {
                remoteVersionInfo = versionInfoStaging;
                IsStaging = true;
                Log.Information("[DUPDATE] Using staging version {Kind} with key {Key} ({Hash})", settings.DalamudBetaKind, settings.DalamudBetaKey, remoteVersionInfo.AssemblyVersion);
            }
            else
            {
                Log.Information("[DUPDATE] Using release version ({Hash})", remoteVersionInfo.AssemblyVersion);
            }

            var versionInfoJson = JsonConvert.SerializeObject(remoteVersionInfo);

            onlineHash = remoteVersionInfo.Hash;

            var addonPath = new DirectoryInfo(Path.Combine(this.addonDirectory.FullName, "Hooks"));
            var currentVersionPath = new DirectoryInfo(Path.Combine(addonPath.FullName, remoteVersionInfo.AssemblyVersion));
            var runtimePaths = new DirectoryInfo[]
            {
                new(Path.Combine(this.runtimeDirectory.FullName, "host", "fxr", remoteVersionInfo.RuntimeVersion)),
                new(Path.Combine(this.runtimeDirectory.FullName, "shared", "Microsoft.NETCore.App", remoteVersionInfo.RuntimeVersion)),
                new(Path.Combine(this.runtimeDirectory.FullName, "shared", "Microsoft.WindowsDesktop.App", remoteVersionInfo.RuntimeVersion)),
            };

            if (!currentVersionPath.Exists || !IsIntegrity(currentVersionPath))
            {
                Log.Information("[DUPDATE] Not found, redownloading");
                SetOverlayProgress(IDalamudLoadingOverlay.DalamudUpdateStep.Dalamud);

                try
                {
                    await DownloadDalamud(currentVersionPath, remoteVersionInfo).ConfigureAwait(true);
                    CleanUpOld(addonPath, remoteVersionInfo.AssemblyVersion);

                    // This is a good indicator that we should clear the UID cache
                    //cache?.Reset();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "[DUPDATE] Could not download dalamud");

                    State = DownloadState.NoIntegrity;
                    return;
                }
            }

            if (remoteVersionInfo.RuntimeRequired || settings.DoDalamudRuntime)
            {
                Log.Information("[DUPDATE] Now starting for .NET Runtime {0}", remoteVersionInfo.RuntimeVersion);

                var versionFile = new FileInfo(Path.Combine(this.runtimeDirectory.FullName, "version"));
                var localVersion = "5.0.6"; // This is the version we first shipped. We didn't write out a version file, so we can't check it.
                if (versionFile.Exists)
                    localVersion = File.ReadAllText(versionFile.FullName);

                if (runtimePaths.Any(p => !p.Exists) || localVersion != remoteVersionInfo.RuntimeVersion)
                {
                    Log.Information("[DUPDATE] Not found or outdated: {LocalVer} - {RemoteVer}", localVersion, remoteVersionInfo.RuntimeVersion);

                    SetOverlayProgress(IDalamudLoadingOverlay.DalamudUpdateStep.Runtime);

                    try
                    {
                        await DownloadRuntime(this.runtimeDirectory, remoteVersionInfo.RuntimeVersion).ConfigureAwait(false);
                        File.WriteAllText(versionFile.FullName, remoteVersionInfo.RuntimeVersion);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "[DUPDATE] Could not download runtime");

                        State = DownloadState.Failed;
                        return;
                    }
                }
            }

            try
            {
                this.SetOverlayProgress(IDalamudLoadingOverlay.DalamudUpdateStep.Assets);
                this.ReportOverlayProgress(null, 0, null);
                AssetDirectory = await AssetManager.EnsureAssets(this.assetDirectory).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[DUPDATE] Asset ensurement error, bailing out...");
                State = DownloadState.Failed;
                return;
            }

            if (!IsIntegrity(currentVersionPath))
            {
                Log.Error("[DUPDATE] Integrity check failed after ensurement.");

                State = DownloadState.NoIntegrity;
                return;
            }

            WriteVersionJson(currentVersionPath, versionInfoJson);

            Log.Information("[DUPDATE] All set for " + remoteVersionInfo.SupportedGameVer);

            Runner = new FileInfo(Path.Combine(currentVersionPath.FullName, "Dalamud.Injector.exe"));

            State = DownloadState.Done;
        }

        private static bool CanRead(FileInfo info)
        {
            try
            {
                using var stream = info.OpenRead();
                stream.ReadByte();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool IsIntegrity(DirectoryInfo addonPath)
        {
            var files = addonPath.GetFiles();

            try
            {
                if (!CanRead(files.First(x => x.Name == "Dalamud.Injector.exe"))
                    || !CanRead(files.First(x => x.Name == "Dalamud.dll"))
                    || !CanRead(files.First(x => x.Name == "ImGuiScene.dll")))
                {
                    Log.Error("[DUPDATE] Can't open files for read");
                    return false;
                }

                var hashesPath = Path.Combine(addonPath.FullName, "hashes.json");

                if (!File.Exists(hashesPath))
                {
                    Log.Error("[DUPDATE] No hashes.json");
                    return false;
                }

                if (!string.IsNullOrEmpty(onlineHash))
                {
                    using var stream = File.OpenRead(hashesPath);
                    using var md5 = MD5.Create();

                    var hashHash = BitConverter.ToString(md5.ComputeHash(stream)).ToUpperInvariant().Replace("-", string.Empty);

                    if (onlineHash != hashHash)
                    {
                        Log.Error("[UPDATE] hashes.json Hash Check Failed");
                        return false;
                    }
                }

                var hashes = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(hashesPath));

                foreach (var hash in hashes)
                {
                    var file = Path.Combine(addonPath.FullName, hash.Key.Replace("\\", "/"));
                    using var fileStream = File.OpenRead(file);
                    using var md5 = MD5.Create();

                    var hashed = BitConverter.ToString(md5.ComputeHash(fileStream)).ToUpperInvariant().Replace("-", string.Empty);

                    if (hashed != hash.Value)
                    {
                        Log.Error("[DUPDATE] Integrity check failed for {0} ({1} - {2})", file, hash.Value, hashed);
                        return false;
                    }

                    Log.Verbose("[DUPDATE] Integrity check OK for {0} ({1})", file, hashed);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[DUPDATE] No dalamud integrity");
                return false;
            }

            return true;
        }

        private static void CleanUpOld(DirectoryInfo addonPath, string currentVer)
        {
            if (!addonPath.Exists)
                return;

            foreach (var directory in addonPath.GetDirectories())
            {
                if (directory.Name == "dev" || directory.Name == currentVer) continue;

                try
                {
                    directory.Delete(true);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private static void WriteVersionJson(DirectoryInfo addonPath, string info)
        {
            File.WriteAllText(Path.Combine(addonPath.FullName, "version.json"), info);
        }

        public static string GetTempFileName()
        {
            // https://stackoverflow.com/a/50413126
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }

        private async Task DownloadDalamud(DirectoryInfo addonPath, DalamudVersionInfo version)
        {
            // Ensure directory exists
            if (!addonPath.Exists)
                addonPath.Create();
            else
            {
                addonPath.Delete(true);
                addonPath.Create();
            }

            var downloadPath = GetTempFileName();

            if (File.Exists(downloadPath))
                File.Delete(downloadPath);

            await this.DownloadFile(version.DownloadUrl, downloadPath, this.defaultTimeout).ConfigureAwait(false);

            if (version.DownloadUrl.EndsWith("zip", StringComparison.OrdinalIgnoreCase))
            {
                ZipFile.ExtractToDirectory(downloadPath, addonPath.FullName);
            }
            else if (version.DownloadUrl.EndsWith("7z", StringComparison.OrdinalIgnoreCase))
            {
                using (var archive = SevenZipArchive.Open(downloadPath))
                {
                    var reader = archive.ExtractAllEntries();
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                            reader.WriteEntryToDirectory(addonPath.FullName, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                    }
                }
            }
            else
            {
                Log.Error("[DUPDATE] Unsupported file.");
            }

            File.Delete(downloadPath);

            try
            {
                var devPath = new DirectoryInfo(Path.Combine(addonPath.FullName, "..", "dev"));

                if (!devPath.Exists)
                    devPath.Create();
                else
                {
                    devPath.Delete(true);
                    devPath.Create();
                }

                foreach (var fileInfo in addonPath.GetFiles())
                {
                    fileInfo.CopyTo(Path.Combine(devPath.FullName, fileInfo.Name));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[DUPDATE] Could not copy to dev folder.");
            }
        }

        private async Task DownloadRuntime(DirectoryInfo runtimePath, string version)
        {
            // Ensure directory exists
            if (!runtimePath.Exists)
            {
                runtimePath.Create();
            }
            else
            {
                runtimePath.Delete(true);
                runtimePath.Create();
            }

            var dotnetUrl = string.Format(REMOTE_DOTNET, version);
            var desktopUrl = string.Format(REMOTE_DESKTOP, version);
            //var dotnetUrl = $"https://dotnetcli.blob.core.windows.net/dotnet/Runtime/{version}/dotnet-runtime-{version}-win-x64.zip";
            //var desktopUrl = $"https://dotnetcli.blob.core.windows.net/dotnet/WindowsDesktop/{version}/windowsdesktop-runtime-{version}-win-x64.zip";

            var downloadPath = GetTempFileName();

            if (File.Exists(downloadPath))
                File.Delete(downloadPath);

            await this.DownloadFile(dotnetUrl, downloadPath, this.defaultTimeout).ConfigureAwait(false);
            ZipFile.ExtractToDirectory(downloadPath, runtimePath.FullName);

            await this.DownloadFile(desktopUrl, downloadPath, this.defaultTimeout).ConfigureAwait(false);
            ZipFile.ExtractToDirectory(downloadPath, runtimePath.FullName);

            File.Delete(downloadPath);
        }

        private async Task DownloadFile(string url, string path, TimeSpan timeout)
        {
            using var downloader = new HttpClientDownloadWithProgress(url, path);
            downloader.ProgressChanged += this.ReportOverlayProgress;

            await downloader.Download().ConfigureAwait(false);
        }
    }
}
