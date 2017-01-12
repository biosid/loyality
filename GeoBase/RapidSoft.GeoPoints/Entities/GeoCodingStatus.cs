namespace RapidSoft.GeoPoints.Entities
{
    /// <summary>
    /// Статус геокодирования
    /// </summary>
    public enum GeoCodingStatus
    {
        /// <summary>
        /// Необработана
        /// </summary>
        Unproccessed = 0,
        /// <summary>
        /// Валидна
        /// </summary>
        Valid = 1,
        /// <summary>
        /// Не валидна
        /// </summary>
        Invalid = 2
    }
}
