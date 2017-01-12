using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.KLADR
{
    public interface IConfiguration
    {
        string ConnectionString { get; }

        string ProviderName { get; }

        int DbTimeout { get; }

        string HttpFilePath { get; }

        string TempFolderPath { get; }

        string TempFolderTemplateName { get; }

        int DownloadAttemptCount { get; }
        
        int ChangeViewAttemptCount { get; }

    }
}
