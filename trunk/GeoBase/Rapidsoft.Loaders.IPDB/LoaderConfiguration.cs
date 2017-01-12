using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RapidSoft.Loaders.IPDB
{
    public class LoaderConfiguration
    {
        public static string ConnectionString { get; set; }
        public static string IpDbLink { get; set; }
        public static Guid PackID { get; set; }
        public static string TempFolderPath { get; set; }
    }
}

