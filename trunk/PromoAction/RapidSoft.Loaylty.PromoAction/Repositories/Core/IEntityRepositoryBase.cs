namespace RapidSoft.Loaylty.PromoAction.Repositories.Core
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// Базовый интефейс загрузки сущностей из хранилища, для сущностей не имеющих составной ключ.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Тип сущности.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// Тип уникального идентификатора сущности.
    /// </typeparam>
    public interface IEntityRepositoryBase<TEntity, in TKey> where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Получение сущности по уникальному идентифкатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <returns>
        /// Полученная сущность.
        /// </returns>
        TEntity Get(TKey id);

        /// <summary>
        /// Получение коллекции всех сущностей.
        /// </summary>
        /// <returns>
        /// Коллекция сущностей.
        /// </returns>
        IList<TEntity> GetAll();
    }
}