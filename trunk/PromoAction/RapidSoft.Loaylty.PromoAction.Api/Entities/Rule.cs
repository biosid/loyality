namespace RapidSoft.Loaylty.PromoAction.Api.Entities 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;

    /// <summary>
    /// Правило преобразования числа
    /// </summary>
    [DataContract]
    public class Rule : BaseEntity, ITraceableEntity<long>, IHistoryTraceable<RuleHistory>, IValidatableObject, IResettable<Rule>
    {
        /// <summary>
        /// Условие (предикат) в формате xml.
        /// </summary>
        private string predicate;

        /// <summary>
        /// Десериализованный предикат (условие).
        /// </summary>
        private filter deserializedPredicate;

        /// <summary>
        /// Домен правил в которое входит данное правило.
        /// </summary>
        private RuleDomain ruleDomain;

        /// <summary>
        /// Идентификатор домена правил в которое входит данное правило.
        /// </summary>
        private long ruleDomainId;

        /// <summary>
        /// Признак не исключаемого правила: <c>false</c> - правило исключаемое, <c>true</c> - правило не исключаемое.
        /// </summary>
        private bool isNotExcludedBy;

        /// <summary>
        /// Десериализованный массив условных коэффициентов.
        /// </summary>
        private ConditionalFactor[] deserializedConditionalFactors;

        private ApproveStatus approved;

        /// <summary>
        /// Уникальный идентифкикатор.
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор домена правил в которое входит данное правило.
        /// </summary>
        [DataMember]
        public long RuleDomainId
        {
            get
            {
                return this.ruleDomainId;
            }

            set
            {
                this.ruleDomainId = value;
                if (this.ruleDomain != null && this.ruleDomainId != this.ruleDomain.Id)
                {
                    this.ruleDomain = null;
                }
            }
        }

        /// <summary>
        /// Название правила/промоакции
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Домен правил в которое входит данное правило.
        /// </summary>
        [IgnoreDataMember]
        public RuleDomain RuleDomain
        {
            get
            {
                return this.ruleDomain;
            }

            set
            {
                this.ruleDomain = value;
                this.ruleDomainId = this.ruleDomain != null ? this.ruleDomain.Id : default(long);
            }
        }

        /// <summary>
        /// Тип правила <see cref="RuleTypes"/>.
        /// </summary>
        [DataMember]
        public RuleTypes Type { get; set; }

        /// <summary>
        /// Время и дата начала действия правила.
        /// </summary>
        [DataMember]
        public DateTime? DateTimeFrom { get; set; }

        /// <summary>
        /// Дата и время после, которой правило прекращает своё действие.
        /// </summary>
        [DataMember]
        public DateTime? DateTimeTo { get; set; }

        /// <summary>
        /// Статус правил
        /// </summary>
        [DataMember]
        public RuleStatuses Status { get; set; }

        /// <summary>
        /// Признак исключающего правила: <c>false</c> - не исключающее, <c>true</c> - исключающее все правила, кроме базовых и помеченных признаком «не исключаемое правило».
        /// У исключающих правил уникальный приоритет.
        /// </summary>
        [DataMember]
        public bool IsExclusive { get; set; }

        /// <summary>
        /// Признак не исключаемого правила: <c>false</c> - правило исключаемое, <c>true</c> - правило не исключаемое.
        /// </summary>
        [DataMember]
        public bool IsNotExcludedBy
        {
            get
            {
                if (this.Type == RuleTypes.BaseMultiplication || this.Type == RuleTypes.BaseAddition)
                {
                    // NOTE: Если правило базовое, то оно всегда не исключаемо!!!
                    return true;
                }

                return this.isNotExcludedBy;
            }

            set
            {
                this.isNotExcludedBy = value;
            }
        }

        /// <summary>
        /// Приоритет правила, который действует только внутри правил данного типа в данном домене правил. 
        /// Значение должно быть уникальным в рамках всех правил одного домена правил, имеющих тип «Базовое» и 
        /// у исключающих правил должен быть уникальный приоритет.
        /// </summary>
        [DataMember]
        public int Priority { get; set; }

        /// <summary>
        /// Условие (предикат) в формате xml.
        /// </summary>
        [DataMember]
        public string Predicate
        {
            get
            {
                return this.predicate;
            }

            set
            {
                if (value != null && string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Пустое значение свойства не допустимо");
                }

                this.deserializedPredicate = null;
                this.predicate = value;
            }
        }

        /// <summary>
        /// Числовое значение.
        /// Если тип правила «Базовое», то исходное значение для расчета должно быть умножено на значение этого поля.
        /// Если тип правила «Мультипликативное», то исходное значение для расчета должно быть умножено на значение этого поля.
        /// Если тип правила «Аддитивное», то исходное значение для расчета должно быть просуммировано со значением этого поля.
        /// </summary>
        [DataMember]
        public decimal Factor { get; set; }

        /// <summary>
        /// Условные коэффициенты в формате xml.
        /// </summary>
        [DataMember]
        public string ConditionalFactors { get; set; }

        ///// <summary>
        ///// Статус промоакции получений из банка, см. банк-коннектор.
        ///// </summary>
        //[DataMember]
        //public string ExternalStatusId { get; set; }

        /// <summary>
        /// Признак подтверждения правила
        /// </summary>
        [DataMember]
        public ApproveStatus Approved
        {
            get
            {
                // NOTE: Базовое правило всегда подтверждено
                if (this.IsBase())
                {
                    return ApproveStatus.Approved;
                }

                return this.approved;
            }

            set
            {
                this.approved = value;
            }
        }

        /// <summary>
        /// Признак подтверждения правила
        /// </summary>
        [DataMember]
        public string ApproveDescription { get; set; }

        public static Rule BuildDefaultRule(decimal factor, RuleTypes type, RuleDomain domain)
        {
            return new Rule
                       {
                           Id = -1,
                           Factor = factor,
                           Type = type,
                           Priority = int.MinValue,
                           Predicate = null, // NOTE: Всегда true!

                           ConditionalFactors = null,
                           DateTimeFrom = null,
                           DateTimeTo = null,
                           IsExclusive = false,
                           IsNotExcludedBy = false,
                           Name = "Default " + type + " rule",
                           Status = RuleStatuses.Active,
                           RuleDomain = domain,
                           InsertedDate = DateTime.MinValue,
                           UpdatedDate = null,
                           UpdatedUserId = "system generated"
                       };
        }

        /// <summary>
        /// Возвращает десериализованный предикат.
        /// </summary>
        /// <returns>
        /// Экземпляр предиката (<see cref="filter"/>).
        /// </returns>
        public filter GetDeserializedPredicate()
        {
            var retVal = this.deserializedPredicate ?? (this.deserializedPredicate = this.Predicate.Deserialize<filter>());
            return retVal;
        }

        /// <summary>
        /// Устанавливает предикат.
        /// </summary>
        /// <param name="filter">
        /// Экземпляр предиката (<see cref="filter"/>).
        /// </param>
        public void SetDeserializedPredicate(filter filter)
        {
            filter.ThrowIfNull("filter");

            this.deserializedPredicate = filter;
            this.predicate = filter.Serialize();
        }

        /// <summary>
        /// Возвращает десериализованные условные коэффициенты.
        /// </summary>
        /// <returns>
        /// Массив условных коэффициентов.
        /// </returns>
        public ConditionalFactor[] GetDeserializedConditionalFactors()
        {
            return this.deserializedConditionalFactors ?? (this.deserializedConditionalFactors = this.ConditionalFactors.Deserialize<ConditionalFactor[]>());
        }

        /// <summary>
        /// Устанавливает массив условных коэффициентов.
        /// </summary>
        /// <param name="conditionalFactors">
        /// Массив условных коэффициентов.
        /// </param>
        public void SetConditionalFactors(ConditionalFactor[] conditionalFactors)
        {
            this.deserializedConditionalFactors = conditionalFactors;
            this.ConditionalFactors = this.deserializedConditionalFactors.Serialize();
        }

        /// <summary>
        /// Возвращает <c>true</c> если правило является базовым.
        /// </summary>
        /// <returns>
        /// <c>true</c> если правило является базовым.
        /// </returns>
        public bool IsBase()
        {
            return this.Type.IsBase();
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
        public RuleHistory ToHistoryEntity(HistoryEvent historyEvent, string userId)
        {
            return new RuleHistory(this, historyEvent);
        }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        /// <param name="validationContext">The validation context.</param>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.IsBase() && !this.IsNotExcludedBy)
            {
                // NOTE: В реализации от 07.12.2012 не возможно срабатывание этой ошибки.
                yield return new ValidationResult("Если правило базовое, то оно должно быть не исключаемое.");
            }

            if (this.DateTimeFrom.HasValue && this.DateTimeTo.HasValue && this.DateTimeFrom > this.DateTimeTo)
            {
                yield return new ValidationResult("Дата начала действия правила не может быть больше даты окончания.");
            }

            if (this.GetDeserializedConditionalFactors() != null)
            {
                if (this.GetDeserializedConditionalFactors().Any(x => x.Predicate == null))
                {
                    yield return new ValidationResult("Предикат условного коэффициента не может null.");
                }

                if (this.GetDeserializedConditionalFactors().GroupBy(x => x.Priority).Any(x => x.Count() > 1))
                {
                    yield return new ValidationResult("Приоритеты условных коэффициента должны уникальны в рамках правила.");
                }
            }
        }

        /// <summary>
        /// Копирование свойств из <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">
        /// Сущность из которой выполняется копирование.
        /// </param>
        public void ResetFrom(Rule entity)
        {
            // NOTE: Approved и ApproveDescription не переустанавливаем, так как эти системные поля и правятся либо авто, либо спец средставми.
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.RuleDomainId = entity.RuleDomainId;
            this.Priority = entity.Priority;
            this.Predicate = entity.Predicate;
            this.ConditionalFactors = entity.ConditionalFactors;
            this.DateTimeFrom = entity.DateTimeFrom;
            this.DateTimeTo = entity.DateTimeTo;
            this.Status = entity.Status;
            this.Factor = entity.Factor;
            this.IsExclusive = entity.IsExclusive;
            this.IsNotExcludedBy = entity.IsNotExcludedBy;
            this.Type = entity.Type;
            this.UpdatedUserId = entity.UpdatedUserId;
            // this.ExternalStatusId = entity.ExternalStatusId;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            var countOfConditionalFactors = this.deserializedConditionalFactors == null
                                                ? "0"
                                                : this.deserializedConditionalFactors.Length.ToString(CultureInfo.InvariantCulture);

            return
                string.Format(
                    "Id: {0}, RuleDomainId: {1}, Type: {2}, Priority: {3}, Factor: {4}, IsExclusive: {5}, IsNotExcludedBy: {6}, Predicate: {7}, Count of ConditionalFactors: {8}, DateTimeFrom: {9}, DateTimeTo: {10}",
                    this.Id,
                    this.RuleDomainId,
                    this.Type,
                    this.Priority,
                    this.Factor,
                    this.IsExclusive,
                    this.IsNotExcludedBy,
                    this.Predicate,
                    countOfConditionalFactors,
                    this.DateTimeFrom.HasValue ? this.DateTimeFrom.ToString() : "null",
                    this.DateTimeTo.HasValue ? this.DateTimeFrom.ToString() : "null");
        }

        public object ToHistoryEntity(HistoryEvent historyEvent)
        {
            return this.ToHistoryEntity(historyEvent, null);
        }
    }
}