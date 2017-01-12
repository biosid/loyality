using RapidSoft.Loaylty.ProductCatalog.API.Entities;

namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    public class PopularProduct
    {
        public int ProductRate
        {
            get;
            set;
        }

        public Product Product
        {
            get;
            set;
        }

        public PopularProductTypes PopularType
        {
            get;
            set;
        }

        public string ProductId
        {
            get;
            set;
        }
    }
}