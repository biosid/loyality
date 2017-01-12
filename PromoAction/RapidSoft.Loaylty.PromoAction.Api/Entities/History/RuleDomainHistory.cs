namespace RapidSoft.Loaylty.PromoAction.Api.Entities.History
{
    using RapidSoft.Extensions;

    /// <summary>
    /// Историческая сущность домена правил.
    /// </summary>
    public class RuleDomainHistory : BaseHistoryEntity
    {
        /// <summary>
        /// Исходный домен правил.
        /// </summary>
        private readonly RuleDomain ruleDomain;

        /// <summary>
        /// Создает новый экземпляр класса <see cref="RuleDomainHistory"/>. Конструтор необходим EF.
        /// </summary>
        public RuleDomainHistory()
        {
            this.ruleDomain = new RuleDomain();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleDomainHistory"/> class.
        /// </summary>
        /// <param name="ruleDomain">
        /// The rule domain.
        /// </param>
        /// <param name="event">
        /// The event.
        /// </param>
        public RuleDomainHistory(RuleDomain ruleDomain, HistoryEvent @event)
            : base(@event)
        {
            ruleDomain.ThrowIfNull("ruleDomain");

            this.ruleDomain = ruleDomain;
            this.UpdatedUserId = ruleDomain.UpdatedUserId;
            this.InsertedDate = ruleDomain.InsertedDate;
        }

        /// <summary>
        /// Идентификатор сущности <see cref="RuleDomain"/>.
        /// </summary>
        public long RuleDomainId
        {
            get
            {
                return this.ruleDomain.Id;
            }

            set
            {
                this.ruleDomain.Id = value;
            }
        }

        /// <summary>
        /// Имя домена правил. Уникально в рамках данного реестра.
        /// </summary>
        public string Name
        {
            get
            {
                return this.ruleDomain.Name;
            }

            set
            {
                this.ruleDomain.Name = value;
            }
        }

        /// <summary>
        /// Краткое описание домен правил.
        /// </summary>
        public string Description
        {
            get
            {
                return this.ruleDomain.Description;
            }

            set
            {
                this.ruleDomain.Description = value;
            }
        }

        public string Metadata
        {
            get
            {
                return this.ruleDomain.Metadata;
            }

            set
            {
                this.ruleDomain.Metadata = value;
            }
        }

        /// <summary>
        /// Тип расчета лимитного результата
        /// </summary>
        public LimitTypes LimitType
        {
            get
            {
                return this.ruleDomain.LimitType;
            }

            set
            {
                this.ruleDomain.LimitType = value;
            }
        }

        /// <summary>
        /// Коэффициент для расчета лимитного результата
        /// </summary>
        public decimal LimitFactor
        {
            get
            {
                return this.ruleDomain.LimitFactor;
            }

            set
            {
                this.ruleDomain.LimitFactor = value;
            }
        }

        /// <summary>
        /// Тип граничного лимита
        /// </summary>
        public LimitTypes StopLimitType
        {
            get
            {
                return this.ruleDomain.StopLimitType;
            }

            set
            {
                this.ruleDomain.StopLimitType = value;
            }
        }

        /// <summary>
        /// Коэффициент граничного лимитного значения
        /// </summary>
        public decimal StopLimitFactor
        {
            get
            {
                return this.ruleDomain.StopLimitFactor;
            }

            set
            {
                this.ruleDomain.StopLimitFactor = value;
            }
        }

        public decimal DefaultBaseMultiplicationFactor
        {
            get
            {
                return this.ruleDomain.DefaultBaseMultiplicationFactor;
            }

            set
            {
                this.ruleDomain.DefaultBaseMultiplicationFactor = value;
            }
        }

        public decimal DefaultBaseAdditionFactor
        {
            get
            {
                return this.ruleDomain.DefaultBaseAdditionFactor;
            }

            set
            {
                this.ruleDomain.DefaultBaseAdditionFactor = value;
            }
        }
    }
}