namespace RapidSoft.Loaylty.PromoAction.Repositories.Core 
{
    using System;

    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// Базовая реализация интерфейса <see cref="ITraceableEntityRepository{TEntity,TKey}"/>
    /// </summary>
    /// <typeparam name="TEntity">
    /// Тип сущность.
    /// </typeparam>
    public abstract class TraceableEntityRepository<TEntity> : EntityRepositoryBase<TEntity>, ITraceableEntityRepository<TEntity, long> where TEntity : class, ITraceableEntity<long>, IEntity<long>
    {
        /// <summary>
        /// Сохранение сущности в хранилище.
        /// </summary>
        /// <param name="entity">
        /// Сохраняемая сущность.
        /// </param>
        public virtual void Save(TEntity entity)
        {
            this.BeforeSave(entity);

            entity.ThrowIfNull("entity");

            entity.InsertedDate = DateTime.Now;

            this.ExecuteSave(entity);
        }

        /// <summary>
        /// Удаление сущности из хранилища по уникальному идентификатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <param name="userId">
        /// Идентификатор пользователя выполняющего удаление
        /// </param>
        public virtual void DeleteById(long id, string userId)
        {
            var entity = this.GetEntityDbSet().Find(id);

            if (entity == null)
            {
                return;
            }

            entity.InsertedDate = DateTime.Now;
            entity.UpdatedUserId = userId;

            this.ExecuteDelete(entity);
        }
    }
}