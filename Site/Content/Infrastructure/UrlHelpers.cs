using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vtb24.Site.Content.Infrastructure
{
    public static class UrlHelpers
    {
        public static string NormalizeUrl(string url)
        {
            return url.StartsWith("/") ? url : string.Format("/{0}", url);
        }
    }
}
