namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Input;

    [DataContract]
    public class GetDeliveryLocationsParameters : CatalogAdminParameters
    {
        [DataMember]
        public int PartnerId { get; set; }

        /// <summary>
        /// Указывает на необходимость возвращать только те точки доставки, которые:
        ///     true - имеют тариф доставки
        ///     false - НЕ имеют тариф доставки
        ///     null - и имеют и НЕ имеют тариф доставки
        /// </summary>
        [DataMember]
        public bool? HasRates { get; set; }

        [DataMember]
        public string SearchTerm { get; set; }

        [DataMember]
        public DeliveryLocationStatuses[] StatusFilters { get; set; }

        [DataMember]
        public PagingParameters Paging { get; set; }
    }
}
