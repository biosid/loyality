namespace RapidSoft.Loaylty.PromoAction.Repositories.Core
{
    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    public interface IEntityRepository<TEntity, in TKey> : IEntityRepositoryBase<TEntity, TKey>
		where TEntity : class, IEntity<TKey>
	{
		/// <summary>
		/// Сохранение сущности в хранилище.
		/// </summary>
		/// <param name="entity">
		/// Сохраняемая сущность.
		/// </param>
		void Save(TEntity entity);

		/// <summary>
		/// Удаление сущности из хранилища по уникальному идентификатору.
		/// </summary>
		/// <param name="id">
		/// Уникальный идентификатор.
		/// </param>
		void DeleteById(TKey id);
	}
}