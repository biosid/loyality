namespace Rapidsoft.Loyalty.NotificationSystem.Repositories
{
    using Rapidsoft.Loyalty.NotificationSystem.Entities;

    /// <summary>
    /// Интерфейс репозитория для сущности <see cref="ClientProfile"/>.
    /// </summary>
    public interface IClientInboxProfileRepository
    {
        /// <summary>
        /// Получение профиля по уникальному идентификатору клиента.
        /// </summary>
        /// <param name="clientId">
        /// Идентификатор клиента.
        /// </param>
        /// <returns>
        /// Профиль клиента.
        /// </returns>
        ClientProfile Get(string clientId);

        /// <summary>
        /// Сохранение профиля клиента.
        /// </summary>
        /// <param name="profile">
        /// Профиль клиента.
        /// </param>
        void Save(ClientProfile profile);

        /// <summary>
        /// Признак существования профиля по уникальному идентификатору клиента.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        bool Exist(string clientId);
    }
}
