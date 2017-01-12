namespace RapidSoft.Loaylty.PromoAction.Repositories
{
    using System.Collections.Generic;

    using Api.Entities;

    /// <summary>
    /// Сервисный интерфейс репозитория целевых аудиторий.
    /// </summary>
    public interface ITargetAudienceRepository
    {
        /// <summary>
        /// Возвращает набор целевых аудиторий в которые входит клиент.
        /// </summary>
        /// <param name="clientId">
        /// Уникальный идентификатор клиента.
        /// </param>
        /// <returns>
        /// Набор целевых аудиторий в которые входит клиент.
        /// </returns>
        IList<TargetAudience> GetByClientId(string clientId);

        IList<TargetAudience> GetBySegment(bool? isSegment);

        TargetAudience Get(string id);

        bool Exists(string id);

        void Save(TargetAudience targetAudience);

        void DeleteById(string id, string userId);
    }
}