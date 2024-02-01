using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;

#pragma warning disable RCS1075

namespace Flow.Launcher.Plugin.TempCleaner
{
    public class Main : IPlugin, ISettingProvider
    {
        private Settings _settings;
        private const string ICON_PATH = @"Images\icon.png";
        private const string FOLDER_NAME = "Flow.Launcher.Plugin.TempCleaner";

        public Control CreateSettingPanel()
        {
            return new TempCleanerSettings(_settings);
        }

        public void Init(PluginInitContext context)
        {
            LoadSettings(context);
        }

        private void LoadSettings(PluginInitContext context)
        {
            var flowLauncherFolder = Directory.GetParent($@"{context.CurrentPluginMetadata.PluginDirectory}\..").FullName;
            var folderAppdata = Directory.GetParent($@"{context.CurrentPluginMetadata.PluginDirectory}\..\..\..").FullName;

            var settingsFolderLocation = Path.Combine(flowLauncherFolder, "Settings", "Plugins", FOLDER_NAME);
            if (!Directory.Exists(settingsFolderLocation))
                Directory.CreateDirectory(settingsFolderLocation);

            var settingsFileLocation = Path.Combine(settingsFolderLocation, "Settings.json");
            if (!File.Exists(settingsFileLocation))
            {
                var tempFolder = Path.Combine(folderAppdata, "Local", "Temp");
                _settings = new Settings
                {
                    SettingsFileLocation = settingsFileLocation,
                    TempFolderPath = tempFolder
                };
                _settings.Save();
            }
            else
            {
                _settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(settingsFileLocation));
                _settings.SettingsFileLocation = settingsFileLocation;
            }
        }

        public List<Result> Query(Query query)
        {
            return new List<Result>
            {
               new() {
                   Title = "Temp Cleaner",
                   SubTitle = "Clean up your temp folder",
                   IcoPath = ICON_PATH,
                   Action = (ActionContext _) => RemoveFiles()
               }
            };
        }

        private bool RemoveFiles()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_settings.TempFolderPath))
                    return true;

                foreach (var path in _settings.TempFolderPath.Split(";"))
                {
                    var directoryInfo = new DirectoryInfo(path);
                    foreach (var file in directoryInfo.GetFiles())
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (Exception) { }
                    }

                    foreach (var dir in directoryInfo.GetDirectories())
                    {
                        try
                        {
                            dir.Delete(true);
                        }
                        catch (Exception) { }
                    }
                }
            }
            catch (Exception) { }

            return true;
        }
    }
}