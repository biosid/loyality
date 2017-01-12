namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class SetPartnerSettingsParameters : CatalogAdminParameters
    {
        [DataMember]
        public int PartnerId { get; set; }

        [DataMember]
        public Dictionary<string, string> Settings { get; set; }
    }
}
