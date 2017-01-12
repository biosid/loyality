namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ProductParameters : CatalogAdminParameters
    {
        /// <summary>
        /// Идентификатор партнера
        /// </summary>
        [DataMember]
        public int PartnerId { get; set; }

        /// <summary>
        ///  Идентификатор категории
        /// </summary>
        [DataMember]
        public int CategoryId { get; set; }

        /// <summary>
        ///  Наименование
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///  Цена в рублях 
        /// </summary>
        [DataMember]
        public decimal PriceRur { get; set; }

        /// <summary>
        ///  Описание товара 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///  Производитель 
        /// </summary>
        [DataMember]
        public string Vendor { get; set; }

        /// <summary>
        ///  Вес (в граммах) 
        /// </summary>
        [DataMember]
        public int Weight { get; set; }

        /// <summary>
        ///  Ссылки на изображение
        /// </summary>
        [DataMember]
        public string[] Pictures { get; set; }

        /// <summary>
        ///  Дополнительные параметры
        /// </summary>
        [DataMember]
        public ProductParameterValue[] Parameters { get; set; }
    }
}
