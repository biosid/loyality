using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.KLADR
{
    public class Configuration : IConfiguration
    {
        public Configuration()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["InformationServicesDB"].ConnectionString;
            ProviderName = ConfigurationManager.ConnectionStrings["InformationServicesDB"].ProviderName;

            EtlPackageId = ConfigurationManager.AppSettings["EtlPackageId"];
            HttpFilePath = ConfigurationManager.AppSettings["HttpFilePath"];
            var tempFolderPath = ConfigurationManager.AppSettings["TempFolderPath"];

            if (string.IsNullOrEmpty(tempFolderPath))
            {
                tempFolderPath = Path.GetTempPath();
            }
            TempFolderPath = tempFolderPath;
            TempFolderTemplateName = ConfigurationManager.AppSettings["TempFolderTemplateName"];

            SetValueOrDefault("DownloadAttemptCount", s => DownloadAttemptCount = int.Parse(s), () => DownloadAttemptCount = 10);
            SetValueOrDefault("ChangeViewAttemptCount", s => ChangeViewAttemptCount = int.Parse(s), () => ChangeViewAttemptCount = 50);
            SetValueOrDefault("ConnectionTimeout", s => DbTimeout = int.Parse(s), () => DbTimeout = 600);
        }

        public string EtlPackageId { get; private set; }

        public int DbTimeout { get; private set; }

        public string ConnectionString { get; private set; }

        public string ProviderName { get; private set; }

        public string HttpFilePath { get; private set; }

        public string TempFolderPath { get; private set; }

        public string TempFolderTemplateName { get; private set; }

        public int DownloadAttemptCount { get; private set; }

        public int ChangeViewAttemptCount { get; private set; }

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
