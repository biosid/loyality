namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Collections.Generic;

    public class CategoryProductParamsParameters : IClientContextParameters
    {
        public int CategoryId
        {
            get;
            set;
        }

        public CategoryProductParamsParameter[] ProductParams
        {
            get;
            set;
        }

        public Dictionary<string, string> ClientContext
        {
            get;
            set;
        }
    }
}
