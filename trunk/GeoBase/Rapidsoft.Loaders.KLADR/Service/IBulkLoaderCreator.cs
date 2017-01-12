using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.KLADR.Service
{
    public interface IBulkLoaderCreator
    {
        IBulkLoader Create(string dbfFolderName, string dbfTableName, string sqlConnectionString, string sqlTableName);
    }
}
