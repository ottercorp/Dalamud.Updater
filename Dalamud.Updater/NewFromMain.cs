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
    public partial class NewFromMain : Form
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
        private int injectTimes = 0;
        private bool isCheckingUpdate = false;

        private void CheckUpdate()
        {
            isCheckingUpdate = true;
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



        public NewFromMain()
        {
            UpdateSelf();
            InitializeComponent();

            SetAutoRun(false);
        }

        private Version GetUpdaterVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
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
#if false
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
                            //this.checkBoxSafeMode.Invoke(UpdateFormConfig);
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

        private void FormMain_Load(object sender, EventArgs e)
        {
        }
        private void FormMain_Disposed(object sender, EventArgs e)
        {
            this.isThreadRunning = false;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://qun.qq.com/qqweb/qunpro/share?_wv=3&_wwv=128&inviteCode=CZtWN&from=181074&biz=ka&shareSource=5");
        }

        private void label2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://aonyx.ffxiv.wang/");
        }

        private void label3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://dotnet.microsoft.com/zh-cn/download/dotnet/thank-you/runtime-desktop-8.0.13-windows-x64-installer");
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
    }
}
