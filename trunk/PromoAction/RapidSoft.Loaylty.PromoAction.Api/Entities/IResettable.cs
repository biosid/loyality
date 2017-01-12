namespace RapidSoft.Loaylty.PromoAction.Api.Entities
{
	/// <summary>
	/// Интерфейс сущности поддерживающей копирование свойств из другой сущности того же типа.
	/// </summary>
	/// <typeparam name="TEntity">
	/// Тип сущности.
	/// </typeparam>
	public interface IResettable<in TEntity> where TEntity : class
	{
		/// <summary>
		/// Копирование свойств из <paramref name="entity"/>.
		/// </summary>
		/// <param name="entity">
		/// Сущность из которой выполняется копирование.
		/// </param>
		void ResetFrom(TEntity entity);
	}
}
