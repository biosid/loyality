using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.KLADR.Service
{
    public interface IBulkLoader
    {
        string DbfFolderName { get; }

        string DbfTableName { get; }

        string SqlTableName { get; }

        string SqlSchemaName { get; }

        string SqlConnectionString { get; }

        string DbfConnectionString { get; }

        void Processing();
    }
}
