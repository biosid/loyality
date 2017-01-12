namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Физические товары. 
    /// Легковесная сущность товара.
    /// </summary>
    [DataContract]
    public class Product
    {
        /// <summary>
        /// Внутренний идентификатор товара
        /// </summary>
        [DataMember]
        public string ProductId { get; set; }

        /// <summary>
        /// Артикул товара
        /// </summary>
        [DataMember]
        public string PartnerProductId
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор партнера-поставщика
        /// </summary>
        [DataMember]
        public int PartnerId
        {
            get;
            set;
        }

        /// <summary>
        /// Название подарка
        /// </summary>
        [DataMember]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор категории
        /// </summary>
        [DataMember]
        public int CategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// Название категории товара
        /// </summary>
        [DataMember]
        public string CategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// Полный путь к категории
        /// </summary>
        [DataMember]
        public string CategoryNamePath
        {
            get;
            set;
        }

        /// <summary>
        /// Статус подарка
        /// </summary>
        [DataMember]
        public ProductStatuses Status
        {
            get;
            set;
        }

        /// <summary>
        /// Статус модерации подарка
        /// </summary>
        [DataMember]
        public ProductModerationStatuses ModerationStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Причина последнего изменения статуса товара
        /// </summary>
        [DataMember]
        public string StatusChangedCause
        {
            get;
            set;
        }

        /// <summary>
        /// Дата и время последнего изменения статуса товара
        /// </summary>
        [DataMember]
        public DateTime StatusChangedDate
        {
            get;
            set;
        }

        /// <summary>
        /// Пользователь, который внес последнее изменение статуса товара
        /// </summary>
        [DataMember]
        public string StatusChangedUser
        {
            get;
            set;
        }

        /// <summary>
        /// Дата и время последнего изменения записи
        /// </summary>
        [DataMember]
        public DateTime LastChangedDate
        {
            get;
            set;
        }

        /// <summary>
        /// Пользователь, который внес последнее изменение
        /// </summary>
        [DataMember]
        public string LastChangedUser
        {
            get;
            set;
        }

        [DataMember]
        public string Description
        {
            get;
            set;
        }

        [DataMember]
        public string PartnerCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// Признак доступности товара у партнера-поставщика. Принимает одно из следующих значений:
        //0 — товара нет в наличии;
        //1 — товарное предложение в наличии.
        /// </summary>  
        [DataMember]
        public bool Available
        {
            get;
            set;
        }

        public string CurrencyId
        {
            get;
            set;
        }

        /// <summary>
        /// Тип товара по классификации YML
        /// </summary>
        [DataMember]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// URL страницы товара. Максимальная длина URL — 512 символов.
        /// </summary>
        [DataMember]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Список ссылок на различные изображения товара.
        /// </summary>
        [DataMember]
        public string[] Pictures
        {
            get;
            set;
        }

        /// <summary>
        /// Название производителя. 
        /// </summary>
        [DataMember]
        public string Vendor
        {
            get;
            set;
        }

        /// <summary>
        /// Код товара (указывается код от производителя).
        /// </summary>
        [DataMember]
        public string VendorCode
        {
            get;
            set;
        }

        //[DataMember]
        public int Bid
        {
            get;
            set;
        }

        //[DataMember]
        public int CBid
        {
            get;
            set;
        }

        [DataMember]
        public string TypePrefix
        {
            get;
            set;
        }

        [DataMember]
        public string Model
        {
            get;
            set;
        }

        [DataMember]
        public bool Store
        {
            get;
            set;
        }

        [DataMember]
        public bool Pickup
        {
            get;
            set;
        }

        [DataMember]
        public bool Delivery
        {
            get;
            set;
        }

        public decimal LocalDeliveryCost
        {
            get;
            set;
        }

        [DataMember]
        public string SalesNotes
        {
            get;
            set;
        }

        [DataMember]
        public bool ManufacturerWarranty
        {
            get;
            set;
        }

        [DataMember]
        public string CountryOfOrigin
        {
            get;
            set;
        }

        [DataMember]
        public bool Downloadable
        {
            get;
            set;
        }

        [DataMember]
        public string Adult
        {
            get;
            set;
        }

        [DataMember]
        public string[] Barcode
        {
            get;
            set;
        }

        [DataMember]
        public ProductParam[] Param
        {
            get;
            set;
        }

        [DataMember]
        public string Author
        {
            get;
            set;
        }

        [DataMember]
        public string Publisher
        {
            get;
            set;
        }

        [DataMember]
        public string Series
        {
            get;
            set;
        }

        [DataMember]
        public int Year
        {
            get;
            set;
        }

        [DataMember]
        public string ISBN
        {
            get;
            set;
        }

        [DataMember]
        public int Volume
        {
            get;
            set;
        }

        [DataMember]
        public int Part
        {
            get;
            set;
        }

        [DataMember]
        public string Language
        {
            get;
            set;
        }

        [DataMember]
        public string Binding
        {
            get;
            set;
        }

        [DataMember]
        public int PageExtent
        {
            get;
            set;
        }

        [DataMember]
        public string TableOfContents
        {
            get;
            set;
        }

        [DataMember]
        public string PerformedBy
        {
            get;
            set;
        }

        [DataMember]
        public string PerformanceType
        {
            get;
            set;
        }

        [DataMember]
        public string Format
        {
            get;
            set;
        }

        [DataMember]
        public string Storage
        {
            get;
            set;
        }

        [DataMember]
        public string RecordingLength
        {
            get;
            set;
        }

        [DataMember]
        public string Artist
        {
            get;
            set;
        }

        [DataMember]
        public string Media
        {
            get;
            set;
        }

        [DataMember]
        public string Starring
        {
            get;
            set;
        }

        [DataMember]
        public string Director
        {
            get;
            set;
        }

        [DataMember]
        public string OriginalName
        {
            get;
            set;
        }

        [DataMember]
        public string Country
        {
            get;
            set;
        }

        [DataMember]
        public string WorldRegion
        {
            get;
            set;
        }

        [DataMember]
        public string Region
        {
            get;
            set;
        }

        [DataMember]
        public int Days
        {
            get;
            set;
        }

        [DataMember]
        public string DataTour
        {
            get;
            set;
        }

        [DataMember]
        public string HotelStars
        {
            get;
            set;
        }

        [DataMember]
        public string Room
        {
            get;
            set;
        }

        [DataMember]
        public string Meal
        {
            get;
            set;
        }

        [DataMember]
        public string Included
        {
            get;
            set;
        }

        [DataMember]
        public string Transport
        {
            get;
            set;
        }

        [DataMember]
        public string Place
        {
            get;
            set;
        }

        [DataMember]
        public string HallPlan
        {
            get;
            set;
        }

        [DataMember]
        public DateTime? Date
        {
            get;
            set;
        }

        [DataMember]
        public bool IsPremiere
        {
            get;
            set;
        }

        [DataMember]
        public bool IsKids
        {
            get;
            set;
        }

        [DataMember]
        public int? Weight
        {
            get;
            set;
        }

        [DataMember]
        public DateTime InsertedDate
        {
            get;
            set;
        }

        [DataMember]
        public DateTime? UpdatedDate
        {
            get;
            set;
        }

        #region Product prices

        /// <summary>
        /// Цена, по которой данный товар можно приобрести. Цена указывается в рублях. 
        /// Цена полученная от партнёра
        /// </summary>
        [DataMember]
        public decimal PriceRUR
        {
            get;
            set;
        }

        /// <summary>
        /// Цена в рублях, по котороый определяется скидка. 
        /// </summary>
        [DataMember]
        public decimal? BasePriceRUR
        {
            get;
            set;
        }


        /// <summary>
        /// Базовая цена в баллах, расчитана на основе цены товара в рублях 
        /// с применением базовых механик.
        /// </summary>
        [DataMember]
        public decimal PriceBase
        {
            get;
            set;
        }

        /// <summary>
        /// Цена товара в баллах, расчитана на основе цены товара в рублях 
        /// с применением базовых и не базовых механик.
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        #endregion

        /// <summary>
        /// Идентификатор партнерского элемента тарифа доставки
        /// </summary>
        [DataMember]
        public string ExternalLocationId { get; set; }

        /// <summary>
        /// Признак действия акции на товар
        /// </summary>
        [DataMember]
        public bool IsActionPrice
        {
            get;
            set;
        }

        [DataMember]
        public string UpdatedUserId
        {
            get;
            set;
        }

        [DataMember]
        public string[] TargetAudiencesIds
        {
            get;
            set;
        }

        /// <summary>
        /// Является ли товар рекоммендованным
        /// </summary>
        [DataMember]
        public bool IsRecommended
        {
            get;
            set;
        }

        /// <summary>
        /// Дата фиксации базовой цены при автоматической установке признака скидки
        /// </summary>
        [DataMember]
        public DateTime? BasePriceRurDate
        {
            get;
            set;
        }

        public int ProductRate
        {
            get;
            set;
        }
        
        public PopularProductTypes PopularType
        {
            get;
            set;
        }

        /// <summary>
        /// Признак, что товар доставляется по E-mail
        /// </summary>
        [DataMember]
        public bool IsDeliveredByEmail { get; set; }
    }
}