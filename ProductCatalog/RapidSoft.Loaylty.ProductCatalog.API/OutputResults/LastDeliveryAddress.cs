namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class LastDeliveryAddress
    {
        public VariantsLocation DeliveryVariantsLocation { get; set; }

        public DeliveryAddress DeliveryAddress { get; set; }
    }
}
