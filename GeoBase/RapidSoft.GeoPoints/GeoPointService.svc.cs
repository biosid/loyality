using RapidSoft.Loaylty.Logging;

namespace RapidSoft.GeoPoints
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Loaylty.Monitoring;

    using RapidSoft.GeoPoints.Entities;
    using RapidSoft.GeoPoints.OutputResults;
    using RapidSoft.GeoPoints.Repositories;
    using RapidSoft.Kladr.Model;
    using RapidSoft.Loaylty.Logging.Wcf;

    [LoggingBehavior]
    public class GeoPointService : SupportService, IGeoPointService
    {
        private readonly ILog log = LogManager.GetLogger(typeof (GeoPointService));

        public KladrAddressResult GetAddressByKladrCode(string KladrCode)
        {
            try
            {
                var rep = new LocationRepository();
                var location = rep.GetLocationByKladrCode(KladrCode);

                var kladrAddress = location.ToKladrAddress();

                kladrAddress.FullText = AddressStringConverter.GetAddressText(kladrAddress, AddressLevel.Town);
                
                return KladrAddressResult.BuildSuccess(kladrAddress);
            }
            catch (Exception ex)
            {
                log.Error("ошибка GetAddressByKladrCode", ex);
                return ServiceOperationResult.BuildErrorResult<KladrAddressResult>(ex);
            }
        }

        public CheckKladrCodeResult GetExistKladrCodes(string[] KladrCodes)
        {
            try
            {
                var rep = new LocationRepository();
                var exists = rep.GetExistKladrCodes(KladrCodes);
                return CheckKladrCodeResult.BuildSuccess(exists);
            }
            catch (Exception ex)
            {
                log.Error("ошибка GetExistKladrCodes", ex);
                return ServiceOperationResult.BuildErrorResult<CheckKladrCodeResult>(ex);
            }
        }

        public CountryListResult GetCountriesNamesByCode(List<string> codes)
        {
            try
            {
                var rep = new LocationRepository();
                var list = rep.GetCountriesByAlpha2Codes(codes);
                return CountryListResult.BuildSuccess(list);
            }
            catch (Exception ex)
            {
                log.Error("ошибка GetCountriesNamesByCode", ex);
                return ServiceOperationResult.BuildErrorResult<CountryListResult>(ex);
            }
        }

        public List<Location> GetCountries(string NameSearchPattern, int? Skip, int? Top)
        {
            try
            {
                var rep = new LocationRepository();
                return rep.GetCountries(null, NameSearchPattern, Skip, Top);
            }
            catch (Exception ex)
            {
                log.Error("ошибка GetCountries", ex);
                throw;
            }
        }

        public List<Location> GetLocationsByParent(Guid ParentId, int? LocationType, string NameSearchPattern, int? Skip, int? Top)
        {
            try
            {
                var rep = new LocationRepository();
                return rep.GetLocationsByParent(ParentId, LocationType, NameSearchPattern, null, Skip, Top);
            }
            catch (Exception ex)
            {
                log.Error("ошибка GetLocationsByParent", ex);
                throw;
            }
        }

        public GetLocationsResult GetLocationsByKladrCode(
            string ParentKladrCode, int[] LocationType, string[] Toponyms, string NameSearchPattern, bool? RegionIsCityOnly, int? Skip, int? Top)
        {
            try
            {
                var rep = new LocationRepository();

                var locations = rep.GetLocations(ParentKladrCode, LocationType, Toponyms, NameSearchPattern, RegionIsCityOnly, Skip, Top);

                return new GetLocationsResult { ResultCode = ResultCodes.SUCCESS, Locations = locations };
            }
            catch (Exception ex)
            {
                log.Error("ошибка GetLocationsByKladrCode", ex);
                return ServiceOperationResult.BuildErrorResult<GetLocationsResult>(ex);
            }
        }

        public List<Location> GetLocationsByIP(string IP, int LocationType, int? Skip, int? Top)
        {
            try
            {
                var rep = new LocationRepository();
                return rep.GetLocationsByIP(IP, LocationType, null, Skip, Top);
            }
            catch (Exception ex)
            {
                log.Error("ошибка GetLocationsByIP", ex);
                throw;
            }
        }

        public Location GetLocationByIP(string IP)
        {
            try
            {
                var rep = new LocationRepository();
                return rep.GetLocationByIP(IP);
            }
            catch (Exception ex)
            {
                log.Error("ошибка GetLocationByIP", ex);
                throw;
            }
        }

        public Location GetLocationById(Guid Id)
        {
            try
            {
                var rep = new LocationRepository();
                return rep.GetLocationById(Id, null);
            }
            catch (Exception ex)
            {
                log.Error("ошибка GetLocationById", ex);
                throw;
            }
        }

        public Location GetLocationByExternalId(string ExternalId)
        {
            try
            {
                var rep = new LocationRepository();
                return rep.GetLocationByExternalId(null, ExternalId);
            }
            catch (Exception ex)
            {
                log.Error("ошибка GetLocationByExternalId", ex);
                throw;
            }
        }

        public Location GetLocationByKladrCode(string KladrCode)
        {
            try
            {
                var rep = new LocationRepository();
                return rep.GetLocationByKladrCode(KladrCode, null);
            }
            catch (Exception ex)
            {
                log.Error("ошибка GetLocationByKladrCode", ex);
                throw;
            }
        }

        public Location GetLocationByCoordinates(string GeoSystem, int? LocationType, int? Radius, decimal Lat, decimal Lng)
        {
            try
            {
                var rep = new LocationRepository();
                return rep.GetLocationByCoordinates(null, GeoSystem, LocationType, Radius, Lat, Lng);
            }
            catch (Exception ex)
            {
                log.Error("ошибка GetLocationByCoordinates", ex);
                throw;
            }
        }

        public Version GetServiceVersion()
        {
            try
            {
                log.Info("Вызван GetServiceVersion");
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
            catch (Exception ex)
            {
                log.Error("ошибка GetServiceVersion", ex);
                throw;
            }
        }
    }
}
