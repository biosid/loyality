namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using API.Entities;

    public class CalculatedPrice
    {
        public int PartnerId
        {
            get;
            set;
        }

        public Price Price
        {
            get;
            set;
        }
    }
}