using System;
using System.IO;
using Newtonsoft.Json;

namespace TrainingB.Configuration
{
    public static class ConfigurationManager
    {
        private static RootConfiguration? _config;
        private static readonly object _lock = new object();

        public static RootConfiguration Config
        {
            get
            {
                if (_config == null)
                {
                    lock (_lock)
                    {
                        if (_config == null)
                        {
                            LoadConfiguration();
                        }
                    }
                }
                return _config!;
            }
        }

        private static void LoadConfiguration()
        {
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                
                if (!File.Exists(configPath))
                {
                    // Create default config if not exists
                    _config = new RootConfiguration();
                    SaveConfiguration();
                    return;
                }

                string json = File.ReadAllText(configPath);
                _config = JsonConvert.DeserializeObject<RootConfiguration>(json) ?? new RootConfiguration();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                _config = new RootConfiguration();
            }
        }

        private static void SaveConfiguration()
        {
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                string json = JsonConvert.SerializeObject(_config, Formatting.Indented);
                File.WriteAllText(configPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving configuration: {ex.Message}");
            }
        }

        public static void ReloadConfiguration()
        {
            lock (_lock)
            {
                _config = null;
            }
        }
    }
}
