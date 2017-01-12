namespace RapidSoft.Loaders.Geocoder.Entities.Yandex
{
    public enum GeocodingPrecision
    {
        /// <summary>
        /// Точное соответствие
        /// </summary>
        Exact = 0,

        /// <summary>
        /// Совпадает только номер дома
        /// </summary>
        Number = 1,

        /// <summary>
        /// Найден дом поблизости (так как 18–16 &lt; 10)
        /// </summary>
        Near = 2,

        /// <summary>
        /// Найдена улица (так как 18–4 &gt; 10)
        /// </summary>
        Street = 3,

        /// <summary>
        /// Улица не найдена, но найден, например, посёлок, район, и т. д.
        /// </summary>
        Other = 4
    }
}