namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Collections.Generic;

    public class GetFilterMetaDataParameters : IClientContextParameters
    {
        public ProductAttributes Attribute { get; set; }

        public int? CategoryId { get; set; }

        public Dictionary<string, string> ClientContext { get; set; }
    }
}