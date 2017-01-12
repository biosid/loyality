namespace RapidSoft.Loaylty.PromoAction.Api.Entities
{
    using System;

    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;

    /// <summary>
    /// Связка "Целевая аудитория"-"Клиент (Профиль клиента)".
    /// </summary>
    public class TargetAudienceClientLink
    {
        /// <summary>
        /// Идентификатор целевой аудитории.
        /// </summary>
        public string TargetAudienceId { get; set; }

        /// <summary>
        /// Целевая аудитория.
        /// </summary>
        public TargetAudience TargetAudience { get; set; }

        /// <summary>
        /// Идентификатор клиента (профиля клиента).
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Локальная дата вставки целевой аудитории.
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Всемирно координированные дата и время вставки ЦА.
        /// </summary>
        public DateTime CreateDateTimeUtc { get; set; }

        /// <summary>
        /// Идентификатор пользователя создавшего ЦА.
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// Метод создает из сущности новую сущность для сохранения в истории.
        /// </summary>
        /// <param name="historyEvent">
        /// Тип события указывающий причину создания сущности версионинга.
        /// </param>
        /// <param name="userId">
        /// Идентификатор пользователя, действия которого приводят к формированию записи версионинга.
        /// </param>
        /// <returns>
        /// Сущность версионинга.
        /// </returns>
        public TargetAudienceClientLinkHistory ToHistoryEntity(HistoryEvent historyEvent, string userId)
        {
            var now = DateTime.Now;

            switch (historyEvent)
            {
                case HistoryEvent.Create:
                    {
                        var retVal = new TargetAudienceClientLinkHistory(this, historyEvent)
                        {
                            CreateDateTime = now,
                            CreateDateTimeUtc = now.ToUniversalTime(),
                            CreateUserId = userId
                        };
                        return retVal;
                    }

                case HistoryEvent.Delete:
                    {
                        var retVal = new TargetAudienceClientLinkHistory(this, historyEvent)
                        {
                            DeleteDateTime = now,
                            DeleteDateTimeUtc = now.ToUniversalTime(),
                            DeleteUserId = userId
                        };

                        return retVal;
                    }

                default:
                    throw new NotSupportedException(string.Format("Тип исторического события {0} не поддерживается", historyEvent));
            }
        }

        public object ToHistoryEntity(HistoryEvent historyEvent)
        {
            return new TargetAudienceClientLinkHistory(this, historyEvent);
        }
    }
}
