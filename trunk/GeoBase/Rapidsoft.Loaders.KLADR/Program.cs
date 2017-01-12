using System;
using System.Data.SqlTypes;
using System.IO;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Logging.Sql;
using RapidSoft.Loaders.KLADR.Logic;
using RapidSoft.Loaders.KLADR.Service;

using SevenZSharp;

using SevenZip;

namespace RapidSoft.Loaders.KLADR
{
    class Program
    {
        static void Main(string[] args)
        {            
            ProcessingKladr();
        }

        private static void ProcessingKladr()
        {
            var config = new Configuration();
			var textLogger = new TextEtlLogger(Console.Out);
            var sqlLogger = new SqlEtlLogger(config.ProviderName, config.ConnectionString, "dbo");
			var logger = new MultiEtlLogger(textLogger, sqlLogger);

            var etlService = new EtlService(new Guid(config.EtlPackageId), Guid.NewGuid(), logger);
            etlService.StartSession("Начало синхронизации КЛАДР");
            try
            {
                new KladrLoader(config, etlService).Processing();
				etlService.EndSession("Синхронизация КЛАДР успешно завершена", EtlStatus.Succeeded);
            }
            catch (Exception ex)
            {
                etlService.AddMessageWithException(ex);
                etlService.EndSession("Синхронизация КЛАДР завершилась с ошибкой", EtlStatus.Failed);
            }
        }
    }
}
