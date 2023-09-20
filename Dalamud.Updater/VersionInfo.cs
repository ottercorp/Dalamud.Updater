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
        [JsonProperty("version")]
        public string AssemblyVersion;
        [JsonProperty("downloadurl")]
        public string DownloadUrl;
        [JsonProperty("changelogurl")]
        public string ChangeLogUrl;
        [JsonProperty("config")]
        public Config Config;
    }
}
