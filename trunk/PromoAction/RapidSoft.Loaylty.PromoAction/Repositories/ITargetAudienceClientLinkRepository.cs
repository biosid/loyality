namespace RapidSoft.Loaylty.PromoAction.Repositories
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    public interface ITargetAudienceClientLinkRepository
    {
        /// <summary>
        /// Получение связки "Целевая аудитория"-"Клиент (Профиль клиента)" по идентифкаторам.
        /// </summary>
        /// <param name="targetAudienceId">
        /// Идентификатор целевой аудитории.
        /// </param>
        /// <param name="clientId">
        /// Идентификатор клиента.
        /// </param>
        /// <returns>
        /// Найденная связка "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </returns>
        TargetAudienceClientLink Get(string targetAudienceId, string clientId);

        /// <summary>
        /// Получение коллекции всех связок "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </summary>
        /// <returns>
        /// Коллекция связок "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </returns>
        IList<TargetAudienceClientLink> GetAll();

        /// <summary>
        /// Сохранение связки "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </summary>
        /// <remarks>
        /// Выполняется добавление, обновление не выполяется!
        /// </remarks>
        /// <param name="targetAudienceClientLink">
        /// Связка "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </param>
        void Insert(TargetAudienceClientLink targetAudienceClientLink);

        /// <summary>
        /// Удаление связки "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </summary>
        /// <param name="targetAudienceId">
        /// Идентификатор целевой аудитории.
        /// </param>
        /// <param name="clientId">
        /// Идентификатор клиента.
        /// </param>
        /// <param name="userId">
        /// Пользователь выполняющий удаление.
        /// </param>
        void DeleteById(string targetAudienceId, string clientId, string userId);

        void DeleteSegment(string clientId, string userId);
    }
}