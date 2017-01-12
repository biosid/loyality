using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Logging.Sql;
using RapidSoft.Geo.Points.Geocoder.Service;
using RapidSoft.Loaders.Geocoder.DataSources;
using RapidSoft.Loaders.Geocoder.Entities;
using RapidSoft.Loaders.Geocoder.Logic;
using RapidSoft.Loaders.Geocoder.Service;
using RapidSoft.Loaders.Geocoder.Service.Google;
using RapidSoft.Loaders.Geocoder.Service.Yandex;

namespace RapidSoft.Loaders.Geocoder
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
			var textlogger = new TextEtlLogger(Console.Out);
            var sqllogger = new SqlEtlLogger(config.ProviderName, config.ConnectionString, "dbo");
			var logger = new MultiEtlLogger(sqllogger, textlogger);

            var etlService = new EtlService(new Guid(config.EtlPackageId), Guid.NewGuid(), logger);
            etlService.StartSession("Запуск процедуры обновления таблицы геокодирования");

            try
            {
                using (var connection = new SqlConnection(config.ConnectionString))
                {
                    connection.Open();

                    //var geocodingService = new GoogleGeocodingService {RequestInterval = config.RequestInterval};
                    var geocodingService = GeoCodingServiceManager.GetGeocodingService(config.GeoCodingService);
                    if (geocodingService == null)
                        throw new Exception(String.Format("Unable to find {0} geocoding service", config.GeoCodingService));
                    var lowLevelService = new ServiceCachingDecorator(geocodingService, connection, String.Format(GeocodingCache.GeocodingCacheTablePattern, config.GeoCodingService));
                    var dataSource = DataSourceManager.GetDataSource(config.DataSource);
                    if (dataSource == null)
                        throw new Exception(String.Format("Unable to find {0} data source", config.DataSource));
                    var service = new GeocoderService(dataSource, lowLevelService, connection, etlService, config.CountInPackage);
                    service.UpdateLocationGeoInfo();
                }
                etlService.EndSession("Процедура обновления геокодирования завершена", EtlStatus.Succeeded);
            }
            catch (Exception ex)
            {
                etlService.AddMessageWithException(ex);
                etlService.EndSession("Процедура обновления геокодирования завершена с ошибкой", EtlStatus.Failed);
            }
        }
    }
}
