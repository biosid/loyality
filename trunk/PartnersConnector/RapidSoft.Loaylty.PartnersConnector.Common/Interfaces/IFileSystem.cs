using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidSoft.Loaylty.PartnersConnector.Common.Interfaces
{
    public interface IFileSystem
    {
        IEnumerable<string> EnumerateDirectories(string path);

        void DeleteDirectory(string path, bool isrecursive = false);
    }
}
