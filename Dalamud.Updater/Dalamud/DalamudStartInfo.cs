using System;

namespace XIVLauncher.Common.Dalamud
{
    [Serializable]
    public sealed class DalamudStartInfo
    {
        public string WorkingDirectory;
        public string ConfigurationPath;
        public string PluginDirectory;
        public string RuntimeDirectory;
        public string DefaultPluginDirectory;
        public string AssetDirectory;
        public string Language;
        public int DelayInitializeMs;

        public string GameVersion;

        public bool OptOutMbCollection;

        public bool GlobalAccelerate;
    }
}
