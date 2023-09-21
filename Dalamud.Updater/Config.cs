using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Serilog;

namespace Dalamud.Updater
{
    public class Config
    {
        public bool? AutoInject { get; set; } = null;
        public bool? AutoStart { get; set; } = null;
        public double? InjectDelaySeconds { get; set; } = null;
        public bool? SafeMode { get; set; } = null; 

        private static readonly JsonSerializerSettings SerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            Formatting = Formatting.Indented,
        };

        [JsonIgnore]
        private string configPath;
        public void Save() {
            try
            {
                File.WriteAllText(this.configPath, JsonConvert.SerializeObject(this, SerializerSettings));
                Log.Information( "Save Config at {0}", this.configPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save Config at {0}", this.configPath);
            }   
        }
        public static Config Load(string path) {
            Log.Debug("Try to parse config: {path}",path);
            Config deserialized = null;
            try {
                deserialized = JsonConvert.DeserializeObject<Config>(File.ReadAllText(path), SerializerSettings);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load Config at {0}", path);
            }
            deserialized ??= new Config() { 
                AutoInject = false,
                AutoStart = false,
                InjectDelaySeconds = 0,
                SafeMode = false,
            };
            //Log.Information(JsonConvert.SerializeObject(deserialized), SerializerSettings);
            deserialized.configPath = path;
            return deserialized;
        }
    }
}
