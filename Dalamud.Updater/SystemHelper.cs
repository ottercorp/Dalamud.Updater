using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Dalamud.Updater.Properties;
using Microsoft.Win32;

namespace Dalamud.Updater
{
    public sealed class SystemHelper
    {
        private SystemHelper() { }
        /// <summary>
            /// 设置程序开机启动
            /// </summary>
            /// <param name="strAppPath">应用程序exe所在文件夹</param>
            /// <param name="strAppName">应用程序exe名称</param>
            /// <param name="bIsAutoRun">自动运行状态</param>
        public static void SetAutoRun(string strAppPath, string strAppName, bool bIsAutoRun)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(strAppPath)
                || string.IsNullOrWhiteSpace(strAppName))
                {
                    throw new Exception("应用程序路径或名称为空！");
                }
                RegistryKey reg = Registry.CurrentUser;
                RegistryKey run = reg.CreateSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\");
                if (bIsAutoRun)
                {
                    run.SetValue(strAppName, strAppPath);
                }
                else
                {
                    if (null != run.GetValue(strAppName))
                    {
                        run.DeleteValue(strAppName);
                    }
                }
                run.Close();
                reg.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
            /// 判断是否开机启动
            /// </summary>
            /// <param name="strAppPath">应用程序路径</param>
            /// <param name="strAppName">应用程序名称</param>
            /// <returns></returns>
        public static bool IsAutoRun(string strAppPath, string strAppName)
        {
            try
            {
                RegistryKey reg = Registry.CurrentUser;
                RegistryKey software = reg.OpenSubKey(@"SOFTWARE");
                RegistryKey run = reg.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\");
                object key = run.GetValue(strAppName);
                software.Close();
                run.Close();
                if (null == key || !strAppPath.Equals(key.ToString()))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public static void Un7za(string path, string output)
        {
            string tempExeName = Path.Combine(Directory.GetCurrentDirectory(), "7za.exe");
            if (File.Exists(tempExeName)) File.Delete(tempExeName);
            using (FileStream fsDst = new FileStream(tempExeName, FileMode.CreateNew, FileAccess.Write))
            {
                byte[] bytes = Resources._7za_exe;

                fsDst.Write(bytes, 0, bytes.Length);
            }

            var sevenzaPath = Path.Combine(Directory.GetCurrentDirectory(), "7za.exe");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                sevenzaPath = "7za";
            }

            var psi = new ProcessStartInfo(sevenzaPath)
            {
                Arguments = $"x -y \"{path}\" -o\"{output}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var tarProcess = Process.Start(psi);
            var outputLines = tarProcess.StandardOutput.ReadToEnd();
            if (tarProcess == null)
                throw new BadImageFormatException("Could not start 7za.");

            tarProcess.WaitForExit();
            if (tarProcess.ExitCode != 0)
                throw new FormatException($"Could not un7z.\n{outputLines}");
        }
    }
}
