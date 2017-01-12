namespace RapidSoft.Loaylty.PromoAction.Api.Entities.History
{
    /// <summary>
    /// Базовая сущность версионинга.
    /// </summary>
    public abstract class BaseHistoryEntity : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHistoryEntity"/> class.
        /// </summary>
        /// <param name="event">
        /// Тип события.
        /// </param>
        protected BaseHistoryEntity(HistoryEvent @event)
        {
            this.Event = @event;
        }

        /// <summary>
        /// Создает новый экземпляр класса <see cref="BaseHistoryEntity"/>. Конструтор необходим EF.
        /// </summary>
        protected BaseHistoryEntity()
        {
        }

        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public long HistoryId { get; set; }

        /// <summary>
        /// Тип события.
        /// </summary>
        public HistoryEvent Event { get; set; }
    }
}