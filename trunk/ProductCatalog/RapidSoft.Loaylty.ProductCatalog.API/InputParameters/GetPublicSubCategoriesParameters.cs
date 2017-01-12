namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class GetPublicSubCategoriesParameters : IClientContextParameters, IPagingParameters
    {
        private ProductCategoryStatuses? status;

        public GetPublicSubCategoriesParameters()
        {
            this.Initialize();
        }

        /// <summary>
        /// Фильтр по идентификатору родительской категории
        /// </summary>
        [DataMember]
        public int? ParentId { get; set; }

        /// <summary>
        /// Возвращать ли родительскую категорию 
        /// </summary>
        [DataMember]
        public bool IncludeParent { get; set; }

        /// <summary>
        /// Фильтр по уровню вложенности категорий
        /// </summary>
        [DataMember]
        public int? NestingLevel { get; set; }

        /// <summary>
        /// Контекст клиента
        /// </summary>
        [DataMember]
        public Dictionary<string, string> ClientContext { get; set; }

        /// <summary>
        /// Количество пропускаемых категорий
        /// </summary>
        [DataMember]
        public int? CountToSkip { get; set; }

        /// <summary>
        /// Количество отбираемых категорий на страницу
        /// </summary>
        [DataMember]
        public int? CountToTake { get; set; }

        /// <summary>
        /// Считать ли общее количество найденных категорий
        /// </summary>
        [DataMember]
        public bool? CalcTotalCount { get; set; }

        /// <summary>
        /// Фильтр по типу категории 
        /// </summary>
        [DataMember]
        public ProductCategoryTypes? Type { get; set; }

        [IgnoreDataMember]
        public int[] CategoryIds { get; set; }

        [IgnoreDataMember]
        public ProductCategoryStatuses? Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
            }
        }

        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }

        private void Initialize()
        {
            // NOTE: По умолчанию поиск публичных категорий включает поиск только активных категорий. 
            this.Status = ProductCategoryStatuses.Active;
        }
    }
}