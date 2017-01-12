using System.Collections.Generic;

namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    public class GetCategoryInfoParameters : IClientContextParameters
    {
        public int CategoryId { get; set; }

        public Dictionary<string, string> ClientContext { get; set; }
    }
}