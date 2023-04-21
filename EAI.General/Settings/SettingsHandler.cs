using EAI.General.Files;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EAI.General.Settings
{

    public class SettingsHandler
    {
        private static IConfigurationRoot _configurationRoot;

        /// <summary>
        /// Init settings configuration root
        /// </summary>
        /// <returns></returns>
        public static IConfigurationRoot GetConfigurationRoot()
        {
            lock (typeof(SettingsHandler))
            {

                if (_configurationRoot != null)
                    return _configurationRoot;

                var name = GetSettingFileName();
                var directory = FileHandler.GetCurrentDirectory();
                if (!File.Exists(Path.Combine(directory, name)))
                {
                    directory = FileHandler.GetAssemblyDirectory(typeof(SettingsHandler));
                }


                _configurationRoot = new ConfigurationBuilder()
                                     .SetBasePath(directory)
                                     .AddJsonFile(name, optional: true, reloadOnChange: true)
                                     .AddEnvironmentVariables()
                                     .Build();

                return _configurationRoot;
            }
        }

        private static string GetSettingFileName()
        {
            return "local.settings.json";
        }
    }
}