using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Serilog;
using System.Security.Cryptography;
using System.Threading.Tasks;
using SharpCompress.Common;
using System.Diagnostics;
using System.Linq;

namespace XIVLauncher.Common.Dalamud
{
    public class AssetManager
    {
        private const string ASSET_STORE_URL = "https://aonyx.ffxiv.wang/Dalamud/Asset/Meta";

        internal class AssetInfo
        {
            public int Version { get; set; }
            public IReadOnlyList<Asset> Assets { get; set; }

            public class Asset
            {
                public string Url { get; set; }
                public string FileName { get; set; }
                public string Hash { get; set; }
            }
        }

        public static async Task<DirectoryInfo> EnsureAssets(DirectoryInfo baseDir)
        {
            var clientHandler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
            using var client = new HttpClient(clientHandler)
            {
                Timeout = TimeSpan.FromMinutes(4),
            };

            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
            };
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Mobile Safari/537.36 Edg/130.0.0.0");
            client.DefaultRequestHeaders.Add("accept-encoding", "gzip, deflate, br");
            using var sha1 = SHA1.Create();

            Log.Verbose("[DASSET] Starting asset download");

            var (isRefreshNeeded, info) = CheckAssetRefreshNeeded(baseDir);

            // NOTE(goat): We should use a junction instead of copying assets to a new folder. There is no C# API for junctions in .NET Framework.

            var currentDir = new DirectoryInfo(Path.Combine(baseDir.FullName, info.Version.ToString()));
            var devDir = new DirectoryInfo(Path.Combine(baseDir.FullName, "dev"));

            var assetFileDownloadList = new List<AssetInfo.Asset>();
            foreach (var entry in info.Assets)
            {
                var filePath = Path.Combine(currentDir.FullName, entry.FileName);

                if (!File.Exists(filePath))
                {
                    Log.Error("[DASSET] {0} not found locally", entry.FileName);
                    assetFileDownloadList.Add(entry);
                    //break;
                    continue;
                }

                if (string.IsNullOrEmpty(entry.Hash))
                    continue;

                try
                {
                    using var file = File.OpenRead(filePath);
                    var fileHash = sha1.ComputeHash(file);
                    var stringHash = BitConverter.ToString(fileHash).Replace("-", "");

                    if (stringHash != entry.Hash)
                    {
                        Log.Error("[DASSET] {0} has {1}, remote {2}, need refresh", entry.FileName, stringHash, entry.Hash);
                        assetFileDownloadList.Add(entry);
                        //break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "[DASSET] Could not read asset");
                    assetFileDownloadList.Add(entry);
                    continue;
                }
            }

            var fontUrls = new List<string>()
            {
                "https://mirrors.aliyun.com/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "http://mirrors.pku.edu.cn/ctan/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "https://mirror.bjtu.edu.cn/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "https://mirrors.bfsu.edu.cn/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "https://mirrors.jlu.edu.cn/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "https://mirrors.sustech.edu.cn/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "https://mirrors.nju.edu.cn/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "https://mirror.nyist.edu.cn/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "https://mirrors.tuna.tsinghua.edu.cn/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "https://mirrors.cloud.tencent.com/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "https://mirrors.ustc.edu.cn/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "https://mirrors.huaweicloud.com/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "https://mirror.lzu.edu.cn/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "https://mirrors.zju.edu.cn/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
                "https://mirror.iscas.ac.cn/CTAN/fonts/notocjksc/NotoSansCJKsc-Medium.otf",
            };

            foreach (var entry in assetFileDownloadList)
            {
                var oldFilePath = Path.Combine(devDir.FullName, entry.FileName);
                var newFilePath = Path.Combine(currentDir.FullName, entry.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(newFilePath)!);

                try
                {
                    if (File.Exists(oldFilePath))
                    {
                        using var file = File.OpenRead(oldFilePath);
                        var fileHash = sha1.ComputeHash(file);
                        var stringHash = BitConverter.ToString(fileHash).Replace("-", "");

                        if (stringHash == entry.Hash)
                        {
                            Log.Verbose("[DASSET] Get asset from old file: {0}", entry.FileName);
                            File.Copy(oldFilePath, newFilePath, true);
                            isRefreshNeeded = true;
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "[DASSET] Could not copy from old asset: {0}", entry.FileName);
                }
                var maxRetryNumber = 5;
                while (maxRetryNumber > 0) { 
                    try
                    {
                        Log.Information("[DASSET] Downloading {0} to {1}...", entry.Url, entry.FileName);

                        var request = await client.GetAsync(entry.Url).ConfigureAwait(true);
                        request.EnsureSuccessStatusCode();
                        File.WriteAllBytes(newFilePath, await request.Content.ReadAsByteArrayAsync().ConfigureAwait(true));
                        isRefreshNeeded = true;
                        continue;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "[DASSET] Could not download old asset: {0}", entry.FileName);
                        if (entry.FileName == "UIRes/NotoSansCJKsc-Medium.otf") {
                            maxRetryNumber= fontUrls.Count;
                            entry.Url = fontUrls.First();
                            fontUrls.RemoveAt(0);
                        }
                        maxRetryNumber --;
                    }
                }
            }

            if (isRefreshNeeded)
            {
                try
                {
                    DeleteAndRecreateDirectory(devDir);
                    CopyFilesRecursively(currentDir, devDir);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "[DASSET] Could not copy to dev dir");
                }

                SetLocalAssetVer(baseDir, info.Version);
            }

            Log.Verbose("[DASSET] Assets OK at {0}", currentDir.FullName);

            try
            {
                CleanUpOld(baseDir, devDir, currentDir);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[DASSET] Could not clean up old assets");
            }

            return currentDir;
        }

        public static void DeleteAndRecreateDirectory(DirectoryInfo dir)
        {
            if (!dir.Exists)
            {
                dir.Create();
            }
            else
            {
                dir.Delete(true);
                dir.Create();
            }
        }

        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (var dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));

            foreach (var file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }

        private static string GetAssetVerPath(DirectoryInfo baseDir)
        {
            return Path.Combine(baseDir.FullName, "asset.ver");
        }

        private static void CleanUpOld(DirectoryInfo baseDir, DirectoryInfo devDir, DirectoryInfo currentDir)
        {
            if (CheckIsGameOpen())
                return;

            if (!baseDir.Exists)
                return;

            foreach (var toDelete in baseDir.GetDirectories())
            {
                if (toDelete.Name != devDir.Name && toDelete.Name != currentDir.Name)
                {
                    toDelete.Delete(true);
                    Log.Verbose("[DASSET] Cleaned out {Path}", toDelete.FullName);
                }
            }

            Log.Verbose("[DASSET] Finished cleaning");
        }

        public static bool CheckIsGameOpen()
        {
#if DEBUG
            return false;
#endif

            var procs = Process.GetProcesses();

            if (procs.Any(x => x.ProcessName == "ffxiv"))
                return true;

            if (procs.Any(x => x.ProcessName == "ffxiv_dx11"))
                return true;

            if (procs.Any(x => x.ProcessName == "ffxivboot"))
                return true;

            if (procs.Any(x => x.ProcessName == "ffxivlauncher"))
                return true;

            return false;
        }

        /// <summary>
        ///     Check if an asset update is needed. When this fails, just return false - the route to github
        ///     might be bad, don't wanna just bail out in that case
        /// </summary>
        /// <param name="baseDir">Base directory for assets</param>
        /// <returns>Update state</returns>
        private static (bool isRefreshNeeded, AssetInfo info) CheckAssetRefreshNeeded(DirectoryInfo baseDir)
        {
            using var client = new WebClient();

            var localVerFile = GetAssetVerPath(baseDir);
            var localVer = 0;

            try
            {
                if (File.Exists(localVerFile))
                    localVer = int.Parse(File.ReadAllText(localVerFile));
            }
            catch (Exception ex)
            {
                // This means it'll stay on 0, which will redownload all assets - good by me
                Log.Error(ex, "[DASSET] Could not read asset.ver");
            }

            var remoteVer = JsonConvert.DeserializeObject<AssetInfo>(client.DownloadString(ASSET_STORE_URL));

            Log.Verbose("[DASSET] Ver check - local:{0} remote:{1}", localVer, remoteVer.Version);

            var needsUpdate = remoteVer.Version > localVer;

            return (needsUpdate, remoteVer);
        }

        private static void SetLocalAssetVer(DirectoryInfo baseDir, int version)
        {
            try
            {
                var localVerFile = GetAssetVerPath(baseDir);
                File.WriteAllText(localVerFile, version.ToString());
            }
            catch (Exception e)
            {
                Log.Error(e, "[DASSET] Could not write local asset version");
            }
        }

        private static void CleanUpOld(DirectoryInfo baseDir, int version)
        {
            //if (GameHelpers.CheckIsGameOpen())
            //    return;

            var toDelete = Path.Combine(baseDir.FullName, version.ToString());

            try
            {
                if (Directory.Exists(toDelete))
                    Directory.Delete(toDelete, true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not clean up old assets");
            }
        }
    }
}
