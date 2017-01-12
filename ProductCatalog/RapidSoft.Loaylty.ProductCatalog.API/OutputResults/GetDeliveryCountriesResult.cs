namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class GetDeliveryCountriesResult : ResultBase
    {
        public IList<Country> DeliveryCountries { get; set; }

        public static GetDeliveryCountriesResult BuildSuccess(IList<Country> countries = null)
        {
            return new GetDeliveryCountriesResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           ResultDescription = null,
                           DeliveryCountries = countries
                       };
        }

        public static GetDeliveryCountriesResult BuildError(string message)
        {
            return new GetDeliveryCountriesResult
                       {
                           ResultCode = ResultCodes.UNKNOWN_ERROR,
                           ResultDescription = message,
                           DeliveryCountries = null
                       };
        }
    }
}