namespace RapidSoft.Loaylty.PromoAction.Api.DTO
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// Целевая аудитория, возвращаемая из компонента.
    /// </summary>
    [DataContract]
    public class TargetAudience : ITargetAudience
    {
        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Имя целевой аудитории, заданное пользователем.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Является ли сегментом
        /// </summary>
        [DataMember]
        public bool IsSegment { get; set; }
    }
}
