using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.KLADR.Service
{
    public class BulkLoaderCreator : IBulkLoaderCreator
    {
        public IBulkLoader Create(string dbfFolderName, string dbfTableName, string sqlConnectionString, string sqlTableName)
        {
            return new BulkLoader(dbfFolderName, dbfTableName, sqlConnectionString, sqlTableName);
        }
    }
}
