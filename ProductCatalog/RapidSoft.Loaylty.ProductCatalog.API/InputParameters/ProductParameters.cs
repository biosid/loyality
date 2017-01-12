namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    [KnownType(typeof(UpdateProductParameters))]
    public class ProductParameters
    {

        [DataMember]
        [StringLength(50)]
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор партнера
        /// </summary>
        [DataMember]
        public int PartnerId
        {
            get;
            set;
        }

        /// <summary>
        ///  Идентификатор категории
        /// </summary>
        [DataMember]
        public int CategoryId
        {
            get;
            set;
        }

        /// <summary>
        ///  Наименование
        /// </summary>
        [DataMember]
        [StringLength(256)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Идентфикатор валюты
        /// </summary>
        [IgnoreDataMember]
        public string CurrencyId
        {
            get
            {
                return "RUR";
            }
            set { }
        }

        /// <summary>
        ///  Цена в рублях 
        /// </summary>
        [DataMember]
        public decimal PriceRUR
        {
            get;
            set;
        }

        /// <summary>
        ///  Базовая цена в рублях 
        /// </summary>
        [DataMember]
        public decimal? BasePriceRUR
        {
            get;
            set;
        }

        /// <summary>
        ///  Описание товара 
        /// </summary>
        [DataMember]
        [StringLength(2000)]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        ///  Производитель 
        /// </summary>
        [DataMember]
        [StringLength(256)]
        public string Vendor
        {
            get;
            set;
        }

        /// <summary>
        ///  Вес (в граммах) 
        /// </summary>
        [DataMember]
        public int Weight
        {
            get;
            set;
        }

		/// <summary>
		/// Признак доставки по email
		/// </summary>
		[DataMember]
		public bool IsDeliveredByEmail 
		{ 
			get; 
			set; 
		}

        /// <summary>
        ///  Ссылки на изображение
        /// </summary>
        [DataMember]
        public string[] Pictures
        {
            get;
            set;
        }

        /// <summary>
        ///  Дополнительные параметры
        /// </summary>
        [DataMember]
        public ProductParam[] Param
        {
            get;
            set;
        }
    }
}
