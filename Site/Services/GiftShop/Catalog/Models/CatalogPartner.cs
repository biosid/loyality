using System.Collections.Generic;

namespace Vtb24.Site.Services.GiftShop.Catalog.Models
{
    public class CatalogPartner
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> RawSettings { get; set; }

        public CatalogPartnerSettings Settings { get; set; }
    }
}
