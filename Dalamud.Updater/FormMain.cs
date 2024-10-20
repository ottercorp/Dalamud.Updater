using AutoUpdaterDotNET;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using XIVLauncher.Common.Dalamud;

namespace Dalamud.Updater
{
    public partial class FormMain : Form
    {
        private const string UPDATEURL = "https://aonyx.ffxiv.wang/Updater/Release/VersionInfo";
        private const string OTTERHOME = """
            如需帮助或者反馈,请前往:
            https://file.bluefissure.com/FFXIV/Dalamud
            https://github.com/ottercorp/Dalamud.Updater
            https://aonyx.ffxiv.wang/
            QQ频道:https://pd.qq.com/s/9ehyfcha3
            QQ频道:https://pd.ottercorp.net
            """;

        // private List<string> pidList = new List<string>();
        private bool firstHideHint = true;
        private bool isThreadRunning = true;
        private bool dotnetDownloadFinished = false;
        private bool desktopDownloadFinished = false;
        private Config config;
        private DalamudLoadingOverlay dalamudLoadingOverlay;

        private readonly DirectoryInfo addonDirectory;
        private readonly DirectoryInfo runtimeDirectory;
        private readonly DirectoryInfo xivlauncherDirectory;
        private readonly DirectoryInfo assetDirectory;
        private readonly DirectoryInfo configDirectory;

        private readonly DalamudUpdater dalamudUpdater;

        public string windowsTitle = "獭纪委 v" + Assembly.GetExecutingAssembly().GetName().Version;

        private int checkTimes = 0;

        private void CheckUpdate()
        {
            checkTimes++;
            if (checkTimes == 8)
            {
                MessageBox.Show("点这么多遍干啥？", windowsTitle);
            }
            else if (checkTimes == 9)
            {
                MessageBox.Show("还点？", windowsTitle);
            }
            else if (checkTimes > 10)
            {
                MessageBox.Show("有问题你发日志，别搁这瞎几把点了", windowsTitle);
            }
            dalamudUpdater.Run();
        }

        private Version GetUpdaterVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }

        private string getVersion()
        {
            var rgx = new Regex(@"^\d+\.\d+\.\d+\.\d+$");
            var stgRgx = new Regex(@"^\d+\.\d+\.\d+\.\d+-\d*-[\da-zA-Z]{9}$");
            var di = new DirectoryInfo(Path.Combine(addonDirectory.FullName, "Hooks"));
            var version = new Version("0.0.0.0");
            if (!di.Exists)
                return version.ToString();
            var dirs = di.GetDirectories("*", SearchOption.TopDirectoryOnly).Where(dir => rgx.IsMatch(dir.Name)).ToArray();
            bool releaseVersionExists = false;
            foreach (var dir in dirs)
            {
                var newVersion = new Version(dir.Name);
                if (newVersion > version)
                {
                    releaseVersionExists = true;
                    version = newVersion;
                }
            }
            if (!releaseVersionExists)
            {
                var stgDirs = di.GetDirectories("*", SearchOption.TopDirectoryOnly).Where(dir => stgRgx.IsMatch(dir.Name)).ToArray();
                if (stgDirs.Length > 0)
                {
                    return stgDirs[0].Name;
                }
            }
            return version.ToString();
        }


        public FormMain()
        {
            InitLogging();
            InitializeComponent();
            InitializePIDCheck();
            InitializeDeleteShit();
            dalamudLoadingOverlay = new DalamudLoadingOverlay(this);
            dalamudLoadingOverlay.OnProgressBar += setProgressBar;
            dalamudLoadingOverlay.OnSetVisible += setVisible;
            dalamudLoadingOverlay.OnStatusLabel += setStatus;

            if (!Directory.Exists(RoamingPath)) Directory.CreateDirectory(RoamingPath);
            DeleteLink();
            CheckPath();

            addonDirectory = new DirectoryInfo(Path.Combine(RoamingPath, "addon"));
            runtimeDirectory = new DirectoryInfo(Path.Combine(RoamingPath, "runtime"));
            assetDirectory = new DirectoryInfo(Path.Combine(RoamingPath, "dalamudAssets"));
            configDirectory = new DirectoryInfo(Path.Combine(RoamingPath));
            //labelVersion.Text = string.Format("卫月版本 : {0}", getVersion());
            string[] strArgs = Environment.GetCommandLineArgs();
            if (strArgs.Length >= 2 && strArgs[1].Equals("-startup"))
            {
                //this.WindowState = FormWindowState.Minimized;
                //this.ShowInTaskbar = false;
                if (firstHideHint)
                {
                    firstHideHint = false;
                    this.DalamudUpdaterIcon.ShowBalloonTip(2000, "自启动成功", "放心，我会在后台偷偷干活的。", ToolTipIcon.Info);
                }
            }
            dalamudUpdater = new DalamudUpdater(addonDirectory, runtimeDirectory, assetDirectory, configDirectory);
            dalamudUpdater.Overlay = dalamudLoadingOverlay;
            dalamudUpdater.OnUpdateEvent += DalamudUpdater_OnUpdateEvent;
            InitializeConfig();
            labelVer.Text = $"v{Assembly.GetExecutingAssembly().GetName().Version}";
            UpdateFormConfig();
            UpdateSelf();

            SetDalamudVersion();

            CheckUpdate();
        }

        private void DalamudUpdater_OnUpdateEvent(DalamudUpdater.DownloadState value)
        {
            switch (value)
            {
                case DalamudUpdater.DownloadState.Failed:
                    MessageBox.Show("更新Dalamud失败", windowsTitle, MessageBoxButtons.YesNo);
                    setStatus("更新Dalamud失败");
                    break;
                case DalamudUpdater.DownloadState.Unknown:
                    setStatus("未知错误");
                    break;
                case DalamudUpdater.DownloadState.NoIntegrity:
                    setStatus("卫月与游戏不兼容");
                    break;
                case DalamudUpdater.DownloadState.Done:
                    SetDalamudVersion();
                    setStatus("更新成功");
                    break;
                case DalamudUpdater.DownloadState.Checking:
                    setStatus("检查更新中...");
                    break;
            }
        }

        public void SetDalamudVersion()
        {
            var verStr = string.Format("卫月版本 : {0}", getVersion());
            if (this.labelVersion.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { labelVersion.Text = x; };
                this.labelVersion.Invoke(actionDelegate, verStr);
            }
            else
            {
                labelVersion.Text = verStr;
            }
        }
        #region init
        private static void InitLogging()
        {
            var baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var logPath = Path.Combine(baseDirectory, "Dalamud.Updater.log");

            var levelSwitch = new LoggingLevelSwitch();

#if DEBUG
            levelSwitch.MinimumLevel = LogEventLevel.Verbose;
#else
            levelSwitch.MinimumLevel = LogEventLevel.Information;
#endif


            Log.Logger = new LoggerConfiguration()
                //.WriteTo.Console(standardErrorFromLevel: LogEventLevel.Verbose)
                .WriteTo.Async(a => a.File(logPath))
                .MinimumLevel.ControlledBy(levelSwitch)
                .CreateLogger();
        }
        private void InitializeConfig()
        {
            this.config = Config.Load(Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "DalamudUpdaterConfig.json"));
        }

        private void InitializeDeleteShit()
        {
            var shitConfig = Path.Combine(Directory.GetCurrentDirectory(), "Dalamud.Updater.exe.config");
            if (File.Exists(shitConfig))
            {
                File.Delete(shitConfig);
            }

            var shitInjector = Path.Combine(Directory.GetCurrentDirectory(), "Dalamud.Injector.exe");
            if (File.Exists(shitInjector))
            {
                File.Delete(shitInjector);
            }

            var shitDalamud = Path.Combine(Directory.GetCurrentDirectory(), "6.3.0.9");
            if (Directory.Exists(shitDalamud))
            {
                Directory.Delete(shitDalamud, true);
            }

            var shitUIRes = Path.Combine(Directory.GetCurrentDirectory(), "XIVLauncher", "dalamudAssets", "UIRes");
            if (Directory.Exists(shitUIRes))
            {
                Directory.Delete(shitUIRes, true);
            }

            var shitAddon = Path.Combine(Directory.GetCurrentDirectory(), "addon");
            if (Directory.Exists(shitAddon))
            {
                Directory.Delete(shitAddon, true);
            }

            var shitRuntime = Path.Combine(Directory.GetCurrentDirectory(), "runtime");
            if (Directory.Exists(shitRuntime))
            {
                Directory.Delete(shitRuntime, true);
            }
        }

        private void InitializePIDCheck()
        {
            var thread = new Thread(() =>
            {
                while (this.isThreadRunning)
                {
                    try
                    {
                        //var newPidList = Process.GetProcessesByName("ffxiv_dx11").Where(process =>
                        //{
                        //    return !process.MainWindowTitle.Contains("FINAL FANTASY XIV");
                        //}).ToList().ConvertAll(process => process.Id.ToString()).ToArray();
                        //为什么我开了FF检测不到啊.jpg
                        var newPidList = Process.GetProcesses().Where(process =>
                        {
                            if (process.ProcessName == "ffxiv_dx11" || process.ProcessName == "ffxiv")
                            {
                                return !process.MainWindowTitle.Contains("FINAL FANTASY XIV");
                            }
                            return false;
                        }).ToList().ConvertAll(process => process.Id.ToString()).ToArray();
                        var newHash = String.Join(", ", newPidList).GetHashCode();
                        var oldPidList = this.comboBoxFFXIV.Items.Cast<Object>().Select(item => item.ToString()).ToArray();
                        var oldHash = String.Join(", ", oldPidList).GetHashCode();
                        if (oldHash != newHash && this.comboBoxFFXIV.IsHandleCreated)
                        {
                            this.comboBoxFFXIV.Invoke((MethodInvoker)delegate
                            {
                                // Running on the UI thread
                                comboBoxFFXIV.Items.Clear();
                                comboBoxFFXIV.Items.AddRange(newPidList);
                                if (newPidList.Length > 0)
                                {
                                    if (!comboBoxFFXIV.DroppedDown)
                                        this.comboBoxFFXIV.SelectedIndex = 0;
                                    if (this.checkBoxAutoInject.Checked)
                                    {
                                        foreach (var pidStr in newPidList)
                                        {
                                            //Thread.Sleep((int)(this.injectDelaySeconds * 1000));
                                            var pid = int.Parse(pidStr);
                                            if (Process.GetProcessById(pid).ProcessName != "ffxiv_dx11")
                                            {
                                                this.DalamudUpdaterIcon.ShowBalloonTip(2000, "找不到游戏", $"进程{pid}不是dx11版FF。", ToolTipIcon.Warning);
                                                Log.Information("{pid} is not dx11", pid);
                                                continue;
                                            }
                                            if (this.Inject(pid, (int)(this.config.InjectDelaySeconds * 1000)))
                                            {
                                                this.DalamudUpdaterIcon.ShowBalloonTip(2000, "帮你注入了", $"帮你注入了进程{pid}，不用谢。", ToolTipIcon.Info);
                                            }
                                        }
                                    }
                                }
                            });
                        }
                    }
                    catch
                    {

                    }
                    Thread.Sleep(1000);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        #endregion
        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        private void UpdateFormConfig()
        {
            this.checkBoxAutoInject.Checked = this.config.AutoInject.Value;
            this.checkBoxAutoStart.Checked = this.config.AutoStart.Value;
            this.delayBox.Value = (decimal)this.config.InjectDelaySeconds;
            this.checkBoxSafeMode.Checked = this.config.SafeMode.Value;
        }

        private void UpdateSelf()
        {
            AutoUpdater.ApplicationExitEvent += () =>
            {
                this.Text = @"Closing application...";
                Thread.Sleep(5000);
                this.Dispose();
                this.DalamudUpdaterIcon.Dispose();
                Application.Exit();
            };
            //AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            AutoUpdater.InstalledVersion = GetUpdaterVersion();
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.UpdateMode = Mode.Normal;
            try
            {
                AutoUpdater.ParseUpdateInfoEvent += (args) =>
                {
                    try
                    {
#if DEBUG
                        var json = JsonConvert.DeserializeObject<VersionInfo>(File.ReadAllText(@"D:\Code\ottercorp\version.txt"), new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All,
                            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                            Formatting = Formatting.Indented,
                            NullValueHandling = NullValueHandling.Ignore,

                        });
#else
                        var json = JsonConvert.DeserializeObject<VersionInfo>(args.RemoteData, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All,
                            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                            Formatting = Formatting.Indented,
                            NullValueHandling = NullValueHandling.Ignore,
                        });
#endif
                        if (json.Version == null || json.DownloadUrl == null)
                        {
                            throw new Exception($"远程版本配置文件错误:\n {args.RemoteData}");
                        }

                        json.ChangeLog ??= "https://bbs.tggfl.com/topic/32/dalamud-%E5%8D%AB%E6%9C%88%E6%A1%86%E6%9E%B6";
                        args.UpdateInfo = new UpdateInfoEventArgs
                        {
                            CurrentVersion = json.Version,
                            ChangelogURL = json.ChangeLog,
                            DownloadURL = json.DownloadUrl,
                        };
                        if (json.Config != null && this.config != null)
                        {
                            var type = typeof(Config);
                            foreach (var property in type.GetProperties())
                            {
                                //if (property.Name.Equals())
                                var remoteValue = property.GetValue(json.Config, null);
                                if (remoteValue != null)
                                {
                                    property.SetValue(this.config, remoteValue);
                                    Log.Information($"Change config {property.Name} value: {property.GetValue(this.config)} -> {remoteValue}");
                                }
                            }
                            this.checkBoxSafeMode.Invoke(UpdateFormConfig);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}\n{OTTERHOME}", "程序启动版本检查失败",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                };
                //AutoUpdater.ShowUpdateForm();
                AutoUpdater.Start(UPDATEURL);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\n{OTTERHOME}", "程序启动版本检查失败",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void FormMain_Disposed(object sender, EventArgs e)
        {
            this.isThreadRunning = false;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            //this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            //this.ShowInTaskbar = false;
            //this.Visible = false;
            if (firstHideHint)
            {
                firstHideHint = false;
                this.DalamudUpdaterIcon.ShowBalloonTip(2000, "小玩意挺会藏", "哎我藏起来了，单击托盘图标呼出程序界面。", ToolTipIcon.Info);
            }
        }

        private void DalamudUpdaterIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //if (this.WindowState == FormWindowState.Minimized)
                //{
                //    this.WindowState = FormWindowState.Normal;
                //    this.FormBorderStyle = FormBorderStyle.FixedDialog;
                //    this.ShowInTaskbar = true;
                //}
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                }
                this.Activate();
            }
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //WindowState = FormWindowState.Normal;
            if (!this.Visible) this.Visible = true;
            this.Activate();
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
            //this.Close();
            this.DalamudUpdaterIcon.Dispose();
            Application.Exit();
        }

        private void ButtonCheckForUpdate_Click(object sender, EventArgs e)
        {
            if (this.comboBoxFFXIV.SelectedItem != null)
            {
                var pid = int.Parse((string)this.comboBoxFFXIV.SelectedItem);
                var process = Process.GetProcessById(pid);
                if (isInjected(process))
                {
                    var choice = MessageBox.Show("经检测存在 ffxiv_dx11.exe 进程，更新卫月需要关闭游戏，需要帮您代劳吗？", "关闭游戏",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Information);
                    if (choice == DialogResult.Yes)
                    {
                        process.Kill();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            CheckUpdate();
        }

        private void comboBoxFFXIV_Clicked(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://qun.qq.com/qqweb/qunpro/share?_wv=3&_wwv=128&inviteCode=CZtWN&from=181074&biz=ka&shareSource=5");
        }

        public readonly string RoamingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XIVLauncherCN");

        private void DeleteLink()
        {
            DeleteSymbolicLink(RoamingPath);

            foreach (var file in Directory.GetFiles(RoamingPath, "*.*", SearchOption.AllDirectories))
            {
                DeleteSymbolicLink(file);
            }

            foreach (var directory in Directory.GetDirectories(RoamingPath, "*", SearchOption.AllDirectories))
            {
                DeleteSymbolicLink(directory);
            }
        }

        public void CheckPath()
        {
            if (!Directory.Exists(Path.Combine(RoamingPath, "addon")))
            {
                Log.Warning($"Moving Roaming to AppData");
                var oldRoamingPath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location)!.FullName, "XIVLauncher");
                if (!Directory.Exists(oldRoamingPath)) return;

                Directory.CreateDirectory(RoamingPath);
                Copy(oldRoamingPath, RoamingPath);
                //Directory.Delete(oldRoamingPath, true);
            }
        }

        private static readonly List<string> Needed = ["addon", "backups", "dalamudAssets", /*"devPlugins",*/ "installedPlugins", "pluginConfigs", "runtime"];

        private static void Copy(string sourcePath, string destPath)
        {
            foreach (var file in Directory.GetFiles(sourcePath))
            {
                var dest = Path.Combine(destPath, Path.GetFileName(file));
                File.Copy(file, dest, true);
            }

            foreach (var directory in Directory.GetDirectories(sourcePath))
            {
                if (sourcePath == Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location)!.FullName, "XIVLauncher"))
                {
                    if (!Needed.Contains(Path.GetFileName(directory)))
                        continue;
                }

                var destDir = Path.Combine(destPath, Path.GetFileName(directory));
                if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
                Copy(directory, destDir);
            }
        }


        private DalamudStartInfo GeneratingDalamudStartInfo(Process process, string dalamudPath, int injectDelay)
        {
            var ffxivDir = Path.GetDirectoryName(process.MainModule.FileName);
            //appDataDir

            var gameVerStr = File.ReadAllText(Path.Combine(ffxivDir, "ffxivgame.ver"));

            var startInfo = new DalamudStartInfo
            {
                ConfigurationPath = Path.Combine(RoamingPath, "dalamudConfig.json"),
                PluginDirectory = Path.Combine(RoamingPath, "installedPlugins"),
                DefaultPluginDirectory = Path.Combine(RoamingPath, "devPlugins"),
                RuntimeDirectory = runtimeDirectory.FullName,
                AssetDirectory = this.dalamudUpdater.AssetDirectory.FullName,
                GameVersion = gameVerStr,
                Language = "4",
                OptOutMbCollection = false,
                WorkingDirectory = dalamudPath,
                DelayInitializeMs = injectDelay
            };

            return startInfo;
        }

        private bool isInjected(Process process)
        {
            try
            {
                for (var j = 0; j < process.Modules.Count; j++)
                {
                    if (process.Modules[j].ModuleName == "Dalamud.dll")
                    {
                        return true;
                    }
                }
            }
            catch
            {

            }
            return false;
        }

        private void DetectSomeShit(Process process)
        {
            try
            {
                for (var j = 0; j < process.Modules.Count; j++)
                {
                    if (process.Modules[j].ModuleName == "ws2detour_x64.dll")
                    {
                        MessageBox.Show("检测到使用网易UU加速器进程模式,有可能注入无反应。\n请使用路由模式。", windowsTitle, MessageBoxButtons.OK);
                    }
                }
            }
            catch
            {
            }
        }

        private bool IsZombieProcess(int pid)
        {
            try
            {
                var process = Process.GetProcessById(pid);
                var mainModule = process.MainModule;
                var handle = SystemHelper.OpenProcess(0x001F0FFF, true, process.Id);
                if (handle == IntPtr.Zero)
                    throw new Exception("ERROR: OpenProcess()");
                SystemHelper.CloseHandle(handle);
            }
            catch (Exception ex)
            {
                MessageBox.Show("""
                    无法访问/打开进程
                    1.请检查安全软件，将Dalamud程序以及相关目录加入白名单
                    2.打开任务管理器，检查是否存在未完全退出且无响应的FFXIV进程,并尝试结束
                    3.尝试重启电脑

                    """ + ex.Message, windowsTitle, MessageBoxButtons.YesNo);
                return true;
            }
            return false;
        }

        private bool Inject(int pid, int injectDelay = 0)
        {
            var process = Process.GetProcessById(pid);
            if (process.ProcessName != "ffxiv_dx11")
            {
                Log.Error("{pid} is not dx11", pid);
                if (MessageBox.Show("此进程并非dx11版FFXIV,无法使用Dalamud。\n解决方法:\n点击确定使用浏览器查看 https://www.yuque.com/ffcafe/act/dx11", windowsTitle, MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    Process.Start("https://www.yuque.com/ffcafe/act/dx11");
                    return false;
                }
            }
            if (IsZombieProcess(pid)) {
                return false;
            }
            if (isInjected(process))
            {
                return false;
            }
            DetectSomeShit(process);
            //var dalamudStartInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(GeneratingDalamudStartInfo(process)));
            //var startInfo = new ProcessStartInfo(injectorFile, $"{pid} {dalamudStartInfo}");
            //startInfo.WorkingDirectory = dalamudPath.FullName;
            //Process.Start(startInfo);
            Log.Information($"[Updater] dalamudUpdater.State:{dalamudUpdater.State}");
            if (dalamudUpdater.State == DalamudUpdater.DownloadState.NoIntegrity)
            {
                if (MessageBox.Show("当前Dalamud版本可能与游戏不兼容,确定注入吗？", windowsTitle, MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return false;
                }
            }
            //return false;
            var dalamudStartInfo = GeneratingDalamudStartInfo(process, Directory.GetParent(dalamudUpdater.Runner.FullName).FullName, injectDelay);
            var environment = new Dictionary<string, string>();
            // No use cuz we're injecting instead of launching, the Dalamud.Boot.dll is reading environment variables from ffxiv_dx11.exe
            /*
            var prevDalamudRuntime = Environment.GetEnvironmentVariable("DALAMUD_RUNTIME");
            if (string.IsNullOrWhiteSpace(prevDalamudRuntime))
                environment.Add("DALAMUD_RUNTIME", runtimeDirectory.FullName);
            */
            WindowsDalamudRunner.Inject(dalamudUpdater.Runner, process.Id, environment, DalamudLoadMethod.DllInject, dalamudStartInfo, this.safeMode);
            return true;
        }

        private void ButtonInject_Click(object sender, EventArgs e)
        {
            if (this.comboBoxFFXIV.SelectedItem != null
                && this.comboBoxFFXIV.SelectedItem.ToString().Length > 0)
            {
                var pidStr = this.comboBoxFFXIV.SelectedItem.ToString();
                if (int.TryParse(pidStr, out var pid))
                {
                    if (Inject(pid))
                    {
                        Log.Information("[DINJECT] Inject finished.");
                    }
                }
                else
                {
                    MessageBox.Show("未能解析游戏进程ID", "找不到游戏",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("未选择游戏进程", "找不到游戏",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

        }
        private void SetAutoRun(bool value)
        {
            string strFilePath = Application.ExecutablePath;
            try
            {
                SystemHelper.SetAutoRun($"\"{strFilePath}\"" + " -startup", "DalamudAutoInjector", value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBoxAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            this.config.AutoStart = checkBoxAutoStart.Checked;
            SetAutoRun(this.config.AutoStart.Value);
            this.config.Save();
        }

        private void checkBoxAutoInject_CheckedChanged(object sender, EventArgs e)
        {
            this.config.AutoInject = checkBoxAutoInject.Checked;
            this.config.Save();
        }

        private void delayBox_ValueChanged(object sender, EventArgs e)
        {
            this.config.InjectDelaySeconds = (double)delayBox.Value;
            this.config.Save();
        }

        private void setProgressBar(int v)
        {
            if (this.toolStripProgressBar1.Owner.InvokeRequired)
            {
                Action<int> actionDelegate = (x) => { toolStripProgressBar1.Value = x; };
                this.toolStripProgressBar1.Owner.Invoke(actionDelegate, v);
            }
            else
            {
                this.toolStripProgressBar1.Value = v;
            }
        }
        private void setStatus(string v)
        {
            if (toolStripStatusLabel1.Owner.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { toolStripStatusLabel1.Text = x; };
                this.toolStripStatusLabel1.Owner.Invoke(actionDelegate, v);
            }
            else
            {
                this.toolStripStatusLabel1.Text = v;
            }
        }
        private void setVisible(bool v)
        {
            if (toolStripProgressBar1.Owner.InvokeRequired)
            {
                Action<bool> actionDelegate = (x) =>
                {
                    toolStripProgressBar1.Visible = x;
                    //toolStripStatusLabel1.Visible = v; 
                };
                this.toolStripStatusLabel1.Owner.Invoke(actionDelegate, v);
            }
            else
            {
                toolStripProgressBar1.Visible = v;
                //toolStripStatusLabel1.Visible = v;
            }
        }

        private bool safeMode = false;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.safeMode = this.checkBoxSafeMode.Checked;
        }

        #region DeleteLink

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetFileAttributesEx(string lpFileName, int fInfoLevelId, out WIN32_FILE_ATTRIBUTE_DATA fileData);

        [StructLayout(LayoutKind.Sequential)]
        private struct WIN32_FILE_ATTRIBUTE_DATA
        {
            public FileAttributes dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
        }

        public static bool IsSymbolicLink(string path)
        {
            if (GetFileAttributesEx(path, 0, out WIN32_FILE_ATTRIBUTE_DATA fileAttributesData))
            {
                return (fileAttributesData.dwFileAttributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint;
            }

            return false;
        }

        public static void DeleteSymbolicLink(string path)
        {
            if (IsSymbolicLink(path))
            {
                // 检查路径是文件还是目录，然后删除
                if (Directory.Exists(path))
                {
                    // 如果是目录符号链接
                    Directory.Delete(path);
                    //Console.WriteLine($"Symbolic link directory '{path}' was deleted.");
                }
                else if (File.Exists(path))
                {
                    // 如果是文件符号链接
                    File.Delete(path);
                    //Console.WriteLine($"Symbolic link file '{path}' was deleted.");
                }
            }
        }

        #endregion

    }
}
