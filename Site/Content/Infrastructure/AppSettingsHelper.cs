using System.Configuration;

namespace Vtb24.Site.Content.Infrastructure
{
    internal static class AppSettingsHelper
    {
        public static int Int(string key, int defaultValue)
        {
            int value;
            if (!int.TryParse(ConfigurationManager.AppSettings[key], out value))
            {
                value = defaultValue;
            }
            return value;
        }

        public static string String(string key, string defaultValue)
        {
            return ConfigurationManager.AppSettings[key] ?? defaultValue;
        }
    }
}