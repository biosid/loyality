namespace RapidSoft.Loaylty.PromoAction.Api.Entities.History
{
    using System;

    using RapidSoft.Extensions;

    /// <summary>
    /// Историческая сущность целевой аудитории.
    /// </summary>
    public class TargetAudienceHistory : IHistoryEntity
    {
        /// <summary>
        /// Исходная целевой аудитории.
        /// </summary>
        private readonly TargetAudience original;

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetAudienceHistory"/> class.
        /// </summary>
        /// <param name="targetAudience">
        /// The target audience.
        /// </param>
        /// <param name="event">
        /// Историческое событие версионинга
        /// </param>
        public TargetAudienceHistory(TargetAudience targetAudience, HistoryEvent @event)
        {
            targetAudience.ThrowIfNull("targetAudience");
            this.Event = @event; 
            this.original = targetAudience;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetAudienceHistory"/> class.
        /// </summary>
        public TargetAudienceHistory()
        {
            this.original = new TargetAudience();
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
        /// Уникальный идентификатор целевой аудитории.
        /// </summary>
        public string TargetAudienceId
        {
            get
            {
                return this.original.Id;
            }

            set
            {
                this.original.Id = value;
            }
        }

        /// <summary>
        /// Имя целевой аудитории, заданное пользователем.
        /// </summary>
        public string Name
        {
            get
            {
                return this.original.Name;
            }

            set
            {
                this.original.Name = value;
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
        /// Локальная дата обновления целевой аудитории.
        /// </summary>
        public DateTime? UpdateDateTime
        {
            get
            {
                return this.original.UpdateDateTime;
            }

            set
            {
                this.original.UpdateDateTime = value;
            }
        }

        /// <summary>
        /// Всемирно координированные дата и время обновления ЦА.
        /// </summary>
        public DateTime? UpdateDateTimeUtc
        {
            get
            {
                return this.original.UpdateDateTimeUtc;
            }

            set
            {
                this.original.UpdateDateTimeUtc = value;
            }
        }

        /// <summary>
        /// Идентификатор пользователя обновившего ЦА.
        /// </summary>
        public string UpdateUserId
        {
            get
            {
                return this.original.UpdateUserId;
            }

            set
            {
                this.original.UpdateUserId = value;
            }
        }

        /// <summary>
        /// Является ли сегментом
        /// </summary>
        public bool IsSegment
        {
            get
            {
                return this.original.IsSegment;
            }

            set
            {
                this.original.IsSegment = value;
            }
        }

        /// <summary>
        /// Локальная дата удаления целевой аудитории.
        /// </summary>
        public DateTime? DeleteDateTime { get; set; }

        /// <summary>
        /// Всемирно координированные дата и время удаления ЦА.
        /// </summary>
        public DateTime? DeleteDateTimeUtc { get; set; }

        /// <summary>
        /// Идентификатор пользователя удалившего ЦА.
        /// </summary>
        public string DeleteUserId { get; set; }
    }
}
