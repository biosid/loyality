namespace RapidSoft.Loaylty.ProductCatalog.Interfaces
{
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;

    public interface ICountrySeacher
    {
        GetDeliveryCountriesResult GetDeliveryCountries(int partnerId);
    }
}