namespace RapidSoft.Loaylty.PromoAction.Api.Entities.History
{
    using System;

    using RapidSoft.Extensions;

    /// <summary>
    /// Историческая сущность связки "Целевая аудитория"-"Клиент (Профиль клиента)".
    /// </summary>
    public class TargetAudienceClientLinkHistory : IHistoryEntity
    {
        /// <summary>
        /// Исходная cвязка "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </summary>
        private readonly TargetAudienceClientLink original;

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetAudienceClientLinkHistory"/> class.
        /// </summary>
        /// <param name="targetAudienceClientLinkargetAudienceClientLink">
        /// The target audience client linkarget audience client link.
        /// </param>
        /// <param name="event">
        /// Историческое событие версионинга.
        /// </param>
        public TargetAudienceClientLinkHistory(TargetAudienceClientLink targetAudienceClientLinkargetAudienceClientLink, HistoryEvent @event)
        {
            targetAudienceClientLinkargetAudienceClientLink.ThrowIfNull("TargetAudienceClientLink");
            this.Event = @event;
            this.original = targetAudienceClientLinkargetAudienceClientLink;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetAudienceClientLinkHistory"/> class.
        /// </summary>
        public TargetAudienceClientLinkHistory()
        {
            this.original = new TargetAudienceClientLink();
        }

        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public long HistoryId { get; set; }

        /// <summary>
        /// Тип события.
        /// </summary>
        public HistoryEvent Event { get; set; }

        /// <summary>
        /// Идентификатор целевой аудитории.
        /// </summary>
        public string TargetAudienceId
        {
            get
            {
                return this.original.TargetAudienceId;
            }

            set
            {
                this.original.TargetAudienceId = value;
            }
        }
        
        /// <summary>
        /// Идентификатор клиента (профиля клиента).
        /// </summary>
        public string ClientId
        {
            get
            {
                return this.original.ClientId;
            }

            set
            {
                this.original.ClientId = value;
            }
        }

        /// <summary>
        /// Локальная дата вставки целевой аудитории.
        /// </summary>
        public DateTime CreateDateTime
        {
            get
            {
                return this.original.CreateDateTime;
            }

            set
            {
                this.original.CreateDateTime = value;
            }
        }

        /// <summary>
        /// Всемирно координированные дата и время вставки ЦА.
        /// </summary>
        public DateTime CreateDateTimeUtc
        {
            get
            {
                return this.original.CreateDateTimeUtc;
            }

            set
            {
                this.original.CreateDateTimeUtc = value;
            }
        }

        /// <summary>
        /// Идентификатор пользователя создавшего ЦА.
        /// </summary>
        public string CreateUserId
        {
            get
            {
                return this.original.CreateUserId;
            }

            set
            {
                this.original.CreateUserId = value;
            }
        }

        /// <summary>
        /// Локальная дата удаления связки.
        /// </summary>
        public DateTime? DeleteDateTime { get; set; }

        /// <summary>
        /// Всемирно координированные дата и время удаления связки.
        /// </summary>
        public DateTime? DeleteDateTimeUtc { get; set; }

        /// <summary>
        /// Имя пользователя удалившего связку.
        /// </summary>
        public string DeleteUserId { get; set; }
    }
}