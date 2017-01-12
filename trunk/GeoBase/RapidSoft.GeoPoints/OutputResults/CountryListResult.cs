namespace RapidSoft.GeoPoints.OutputResults
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using RapidSoft.GeoPoints.Entities;

    [DataContract]
    public class CountryListResult : ResultBase
    {
        [DataMember]
        public IList<Country> Countries { get; set; }

        public static CountryListResult BuildSuccess(IList<Country> countries)
        {
            return new CountryListResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           ResultDescription = null,
                           Countries = countries
                       };
        }
    }
}