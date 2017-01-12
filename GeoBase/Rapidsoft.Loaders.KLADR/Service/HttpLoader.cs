using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace RapidSoft.Loaders.KLADR.Service
{
    public class HttpLoader : IHttpLoader
    {
        public void LoadFile(string httpPath, string fullFileName)
        {
            var uri = new Uri(httpPath);
            
            var myWebClient = new WebClient();
            myWebClient.DownloadFile(uri, fullFileName);
        }
    }
}
