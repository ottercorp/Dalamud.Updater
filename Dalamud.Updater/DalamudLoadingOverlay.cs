using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using XIVLauncher.Common.Dalamud;

namespace Dalamud.Updater
{
    internal class DalamudLoadingOverlay : IDalamudLoadingOverlay
    {
        public delegate void progressBar(int value);
        public delegate void statusLabel(string value);
        public delegate void setVisible(bool value);
        public event progressBar OnProgressBar;
        public event statusLabel OnStatusLabel;
        public event setVisible OnSetVisible;
        public DalamudLoadingOverlay(FormMain form)
        {
            //this.progressBar = form.toolStripProgressBar1;
            //this.statusLabel = form.toolStripStatusLabel1;
        }
        public void ReportProgress(long? size, long downloaded, double? progress)
        {
            size = size ?? 0;
            progress = progress ?? 0;
            OnProgressBar?.Invoke((int)progress.Value);
        }

        public void SetInvisible()
        {
            OnSetVisible?.Invoke(false);
        }

        public void SetStep(IDalamudLoadingOverlay.DalamudUpdateStep progress)
        {
            switch (progress)
            {
                case IDalamudLoadingOverlay.DalamudUpdateStep.Dalamud:
                    OnStatusLabel?.Invoke("正在更新核心……");
                    break;

                case IDalamudLoadingOverlay.DalamudUpdateStep.Assets:
                    OnStatusLabel?.Invoke("正在更新资源文件...");
                    break;

                case IDalamudLoadingOverlay.DalamudUpdateStep.Runtime:
                    OnStatusLabel?.Invoke("正在更新运行库...");
                    break;

                case IDalamudLoadingOverlay.DalamudUpdateStep.Unavailable:
                    OnStatusLabel?.Invoke("由于游戏更新，插件目前无法使用。");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(progress), progress, null);
            }
        }

        public void SetVisible()
        {
            OnSetVisible?.Invoke(true);
        }
    }
}
