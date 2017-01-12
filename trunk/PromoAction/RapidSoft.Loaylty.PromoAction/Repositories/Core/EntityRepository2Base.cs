namespace RapidSoft.Loaylty.PromoAction.Repositories.Core
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// Базовая реализация интерфейса <see cref="IEntityRepositoryBase{TEntity,TKey}"/>
    /// </summary>
    /// <typeparam name="TEntity">
    /// Тип сущности
    /// </typeparam>
    public abstract class EntityRepository2Base<TEntity> : IEntityRepositoryBase<TEntity, string>
        where TEntity : class, IEntity<string>
    {
        /// <summary>
        /// Получение сущности по уникальному идентифкатору
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <returns>
        /// Полученная сущность.
        /// </returns>
        public TEntity Get(string id)
        {
            return this.GetEntityDbSet().SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Получение коллекции всех сущностей.
        /// </summary>
        /// <returns>
        /// Коллекция сущностей.
        /// </returns>
        public IList<TEntity> GetAll()
        {
            return this.GetEntityDbSet().ToList();
        }

        /// <summary>
        /// Контекст доступа к БД.
        /// </summary>
        /// <returns>
        /// Контекст доступа к данным.
        /// </returns>
        protected abstract DbContext GetContext();

        /// <summary>
        /// Метод получения коллекции типизированных сущностей
        /// </summary>
        /// <returns>
        /// Коллекция типизированных сущностей
        /// </returns>
        protected virtual DbSet<TEntity> GetEntityDbSet()
        {
            return this.GetContext().Set<TEntity>();
        }
    }
}