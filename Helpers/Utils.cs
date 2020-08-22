using System.Configuration;

namespace CryptographyHelperUtil.Helpers
{
    public class Utils
    {
        public static Configuration OpenConfigurationFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return null;

            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = filePath;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            return config;

        }
    }
}
