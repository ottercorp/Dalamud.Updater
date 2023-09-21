using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dalamud.Updater
{
    internal class VersionInfo
    {
        public string Version;
        public string DownloadUrl;
        public string ChangeLog;
        public Config Config;
    }
}
