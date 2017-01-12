namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class ImportProductsFromYmlHttpParameters
    {
        public int PartnerId { get; set; }

        public string FullFilePath { get; set; }

        public string UserId { get; set; }

        public WeightProcessTypes? WeightProcessType { get; set; }
    }
}

