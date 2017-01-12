namespace RapidSoft.Loaylty.PromoAction.Api.Entities
{
    using System;

    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;

    /// <summary>
    /// Целевая аудитория, хранимая в БД.
    /// </summary>
    public class TargetAudience : ITargetAudience, IEntity<string>
    {
        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Имя целевой аудитории, заданное пользователем.
        /// </summary>
        public string Name { get; set; }

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
        /// Локальная дата обновления целевой аудитории.
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// Всемирно координированные дата и время обновления ЦА.
        /// </summary>
        public DateTime? UpdateDateTimeUtc { get; set; }

        /// <summary>
        /// Имя пользователя обновившего ЦА.
        /// </summary>
        public string UpdateUserId { get; set; }

        /// <summary>
        /// Является ли сегментом
        /// </summary>
        public bool IsSegment { get; set; }

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
        public TargetAudienceHistory ToHistoryEntity(HistoryEvent historyEvent, string userId)
        {
            var now = DateTime.Now;

            switch (historyEvent)
            {
                case HistoryEvent.Create:
                    {
                        var retVal = new TargetAudienceHistory(this, historyEvent)
                                         {
                                             CreateDateTime = now,
                                             CreateDateTimeUtc = now.ToUniversalTime(),
                                             CreateUserId = userId
                                         };
                        return retVal;
                    }

                case HistoryEvent.Update:
                    {
                        var retVal = new TargetAudienceHistory(this, historyEvent)
                                         {
                                             UpdateDateTime = now,
                                             UpdateDateTimeUtc = now.ToUniversalTime(),
                                             UpdateUserId = userId
                                         };
                        return retVal;
                    }

                case HistoryEvent.Delete:
                    {
                        var retVal = new TargetAudienceHistory(this, historyEvent)
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

        /// <summary>
        /// Создает DTO копию текущего объекта.
        /// </summary>
        /// <returns>
        /// Целевая аудитория, возвращаемая из компонента.
        /// </returns>
        public DTO.TargetAudience ToDTO()
        {
            return new DTO.TargetAudience { Id = this.Id, Name = this.Name, IsSegment = this.IsSegment };
        }

        public object ToHistoryEntity(HistoryEvent historyEvent)
        {
            return new TargetAudienceHistory(this, historyEvent);
        }

        public static TargetAudience BuildTargetAudience(string id, string promoActionId, string userId)
        {
            var now = DateTime.Now;
            var ta = new TargetAudience
                         {
                             CreateDateTime = now,
                             CreateDateTimeUtc = now.ToUniversalTime(),
                             CreateUserId = userId,
                             Id = id,
                             IsSegment = false,
                             Name = string.Format("Целевая аудитория для механики {0}", promoActionId),
                             UpdateDateTime = null,
                             UpdateDateTimeUtc = null,
                             UpdateUserId = null,
                         };
            return ta;
        }

        public static TargetAudience BuildSegment(string segmentId, string userId)
        {
            var now = DateTime.Now;
            var segment = new TargetAudience
                                     {
                                         CreateDateTime = now,
                                         CreateDateTimeUtc = now.ToUniversalTime(),
                                         CreateUserId = userId,
                                         Id = segmentId,
                                         IsSegment = true,
                                         Name = segmentId
                                     };
            return segment;
        }
    }
}
