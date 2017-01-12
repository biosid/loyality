namespace RapidSoft.Loaylty.PromoAction.Api.Entities.History
{
    public interface IHistoryTraceable
    {
        object ToHistoryEntity(HistoryEvent historyEvent);
    }

    /// <summary>
    /// Интерфейс поддержки версионинга. Сущность поддерживающая версионинг должна реализовывать данный интерфейс.
    /// </summary>
    /// <typeparam name="THistoryEntity">
    /// Тип получаемой сущности версионинга.
    /// </typeparam>
    public interface IHistoryTraceable<out THistoryEntity> : IHistoryTraceable where THistoryEntity : class
    {
        /// <summary>
        /// Метод создает из сущности новую сущность для сохранения в истории.
        /// </summary>
        /// <param name="historyEvent">
        /// Тип события указывающий причину создания сущности версионинга.
        /// </param>
        /// <param name="userId">
        /// Идентификатор пользователя, действия которого приводят к формированию записи версионинга.
        /// </param>
        /// <returns>
        /// Сущность версионинга.
        /// </returns>
        THistoryEntity ToHistoryEntity(HistoryEvent historyEvent, string userId);
    }
}
