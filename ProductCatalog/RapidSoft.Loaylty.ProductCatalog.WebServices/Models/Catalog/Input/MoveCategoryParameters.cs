namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class MoveCategoryParameters : CatalogAdminParameters
    {
        /// <summary>
        /// Идентификатор перемещаемой категории.
        /// </summary>
        [DataMember]
        public int CategoryId { get; set; }

        /// <summary>
        /// Идентификатор категории относительно которой осуществляется перенос
        /// </summary>
        [DataMember]
        public int? ReferenceCategoryId { get; set; }

        /// <summary>
        /// Тип перемещения
        /// </summary>
        [DataMember]
        public CategoryPositionTypes PositionType { get; set; }
    }
}
