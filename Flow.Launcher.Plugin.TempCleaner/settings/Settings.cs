using System.IO;
using System.Text.Json;

namespace Flow.Launcher.Plugin.TempCleaner
{
    public class Settings
    {
        public string TempFolderPath { get; set; }
        internal string SettingsFileLocation { get; set; }

        internal void Save()
        {
            File.WriteAllText(SettingsFileLocation, JsonSerializer.Serialize(this));
        }
    }
}