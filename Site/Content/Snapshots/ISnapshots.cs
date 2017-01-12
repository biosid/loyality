using Vtb24.Site.Content.Snapshots.Models;

namespace Vtb24.Site.Content.Snapshots
{
    internal interface ISnapshots
    {
        /// <summary>
        /// Создание снимка сущности для истории
        /// </summary>
        /// <remarks>
        /// Снимок создается только в том случае, если он отличается от последнего добавленного
        /// </remarks>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="entityId">id сущности. Требуется для связывания</param>
        /// <param name="entity">сама сущность</param>
        /// <param name="author">пользователь, создавший снимок</param>
        bool Create<T>(long entityId, T entity, string author) where T : class;

        /// <summary>
        /// Получение снимка по его Id
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="id">Id снимка</param>
        /// <returns>снимок сущности</returns>
        Snapshot<T> GetById<T>(string id) where T : class;

        /// <summary>
        /// Получение списка снимков сущности по ее Id
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="entityId">Id сущности</param>
        /// <returns>массив снимков сущности</returns>
        Snapshot<T>[] GetByEntityId<T>(long entityId) where T : class;
    }
}