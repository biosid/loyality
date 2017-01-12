using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using RapidSoft.GeoPoints.Entities;
using RapidSoft.Loaders.Geocoder.DataSources;
using RapidSoft.Loaders.Geocoder.Entities;
using RapidSoft.Loaders.Geocoder.Exceptions;
using RapidSoft.Loaders.Geocoder.Service;
using RapidSoft.Loaders.Geocoder.Service.Google;
using RapidSoft.Loaders.Geocoder.Service.Yandex;

namespace RapidSoft.Loaders.Geocoder.Logic
{
    public class GeocoderService
    {
        public GeocoderService(IDataSource dataSource, IGeocodingService geoservice, SqlConnection connection, IEtlService etlService, int countInPackage)
        {
            DataSource = dataSource;
            GeocodingService = geoservice;
            Connection = connection;
            EtlService = etlService;
            CountInPackage = countInPackage;
        }

        public IDataSource DataSource { get; private set; }

        public IGeocodingService GeocodingService { get; private set; }

        public IEtlService EtlService { get; private set; }
        
        public SqlConnection Connection { get; private set; }

        public int CountInPackage { get; private set; }

        public void UpdateLocationGeoInfo()
        {
            try
            {
                //var locations = GetLocationWithoutGeoInfo();
                var locations = DataSource.GetLocations();
                var index = 0;
                foreach (var location in locations)
                {
                    try
                    {
                        if (index % CountInPackage == 0)
                        {
                            EtlService.AddMessage(string.Format("Начало обработки {0} пачки ({1} в пачке)", 1 + index / 10000, CountInPackage));
                        }
                        index++;
                        Console.WriteLine("Обработка записи {0} - {1}", index, location.Address);
                        var georesponse = GeocodingService.ResolveAddress(location.Address);

                        var locationInfo = new LocationGeoInfo
                        {
                            CreatedDateTime = DateTime.Now,
                            CreatedUtcDateTime = DateTime.UtcNow,
                            ModifiedDateTime = DateTime.Now,
                            ModifiedUtcDateTime = DateTime.UtcNow,
                            EtlPackageId = EtlService.EtlPackageId,
                            EtlSessionId = EtlService.EtlSessionId,
                            GeoSystem = GeocodingService.ServiceName,
                            GeoDateTime = DateTime.Now,
                        };

                        if (georesponse.Addresses.Length > 0)
                        {
                            if (georesponse.Addresses[0].Accuracy <= GeoCodingAccuracy.City && georesponse.Addresses[0].Accuracy != GeoCodingAccuracy.Unknown)
                            {
                                locationInfo.Lat = georesponse.Addresses[0].Coordinate.Latitude;
                                locationInfo.Lng = georesponse.Addresses[0].Coordinate.Longitude;
                                locationInfo.GeoCodingAccuracy = georesponse.Addresses[0].Accuracy;
                                locationInfo.GeoCodingStatus = GeoCodingStatus.Valid;

                            }
                            else
                            {
                                locationInfo.GeoCodingStatus = GeoCodingStatus.Invalid;
                            }
                        }
                        else
                        {
                            locationInfo.GeoCodingStatus = GeoCodingStatus.Invalid;
                        }

                        DataSource.UpdateGeoInfo(location.Id, locationInfo);

                        if ((index + 1) % CountInPackage == 0)
                        {
                            EtlService.AddMessage(string.Format("Окончание обработки {0} пачки ({1} в пачке)", 1 + index / 10000, CountInPackage));
                        }
                    }
                    catch (GeocodingException e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }

                if ((index + 1) % CountInPackage != 0)
                {
                    EtlService.AddMessage(string.Format("Окончание обработки {0} пачки ({1} в пачке)", index / 10000, CountInPackage));
                }
            } 
            catch (DailyRequestsLimitGeocodingException ex)
            {
                throw;
            }
        }
    }
}