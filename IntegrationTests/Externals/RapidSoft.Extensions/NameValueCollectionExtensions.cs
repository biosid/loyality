using System;
using System.Collections.Specialized;
using System.Configuration;

namespace RapidSoft.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static int GetIntOrDefault(this NameValueCollection collection, string propertyName, int defaultVal = 0)
        {
            var setting = collection[propertyName];

            int val;
            if (!int.TryParse(setting, out val))
            {
                return defaultVal;
            }
            return val;
        }

        //public static int GetIntOrDefault(this NameValueCollection collection, string propertyName, int defaultVal = 0)
        //{
        //    var setting = collection[propertyName];

        //    int val;
        //    if (!int.TryParse(setting, out val))
        //    {
        //        throw new InvalidOperationException(string.Format("{0} should be numeric", propertyName));
        //    }
        //    return val;
        //}

        //private static string GetSettingOrDefault(this NameValueCollection collection, string propertyName)
        //{
        //    var setting = collection[propertyName];
        //    if (string.IsNullOrEmpty(setting))
        //    {
        //        throw new InvalidOperationException(string.Format("Please set {0} in appSettings", propertyName));
        //    }
        //    return setting;
        //}


        //public static bool GetBoolOrFail(string propertyName)
        //{
        //    var setting = GetSettingOrFail(propertyName);
        //    bool val;
        //    if (!bool.TryParse(setting, out val))
        //    {
        //        throw new ConfigurationErrorsException(string.Format("{0} should be numeric", propertyName));
        //    }
        //    return val;
        //}
    }
}
