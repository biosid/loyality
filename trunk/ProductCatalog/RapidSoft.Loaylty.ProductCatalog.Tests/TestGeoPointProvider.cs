namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using RapidSoft.Kladr.Model;
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.Services;

    public class TestGeoPointProvider : GeoPointProvider
    {
        public override KladrAddress GetAddressByKladrCode(string kladrCode)
        {
            return new KladrAddress
            {
                AddressLevel = AddressLevel.City,
                Region =
                    new AddressElement
                    {
                        Level = AddressLevel.Region,
                        Code = "7700000000000",
                        Name = "Москва",
                        Prefix = "г."
                    },
                City =
                    new AddressElement
                    {
                        Level = AddressLevel.City,
                        Code = "7700000000000",
                        Name = "Москва",
                        Prefix = "г."
                    },
                FullText = "г. Москва"
            };
        }
    }
}