namespace RapidSoft.Loaylty.PromoAction.Api.Entities.History
{
    using System;

    using RapidSoft.Extensions;

    /// <summary>
    /// Историческая сущность правила.
    /// </summary>
    public class RuleHistory : BaseHistoryEntity
    {
        /// <summary>
        /// Исходное правило.
        /// </summary>
        private readonly Rule rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleHistory"/> class.
        /// </summary>
        /// <param name="rule">
        /// The rule.
        /// </param>
        /// <param name="historyEvent">
        /// The history event.
        /// </param>
        public RuleHistory(Rule rule, HistoryEvent historyEvent)
            : base(historyEvent)
        {
            rule.ThrowIfNull("rule");

            this.rule = rule;
            this.InsertedDate = rule.InsertedDate;
            this.UpdatedDate = rule.UpdatedDate;
            this.UpdatedUserId = rule.UpdatedUserId;
        }

        /// <summary>
        /// Создает новый экземпляр класса <see cref="RuleHistory"/>. Конструтор необходим EF.
        /// </summary>
        protected RuleHistory()
        {
            this.rule = new Rule();
        }

        /// <summary>
        /// Идентификатор сущности <see cref="Rule"/>.
        /// </summary>
        public long RuleId
        {
            get
            {
                return this.rule.Id;
            }

            set
            {
                this.rule.Id = value;
            }
        }

        /// <summary>
        /// Название правила/промоакции.
        /// </summary>
        public string Name
        {
            get
            {
                return this.rule.Name;
            }

            set
            {
                this.rule.Name = value;
            }
        }

        /// <summary>
        /// Идентификатор домена правил в которое входит данное правило.
        /// </summary>
        public long RuleDomainId
        {
            get
            {
                return this.rule.RuleDomainId;
            }

            set
            {
                this.rule.RuleDomainId = value;
            }
        }

        /// <summary>
        /// Тип правила.
        /// </summary>
        public RuleTypes Type
        {
            get
            {
                return this.rule.Type;
            }

            set
            {
                this.rule.Type = value;
            }
        }

        /// <summary>
        /// Время и дата начала действия правила.
        /// </summary>
        public DateTime? DateTimeFrom
        {
            get
            {
                return this.rule.DateTimeFrom;
            }

            set
            {
                this.rule.DateTimeFrom = value;
            }
        }

        /// <summary>
        /// Дата и время после, которой правило прекращает своё действие.
        /// </summary>
        public DateTime? DateTimeTo
        {
            get
            {
                return this.rule.DateTimeTo;
            }

            set
            {
                this.rule.DateTimeTo = value;
            }
        }

        public RuleStatuses Status
        {
            get
            {
                return this.rule.Status;
            }

            set
            {
                this.rule.Status = value;
            }
        }

        /// <summary>
        /// Признак исключающего правила: <c>false</c> - не исключающее, <c>true</c> - исключающее все правила, кроме базовых и помеченных признаком «не исключаемое правило».
        /// У исключающих правил уникальный приоритет.
        /// </summary>
        public bool IsExclusive
        {
            get
            {
                return this.rule.IsExclusive;
            }

            set
            {
                this.rule.IsExclusive = value;
            }
        }

        /// <summary>
        /// Признак не исключаемого правила: <c>false</c> - правило исключаемое, <c>true</c> - правило не исключаемое.
        /// </summary>
        public bool IsNotExcludedBy
        {
            get
            {
                return this.rule.IsNotExcludedBy;
            }

            set
            {
                this.rule.IsNotExcludedBy = value;
            }
        }

        /// <summary>
        /// Приоритет правила, который действует только внутри правил данного типа в данном домене правил. 
        /// Значение должно быть уникальным в рамках всех правил одного домена правил, имеющих тип «Базовое».
        /// </summary>
        public int Priority
        {
            get
            {
                return this.rule.Priority;
            }

            set
            {
                this.rule.Priority = value;
            }
        }

        /// <summary>
        /// Условие (предикат) в формате xml
        /// </summary>
        public string Predicate
        {
            get
            {
                return this.rule.Predicate;
            }

            set
            {
                this.rule.Predicate = value;
            }
        }

        /// <summary>
        /// Числовое значение.
        /// Если тип правила «Базовое», то исходное значение для расчета должно быть умножено на значение этого поля.
        /// Если тип правила «Мультипликативное», то исходное значение для расчета должно быть умножено на значение этого поля.
        /// Если тип правила «Аддитивное», то исходное значение для расчета должно быть просуммировано со значением этого поля.
        /// </summary>
        public decimal Factor
        {
            get
            {
                return this.rule.Factor;
            }

            set
            {
                this.rule.Factor = value;
            }
        }

        /// <summary>
        /// Условные коэффициенты в формате xml.
        /// </summary>
        public string ConditionalFactors
        {
            get
            {
                return this.rule.ConditionalFactors;
            }

            set
            {
                this.rule.ConditionalFactors = value;
            }
        }

        public ApproveStatus Approved
        {
            get
            {
                return this.rule.Approved;
            }

            set
            {
                this.rule.Approved = value;
            }
        }

        public string ApproveDescription
        {
            get
            {
                return this.rule.ApproveDescription;
            }

            set
            {
                this.rule.ApproveDescription = value;
            }
        }
    }
}