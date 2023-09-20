using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Serilog;
using XIVLauncher.Common.Dalamud;


public static class WindowsDalamudRunner
{
    public static void Inject(FileInfo runner, int gamePid, IDictionary<string, string> environment, DalamudLoadMethod loadMethod, DalamudStartInfo startInfo,bool safeMode = false)
    {
        // Process process = Process.GetProcessById(gamePid);
        // var gamePath = Path.Combine(process.MainModule.FileName, "..", "..");
        var launchArguments = new List<string>
        {
            "inject -v",
            $"{gamePid}",
            //$"--all --warn",
            //$"--game=\"{gamePath}\"",
            $"--dalamud-working-directory=\"{startInfo.WorkingDirectory}\"",
            $"--dalamud-configuration-path=\"{startInfo.ConfigurationPath}\"",
            $"--dalamud-plugin-directory=\"{startInfo.PluginDirectory}\"",
            $"--dalamud-dev-plugin-directory=\"{startInfo.DefaultPluginDirectory}\"",
            $"--dalamud-asset-directory=\"{startInfo.AssetDirectory}\"",
            $"--dalamud-client-language={startInfo.Language}",
            $"--dalamud-delay-initialize={startInfo.DelayInitializeMs}"
        };

        if (safeMode) launchArguments.Add("--no-plugin");

        var psi = new ProcessStartInfo(runner.FullName) {
            Arguments = string.Join(" ", launchArguments),
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var keyValuePair in environment)
        {
            if (psi.EnvironmentVariables.ContainsKey(keyValuePair.Key))
                psi.EnvironmentVariables[keyValuePair.Key] = keyValuePair.Value;
            else
                psi.EnvironmentVariables.Add(keyValuePair.Key, keyValuePair.Value);
        }

        // psi.EnvironmentVariables.Add("DALAMUD_RUNTIME", startInfo.RuntimeDirectory);

        var dalamudProcess = Process.Start(psi);
        while (!dalamudProcess.StandardOutput.EndOfStream)
        {
            string line = dalamudProcess.StandardOutput.ReadLine();
            Log.Information(line);
        }
    }

    /// <summary>
    /// DUPLICATE_* values for DuplicateHandle's dwDesiredAccess.
    /// </summary>
    [Flags]
    private enum DuplicateOptions : uint {
        /// <summary>
        /// Closes the source handle. This occurs regardless of any error status returned.
        /// </summary>
        CloseSource = 0x00000001,

        /// <summary>
        /// Ignores the dwDesiredAccess parameter. The duplicate handle has the same access as the source handle.
        /// </summary>
        SameAccess = 0x00000002,
    }

    /// <summary>
    /// Duplicates an object handle.
    /// </summary>
    /// <param name="hSourceProcessHandle">
    /// A handle to the process with the handle to be duplicated.
    ///
    /// The handle must have the PROCESS_DUP_HANDLE access right.
    /// </param>
    /// <param name="hSourceHandle">
    /// The handle to be duplicated. This is an open object handle that is valid in the context of the source process.
    /// For a list of objects whose handles can be duplicated, see the following Remarks section.
    /// </param>
    /// <param name="hTargetProcessHandle">
    /// A handle to the process that is to receive the duplicated handle.
    ///
    /// The handle must have the PROCESS_DUP_HANDLE access right.
    /// </param>
    /// <param name="lpTargetHandle">
    /// A pointer to a variable that receives the duplicate handle. This handle value is valid in the context of the target process.
    ///
    /// If hSourceHandle is a pseudo handle returned by GetCurrentProcess or GetCurrentThread, DuplicateHandle converts it to a real handle to a process or thread, respectively.
    ///
    /// If lpTargetHandle is NULL, the function duplicates the handle, but does not return the duplicate handle value to the caller. This behavior exists only for backward compatibility with previous versions of this function. You should not use this feature, as you will lose system resources until the target process terminates.
    ///
    /// This parameter is ignored if hTargetProcessHandle is NULL.
    /// </param>
    /// <param name="dwDesiredAccess">
    /// The access requested for the new handle. For the flags that can be specified for each object type, see the following Remarks section.
    ///
    /// This parameter is ignored if the dwOptions parameter specifies the DUPLICATE_SAME_ACCESS flag. Otherwise, the flags that can be specified depend on the type of object whose handle is to be duplicated.
    ///
    /// This parameter is ignored if hTargetProcessHandle is NULL.
    /// </param>
    /// <param name="bInheritHandle">
    /// A variable that indicates whether the handle is inheritable. If TRUE, the duplicate handle can be inherited by new processes created by the target process. If FALSE, the new handle cannot be inherited.
    ///
    /// This parameter is ignored if hTargetProcessHandle is NULL.
    /// </param>
    /// <param name="dwOptions">
    /// Optional actions.
    /// </param>
    /// <returns>
    /// If the function succeeds, the return value is nonzero.
    ///
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
    /// </returns>
    /// <remarks>
    /// See https://docs.microsoft.com/en-us/windows/win32/api/handleapi/nf-handleapi-duplicatehandle.
    /// </remarks>
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool DuplicateHandle(
        IntPtr hSourceProcessHandle,
        IntPtr hSourceHandle,
        IntPtr hTargetProcessHandle,
        out IntPtr lpTargetHandle,
        uint dwDesiredAccess,
        [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
        DuplicateOptions dwOptions);
}
