namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ClientItemProduct
    {
        /// <summary>
        /// Внутренний идентификатор товара
        /// </summary>
        [DataMember]
        public string ProductId { get; set; }

        /// <summary>
        /// Идентификатор партнера-поставщика
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Идентификатор категории
        /// </summary>
        [DataMember]
        public int CategoryId { get; set; }

        /// <summary>
        /// Название категории товара
        /// </summary>
        [DataMember]
        public string CategoryName { get; set; }

        /// <summary>
        /// Название товара
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Список ссылок на различные изображения товара
        /// </summary>
        [DataMember]
        public string[] Pictures { get; set; }

        /// <summary>
        /// Название производителя
        /// </summary>
        [DataMember]
        public string Vendor { get; set; }

        /// <summary>
        /// Признак действия акции на товар
        /// </summary>
        [DataMember]
        public bool IsActionPrice { get; set; }

        /// <summary>
        /// Дата добавления товара в каталог
        /// </summary>
        [DataMember]
        public DateTime InsertedDate { get; set; }
    }
}
