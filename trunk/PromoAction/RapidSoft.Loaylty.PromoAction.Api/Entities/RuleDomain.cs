namespace RapidSoft.Loaylty.PromoAction.Api.Entities 
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;

    /// <summary>
    /// Домен правил.
    /// </summary>
    [DataContract]
    public class RuleDomain : BaseEntity, ITraceableEntity<long>, IHistoryTraceable<RuleDomainHistory>, IResettable<RuleDomain>
    {
        private EntitiesMetadata deserializedMetadata;

        private string metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleDomain"/> class.
        /// </summary>
        public RuleDomain()
        {
            this.DefaultBaseAdditionFactor = RuleTypes.BaseAddition.GetDefaultFactor();
            this.DefaultBaseMultiplicationFactor = RuleTypes.BaseMultiplication.GetDefaultFactor();
        }

        /// <summary>
        /// Уникальный идентифкикатор
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// Имя домена правил. Уникально в рамках данного реестра.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Краткое описание домен правил.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Метаданные домен правил.
        /// </summary>
        [DataMember]
        public string Metadata
        {
            get
            {
                return this.metadata;
            }

            set
            {
                this.metadata = value;
            }
        }

        /// <summary>
        /// Тип способа вычисления лимитного значения, если в ходе вычисления обнаружено 
        /// что результат меньше граничного значения определяемого <see cref="StopLimitType"/> и <see cref="StopLimitFactor"/>.
        /// </summary>
        [DataMember]
        public LimitTypes LimitType { get; set; }

        /// <summary>
        /// Коэффициент лимитного значения, число используемое для вычисление результата, если в ходе вычисления обнаружено 
        /// что результат меньше граничного значения определяемого <see cref="StopLimitType"/> и <see cref="StopLimitFactor"/>.
        /// </summary>
        [DataMember]
        public decimal LimitFactor { get; set; }

        /// <summary>
        /// Тип способа вычисления граничного значения, то есть значения ниже которого результат является недопустимым.
        /// </summary>
        [DataMember]
        public LimitTypes StopLimitType { get; set; }

        /// <summary>
        /// Коэффициент расчета граничного значения. Например:
        /// пусть коэффициент расчета граничного значения = 15,
        /// если <see cref="StopLimitType"/> = <see cref="LimitTypes.Fixed"/>, 
        /// любой результат, полученный в ходе вычисленый, меньше 15 является недопустымым и 
        /// результат вычисляется с помощью <see cref="LimitType"/> и <see cref="LimitFactor"/>.
        /// если <see cref="StopLimitType"/> = <see cref="LimitTypes.Percent"/>, 
        /// любой результат, полученный в ходе вычисленый, меньше 15% от входного значения является недопустымым и 
        /// результат вычисляется с помощью <see cref="LimitType"/> и <see cref="LimitFactor"/>.
        /// </summary>
        [DataMember]
        public decimal StopLimitFactor { get; set; }

        /// <summary>
        /// Базовый аддитивный коэфициент по умолчанию
        /// </summary>
        [DataMember]
        public decimal DefaultBaseAdditionFactor { get; set; }

        /// <summary>
        /// Базовый мульпликативный коэфициент по умолчанию
        /// </summary>
        [DataMember]
        public decimal DefaultBaseMultiplicationFactor { get; set; }

        /// <summary>
        /// Коллекция правил в данном домене.
        /// </summary>
        [IgnoreDataMember]
        public virtual IList<Rule> Rules { get; set; }
        
        public EntitiesMetadata GetDeserializedMetadata()
        {
            var retVal = this.deserializedMetadata ?? (this.deserializedMetadata = this.Metadata.Deserialize<EntitiesMetadata>());
            return retVal;
        }

        public void SetDeserializedMetadata(EntitiesMetadata entitiesMetadata)
        {
            entitiesMetadata.ThrowIfNull("entitiesMetadata");

            this.deserializedMetadata = entitiesMetadata;
            this.metadata = entitiesMetadata.Serialize();
        }

        /// <summary>
        /// Метод создает из сущности новую сущность для сохранения в истории.
        /// </summary>
        /// <param name="historyEvent">
        /// Тип события указывающий причину создания сущности версионинга.
        /// </param>
        /// <returns>
        /// Сущность версионинга.
        /// </returns>
        public RuleDomainHistory ToHistoryEntity(HistoryEvent historyEvent, string userId)
        {
            return new RuleDomainHistory(this, historyEvent);
        }

        /// <summary>
        /// Копирование свойств из <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">
        /// Сущность из которой выполняется копирование.
        /// </param>
        public void ResetFrom(RuleDomain entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.Description = entity.Description;
            this.Metadata = this.Metadata;
            this.UpdatedUserId = entity.UpdatedUserId;
            this.InsertedDate = entity.InsertedDate;
            this.LimitType = entity.LimitType;
            this.LimitFactor = entity.LimitFactor;
            this.StopLimitType = entity.StopLimitType;
            this.StopLimitFactor = entity.StopLimitFactor;
            this.DefaultBaseAdditionFactor = entity.DefaultBaseAdditionFactor;
            this.DefaultBaseMultiplicationFactor = entity.DefaultBaseMultiplicationFactor;
        }

        public object ToHistoryEntity(HistoryEvent historyEvent)
        {
            return this.ToHistoryEntity(historyEvent, null);
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Id: {0}, Name: {1}, Description: {2}, Metadata: {3}, LimitType: {4}, LimitFactor: {5}, DefaultBaseAdditionFactor: {6}, DefaultBaseMultiplicationFactor: {7}, Rules count: {8}",
                    this.Id,
                    this.Name,
                    this.Description,
                    this.metadata,
                    this.LimitType,
                    this.LimitFactor,
                    this.DefaultBaseAdditionFactor,
                    this.DefaultBaseMultiplicationFactor,
                    this.Rules == null ? "null" : this.Rules.Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}