namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CatalogAdminParameters
    {
        /// <summary>
        /// Идентификатор пользователя в системе безопасности
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
    }
}
