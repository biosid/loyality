namespace RapidSoft.Loaylty.PromoAction.Api.Entities.History
{
	/// <summary>
	/// Типы исторических событий для версионинга.
	/// </summary>
	public enum HistoryEvent
	{
		/// <summary>
		/// Событие не определено. Данный тип события не должен появляться в исторических данных, 
		/// но так как ведение исторических данных второстепенная задача, чтобы избежать ошибки предусмотрен данный тип.
		/// </summary>
		Unknow = 0,

		/// <summary>
		/// Событие создания.
		/// </summary>
		Create = 1,

		/// <summary>
		/// Событие обновления.
		/// </summary>
		Update = 2,

		/// <summary>
		/// Событие удаления.
		/// </summary>
		Delete = 3,
	}
}