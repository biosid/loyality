using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RapidSoft.Etl;
using RapidSoft.Loaders.KLADR.Exceptions;
using RapidSoft.Loaders.KLADR.Service;
using RapidSoft.Loaders.KLADR.Utils;
using SevenZSharp;

namespace RapidSoft.Loaders.KLADR.Logic
{
    using System.Configuration;

    public class KladrLoader
    {
        #region Fields

        public readonly static string KLAND_DBF = "KLADR.DBF";

        public readonly static string STREET_DBF = "STREET.DBF";

        public readonly static string ALTNAMES_DBF = "ALTNAMES.DBF";

        public readonly static string KLAND_SQL = "[Geopoints].[BUFFER_KLADR]";

        public readonly static string STREET_SQL = "[Geopoints].[BUFFER_STREET]";

        public readonly static string ALTNAMES_SQL = "[Geopoints].[BUFFER_ALTNAMES]";

        #endregion

        public KladrLoader(IConfiguration configuration, IEtlService etlService) : this(configuration, etlService ,new HttpLoader(), new BulkLoaderCreator(), new SqlUtils(configuration)) {}

        public KladrLoader(IConfiguration configuration, IEtlService etlService, IHttpLoader httpLoader, IBulkLoaderCreator bulkLoaderCreator, ISqlUtils sqlUtils)
        {
            EtlService = etlService;

            Configuration = configuration;
            HttpLoader = httpLoader;
            BulkLoaderCreator = bulkLoaderCreator;
            SqlUtils = sqlUtils;

            UniqueKey = Guid.NewGuid().ToString();
            DateKey = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            TempFolderPath = Path.Combine(Configuration.TempFolderPath, string.Format(Configuration.TempFolderTemplateName, UniqueKey));
            ArchiveFileName = Path.GetFileName(Configuration.HttpFilePath);
        }

        #region Property

        public IConfiguration Configuration { get; private set; }

        public IHttpLoader HttpLoader { get; private set; }

        public IBulkLoaderCreator BulkLoaderCreator { get; private set; }

        public ISqlUtils SqlUtils { get; private set; }

        public string TempFolderPath { get; private set; }

        public string UniqueKey { get; private set; }
        
        public string DateKey { get; private set; }

        public string ArchiveFileName { get; private set; }

        public IEtlService EtlService { get; private set; }

        #endregion

        public void Processing()
        {
            EtlService.AddMessage("Начало скачивания данных КЛАДР");
            DownloadArchive();
            EtlService.AddMessage("Окончание скачивания данных КЛАДР");
            EtlService.AddMessage("Начало генерации временных таблиц");
            GenerateSqlTable();
            EtlService.AddMessage("Окончание генерации временных таблиц");
            EtlService.AddMessage("Начало загрузки данных КЛАДР в БД");
            ProcessingArchive();
            EtlService.AddMessage("Окончание загрузки данных КЛАДР в БД");
            EtlService.AddMessage("Начало обработки КЛАДР");
            ProcessingSqlData();
            EtlService.AddMessage("Окончание обработки КЛАДР");
            EtlService.AddMessage("Начало изменения источника таблицы Locality");
            CompleteProcessing();
            EtlService.AddMessage("Окончание изменения источника таблицы Locality");
        }

        private void DownloadArchive()
        {
            if (!Directory.Exists(TempFolderPath))
            {
                Directory.CreateDirectory(TempFolderPath);
            }

            int count = 0;
            while (true)
            {
                try
                {
                    HttpLoader.LoadFile(Configuration.HttpFilePath, Path.Combine(TempFolderPath, ArchiveFileName));
                }
                catch
                {
                    if (count < Configuration.DownloadAttemptCount)
                    {
                        count++;
                        continue;
                    }
                    throw;
                }
                return;
            }
        }

        private void GenerateSqlTable()
        {
            SqlUtils.Execute(string.Format(@"exec [Geopoints].[RecreateBaseKlandTable] '{0}', '{1}'", EtlService.EtlPackageId, EtlService.EtlSessionId));
        }

        private void ProcessingArchive()
        {
            var inFile = Path.Combine(TempFolderPath, ArchiveFileName);

            EtlService.AddMessage(string.Format("Декодирую файл {0} в папку {1}", inFile, TempFolderPath));

            CompressionEngine.SetOptions(ConfigurationManager.AppSettings["7ZipFilePath"]);
            CompressionEngine.Current.Decoder.DecodeIntoDirectory(inFile, TempFolderPath);

            var kladrDbfUploader = BulkLoaderCreator.Create(TempFolderPath, KLAND_DBF, Configuration.ConnectionString, KLAND_SQL);
            kladrDbfUploader.Processing();

            var streetDbfUploader = BulkLoaderCreator.Create(TempFolderPath, STREET_DBF, Configuration.ConnectionString, STREET_SQL);
            streetDbfUploader.Processing();
            
            var altnamesDbfUploader = BulkLoaderCreator.Create(TempFolderPath, ALTNAMES_DBF, Configuration.ConnectionString, ALTNAMES_SQL);
            altnamesDbfUploader.Processing();
        }

        private void ProcessingSqlData()
        {
            //Processing data
            SqlUtils.ExecuteScalar(string.Format("exec [Geopoints].[ProcessingKladr] '{0}', '{1}'", EtlService.EtlPackageId, EtlService.EtlSessionId));

            //Save data to Locality table
            SqlUtils.ExecuteScalar(string.Format("exec [Geopoints].[SaveDataToLocality] '{0}', '{1}', '{2}'", EtlService.EtlPackageId, EtlService.EtlSessionId, DateKey));
        }

        private void CompleteProcessing()
        {
            var count = 0;
            while (true)
            {
                try
                {
                    SqlUtils.ExecuteScalar(string.Format("exec [Geopoints].[RemapKladrView] '{0}'", DateKey));
                }
                catch
                {
                    if (count < Configuration.ChangeViewAttemptCount)
                    {
                        count++;
                        continue;
                    }
                    throw;
                }

                return;
            }
        }
    }
}
