namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    using System;

    using Extensions;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    /// <summary>
    /// Проекция товара для сортировки силами Entity Framework
    /// </summary>
    public class ProductSortProjection
    {
        /// <summary>
        /// Внутренний идентификатор товара
        /// </summary>
        public string ProductId { get; set; }

        public int PartnerId { get; set; }

        /// <summary>
        /// Название подарка
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Артикул
        /// </summary>
        public string PartnerProductId { get; set; }

        public int ModerationStatusCode { get; set; }

        public ProductModerationStatuses ModerationStatus
        {
            get { return (ProductModerationStatuses)ModerationStatusCode; }
            set { ModerationStatusCode = (int)value; }
        }

        public int StatusCode { get; set; }

        public ProductStatuses Status
        {
            get { return (ProductStatuses)StatusCode; }
            set { StatusCode = (int)value; }
        }

        public bool IsRecommended { get; set; }

        public DateTime InsertedDate { get; set; }

        public int CategoryId { get; set; }

        public string UpdatedUserId { get; set; }

        public string ParamsXml { get; set; }

        public ProductParam[] Param
        {
            get
            {
                return string.IsNullOrEmpty(ParamsXml) ? null : XmlSerializer.Deserialize<ProductParam[]>(ParamsXml);
            }

            set
            {
                ParamsXml = XmlSerializer.SerializeToElement(value).ToString();
            }
        }

        public string Vendor { get; set; }

        public int? Weight { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("ProductId: {0}, Name: {1}", this.ProductId, this.Name);
        }
    }
}