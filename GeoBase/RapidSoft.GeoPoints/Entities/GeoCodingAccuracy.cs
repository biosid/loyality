namespace RapidSoft.GeoPoints.Entities
{
    /// <summary>
    /// Точность геокодирования (тип адреса)
    /// </summary>
    public enum GeoCodingAccuracy
    {
        /// <summary>
        /// Точность неизвестна
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Дом, здание
        /// </summary>
        House = 1,

        /// <summary>
        /// Улица
        /// </summary>
        Street = 2,
        
        /// <summary>
        /// Район города
        /// </summary>
        District = 3,

        /// <summary>
        /// Город, поселение
        /// </summary>
        City = 4,

        /// <summary>
        /// Район области
        /// </summary>
        Area = 5,

        /// <summary>
        /// Область
        /// </summary>
        Province = 6,

        /// <summary>
        /// Страна
        /// </summary>
        Country = 7
    }
}