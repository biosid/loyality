namespace RapidSoft.Loaylty.PromoAction.Api.Entities
{
    using System;

    /// <summary>
    /// Интерфейс сущности выполняющей "отслеживание" изменений.
    /// </summary>
    /// <typeparam name="TKey">
    /// Тип уникального идентификатора.
    /// </typeparam>
    public interface ITraceableEntity<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// Дата и время создания.
        /// </summary>
        DateTime InsertedDate { set; }

        /// <summary>
        /// Дата и время создания.
        /// </summary>
        DateTime? UpdatedDate { set; }

        /// <summary>
        /// Идентификатор пользователя в системе безопасности, который внес последнее изменение.
        /// </summary>
        string UpdatedUserId { set; }
    }
}