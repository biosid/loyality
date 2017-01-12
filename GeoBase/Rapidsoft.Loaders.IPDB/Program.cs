using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RapidSoft.Loaders.IPDB.Properties;

namespace RapidSoft.Loaders.IPDB
{
    public class Program
    {
        static void Main(string[] args)
        {
            LoaderConfiguration.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InformationServicesDB"].ConnectionString;
            LoaderConfiguration.IpDbLink = Settings.Default.IpDbLink;
            LoaderConfiguration.PackID = new Guid(Settings.Default.PackID);
            LoaderConfiguration.TempFolderPath = Settings.Default.TempFolderPath;

            IpDbLoader loader = new IpDbLoader();
            loader.Load();
        }
    }
}
