namespace RapidSoft.Loaylty.PromoAction.Api.Entities.History
{
    using System;

    /// <summary>
	/// Интерфейс сущности представляющий историческую запись.
	/// </summary>
	public interface IHistoryEntity
	{
		/// <summary>
		/// Уникальный идентификатор.
		/// </summary>
		long HistoryId { get; set; }

		/// <summary>
		/// Тип события.
		/// </summary>
		HistoryEvent Event { get; set; }

		/// <summary>
		/// Локальная дата удаления сущности.
		/// </summary>
		DateTime? DeleteDateTime { get; set; }

		/// <summary>
		/// Всемирно координированные дата и время удаления сущности.
		/// </summary>
		DateTime? DeleteDateTimeUtc { get; set; }

		/// <summary>
		/// Имя пользователя удалившего сущность.
		/// </summary>
		string DeleteUserId { get; set; }
	}
}
