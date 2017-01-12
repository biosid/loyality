namespace RapidSoft.Loaylty.PromoAction.Api.Entities
{
    using System.Runtime.Serialization;

    /// <summary>
	/// Интерфейс сущности "Целевая аудитория".
	/// </summary>
	public interface ITargetAudience
	{
		/// <summary>
		/// Уникальный идентификатор.
		/// </summary>
		[DataMember]
		string Id { get; set; }

		/// <summary>
		/// Имя целевой аудитории, заданное пользователем.
		/// </summary>
		[DataMember]
		string Name { get; set; }

        /// <summary>
        /// Является ли сегментом
        /// </summary>
        [DataMember]
        bool IsSegment { get; set; }
	}
}