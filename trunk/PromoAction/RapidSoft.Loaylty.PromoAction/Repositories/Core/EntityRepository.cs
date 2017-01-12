namespace RapidSoft.Loaylty.PromoAction.Repositories.Core
{
    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// Базовая реализация интерфейса <see cref="IEntityRepository{TEntity,TKey}"/>
    /// </summary>
    /// <typeparam name="TEntity">
    /// Тип сущность.
    /// </typeparam>
    public abstract class EntityRepository<TEntity> : EntityRepositoryBase<TEntity>, IEntityRepository<TEntity, long>
        where TEntity : class, IEntity<long>
    {
        /// <summary>
        /// Сохранение сущности в хранилище.
        /// </summary>
        /// <param name="entity">
        /// Сохраняемая сущность.
        /// </param>
        public void Save(TEntity entity)
        {
            this.BeforeSave(entity);

            entity.ThrowIfNull("entity");

            this.ExecuteSave(entity);
        }

        /// <summary>
        /// Удаление сущности из хранилища по уникальному идентификатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        public void DeleteById(long id)
        {
            var entity = this.GetEntityDbSet().Find(id);

            this.ExecuteDelete(entity);
        }
    }
}