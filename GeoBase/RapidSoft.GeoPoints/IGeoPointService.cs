namespace RapidSoft.GeoPoints
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;

    using RapidSoft.GeoPoints.Entities;
    using RapidSoft.GeoPoints.OutputResults;
    using RapidSoft.Loaylty.Monitoring;

    [ServiceContract]
    public interface IGeoPointService : ISupportService
    {
        /// <summary>
        /// Возвращает максимально детальный адрес без улицы и дома по коду КЛАДР.
        /// </summary>
        /// <param name="KladrCode">
        /// Коду КЛАДР.
        /// </param>
        /// <returns>
        /// The <see cref="KladrAddressResult"/>.
        /// </returns>
        [OperationContract]
        KladrAddressResult GetAddressByKladrCode(string KladrCode);

        [OperationContract]
        CheckKladrCodeResult GetExistKladrCodes(string[] KladrCodes);

        /// <summary>
        /// Возвращает список стран по их буквенным кодам страны альфа-2 по ОКСМ (двухбуквеннный код).
        /// </summary>
        /// <param name="codes">
        /// Набор двухбуквенных строк.
        /// </param>
        /// <returns>
        /// Найденные страны. Если по двухбуквенному коду страна не найдена, то код игнорируется.
        /// </returns>
        [OperationContract]
        CountryListResult GetCountriesNamesByCode(List<string> codes);

        [OperationContract]
        List<Location> GetCountries(string NameSearchPattern, int? Skip, int? Top);

        [OperationContract]
        List<Location> GetLocationsByParent(Guid ParentId, int? LocationType, string NameSearchPattern, int? Skip, int? Top);

        [OperationContract]
        List<Location> GetLocationsByIP(string IP, int LocationType, int? Skip, int? Top);

        [OperationContract]
        Location GetLocationByIP(string IP);

        [OperationContract]
        Location GetLocationById(Guid Id);

        [OperationContract]
        Location GetLocationByExternalId(string ExternalId);

        [OperationContract]
        Location GetLocationByKladrCode(string KladrCode);

        [OperationContract]
        GetLocationsResult GetLocationsByKladrCode(
            string ParentKladrCode, int[] LocationType, string[] Toponyms, string NameSearchPattern, bool? RegionIsCityOnly, int? Skip, int? Top);

        [OperationContract]
        Location GetLocationByCoordinates(string GeoSystem, int? LocationType, int? Radius, decimal Lat, decimal Lng);

        [OperationContract]
        Version GetServiceVersion();
    }
}
