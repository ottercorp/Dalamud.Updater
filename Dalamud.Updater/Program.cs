using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dalamud.Updater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] strArgs = Environment.GetCommandLineArgs();
            if (!IsAdministrator())
            {
                // Restart and run as admin
                var exeName = Process.GetCurrentProcess().MainModule.FileName;
                ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                startInfo.Verb = "runas";
                var args = strArgs.Skip(1).ToList();
                //if (strArgs.Length >= 2 && strArgs[1].Equals("-startup"))
                //{
                //    startInfo.Arguments = "-startup";
                //}
                args.Add("-no_mutex");
                startInfo.Arguments = string.Join(" ", args);
                startInfo.WorkingDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
                Process.Start(startInfo);
                //Thread.Sleep(10000);
                Application.Exit();
                return;
            }
            if (!strArgs.Contains("-no_mutex") && ProcessMutexInstance()) {
                //MessageBox.Show("Exists!");
                Application.Exit();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form form = new FormMain();
            if (strArgs.Length >= 2 && strArgs[1].Equals("-startup"))
            {
                form.Opacity = 0;
                form.ShowInTaskbar = false;
                form.Show();
                form.Visible = false;
                form.Opacity = 1;
                form.ShowInTaskbar = true;
            } else
            {
                form.Show();
            }
            Application.Run();
        }

        static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static bool ProcessMutexInstance() {
            Process current = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(current.ProcessName).Where(x => (x.Id != current.Id && x.MainModule.FileName == current.MainModule.FileName));
            //var processes = Process.GetProcessesByName(current.ProcessName).Where(x => (x.Id != current.Id));
            if (processes.ToList().Count == 0)
                return false;
            foreach (var p in processes) { 
                var hWnd = p.MainWindowHandle;
                if (hWnd == IntPtr.Zero) {
                    hWnd = FindWindow(null,"卫月更新器");
                    GetWindowThreadProcessId(hWnd,out var pid);
                    if (pid == p.Id) {
                        ShowWindow(hWnd, 5);
                        ShowWindow(hWnd, 1);
                    }
                } else
                    ShowWindow(hWnd, 1);
                SetForegroundWindow(hWnd);
            }
            return true;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string className, string frmText);
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        private static extern int ShowWindow(IntPtr hwnd, int showWay);
        [DllImport("user32.dll ", SetLastError = true)]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
    }
}
