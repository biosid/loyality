namespace RapidSoft.Loaylty.ProductCatalog.Tests.Services
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;

    public class GetCatalogParameters : IClientContextParameters
    {
        public Dictionary<string, string> ClientContext { get; set; }
    }
}