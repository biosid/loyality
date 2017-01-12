namespace RapidSoft.Loaylty.PromoAction.Repositories.Core 
{
    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// The EntityRepository interface.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Тип сущность
    /// </typeparam>
    /// <typeparam name="TKey">
    /// Тип уникального идентификатора сущности
    /// </typeparam>
    public interface ITraceableEntityRepository<TEntity, in TKey> : IEntityRepositoryBase<TEntity, TKey>
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
        /// <param name="userId">
        /// Идентификатор пользователя выполняющего удаление
        /// </param>
        void DeleteById(TKey id, string userId);
    }
}