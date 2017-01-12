namespace RapidSoft.Loaylty.PromoAction.Api.Entities
{
	/// <summary>
	/// Базовый интерфейс любой сущности сохранаяемой в БД и не имеющей составного первичного ключа.
	/// </summary>
	/// <typeparam name="TKey">
	/// Тип уникального идентификатора.
	/// </typeparam>
	public interface IEntity<TKey>
	{
		/// <summary>
		/// Уникальный идентификатор сущности.
		/// </summary>
		TKey Id { get; set; }
	}
}
