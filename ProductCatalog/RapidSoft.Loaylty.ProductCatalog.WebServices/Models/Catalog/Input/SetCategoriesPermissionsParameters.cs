namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SetCategoriesPermissionsParameters : CatalogAdminParameters
    {
        /// <summary>
        /// Идентификатор партнера.
        /// </summary>
        [DataMember]
        public int PartnerId { get; set; }

        /// <summary>
        /// Набор идентификаторов категорий для которых необходимо добавить разрешение.
        /// </summary>
        [DataMember]
        public int[] AddCategoriesId { get; set; }

        /// <summary>
        /// Набор идентификаторов категорий для которых необходимо удалить разрешение.
        /// </summary>
        [DataMember]
        public int[] RemoveCategoriesId { get; set; }
    }
}
