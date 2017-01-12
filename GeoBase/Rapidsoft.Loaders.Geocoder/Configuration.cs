using System;
using System.Configuration;
using RapidSoft.Loaders.Geocoder.Logic;

namespace RapidSoft.Loaders.Geocoder
{
    public class Configuration : IConfiguration, IYandexConfiguration
    {
        public Configuration()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["InformationServicesDB"].ConnectionString;
            ProviderName = ConfigurationManager.ConnectionStrings["InformationServicesDB"].ProviderName;

            EtlPackageId = ConfigurationManager.AppSettings["EtlPackageId"];
            YandexKey = ConfigurationManager.AppSettings["YandexKey"];
            SetValueOrDefault("RequestInterval", s => RequestInterval = int.Parse(s), () => RequestInterval = 100);
            SetValueOrDefault("CountInPackage", s => CountInPackage = int.Parse(s), () => CountInPackage = 10000);
            GeoCodingService = ConfigurationManager.AppSettings["GeoCodingService"];
            DataSource = ConfigurationManager.AppSettings["DataSource"];
            SourceFile = ConfigurationManager.AppSettings["SourceFile"];
        }

        public string ConnectionString { get; private set; }

        public string ProviderName { get; private set; }

        public string EtlPackageId { get; private set; }

        public int CountInPackage { get; private set; }

        public string YandexKey { get; private set; }

        public int RequestInterval { get; private set; }

        public string GeoCodingService { get; private set; }

        public string DataSource { get; private set; }

        public string SourceFile { get; private set; }

        private void SetValueOrDefault(string key, Action<string> setter, Action setDefault)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                setter(ConfigurationManager.AppSettings[key]);
            }
            else
            {
                setDefault();
            }
        }
    }
}
