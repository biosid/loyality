namespace RapidSoft.Loaders.Geocoder.Entities.Google
{
    /// <summary>
    /// Точность геокодирования
    /// </summary>
    public enum GeocodingAccuracy
    {
        /// <summary>
        /// Точность неизвестна
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Точность на уровне страны
        /// </summary>
        Country = 1,

        /// <summary>
        /// Точность на уровне региона (штат, область, префектура и т. д.) уровень точности
        /// </summary>
        Region = 2,

        /// <summary>
        /// Точность на уровне составных частей регионов (район, муниципалитет и т. д.)
        /// уровень точности
        /// </summary>
        SubRegion = 3,

        /// <summary>
        /// Точность на уровне города (поселка)
        /// </summary>
        Town = 4,

        /// <summary>
        /// Точность на уровне почтового индекса
        /// </summary>
        PostCode = 5,

        /// <summary>
        /// Точность на уровне улицы
        /// </summary>
        Street = 6,

        /// <summary>
        /// Точность на уровне перекрестка
        /// </summary>
        Intersection = 7,

        /// <summary>
        /// Точность на уровне адреса
        /// </summary>
        Address = 8,

        /// <summary>
        /// Точность на уровне здания (название постройки, дома, торговый центр, и т.–д.)
        /// уровень точности
        /// </summary>
        Premise = 9
    }
}