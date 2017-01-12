using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidSoft.Loaylty.PartnersConnector.Common.Interfaces;

namespace RapidSoft.Loaylty.PartnersConnector.Common
{
    public class FileSystem : IFileSystem
    {
        public IEnumerable<string> EnumerateDirectories(string path)
        {
            return Directory.EnumerateDirectories(path);
        }

        public void DeleteDirectory(string path, bool isrecursive = false)
        {
            Directory.Delete(path, isrecursive);
        }
    }
}
