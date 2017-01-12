using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.KLADR.Service
{
    public interface IHttpLoader
    {
        void LoadFile(string httpPath, string fullFileName);
    }
}
