namespace RapidSoft.Loaders.Geocoder.Entities.Yandex
{
    public enum AddressKind
    {
        /// <summary>
        /// Отдельный дом
        /// </summary>
        House = 0,

        /// <summary>
        /// Улица
        /// </summary>
        Street = 1,

        /// <summary>
        /// Станция метро
        /// </summary>
        Metro = 2,

        /// <summary>
        /// Район города
        /// </summary>
        District = 3,

        /// <summary>
        /// Населённый пункт: город/поселок/деревня/село/...
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
        Country = 7,

        /// <summary>
        /// Река,озеро,ручей,водохранилище...
        /// </summary>
        Hydro = 8,

        /// <summary>
        /// Ж/д. станция
        /// </summary>
        Railway = 9,

        /// <summary>
        /// Линия метро / шоссе / ж.д. линия
        /// </summary>
        Route = 10,

        /// <summary>
        /// Лес, парк...
        /// </summary>
        Vegetation = 11,

        /// <summary>
        /// Кладбище
        /// </summary>
        Cemetery = 12,

        /// <summary>
        /// Мост
        /// </summary>
        Bridge = 13,

        /// <summary>
        /// Километр шоссе
        /// </summary>
        Km = 14,

        /// <summary>
        /// Разное
        /// </summary>
        Other = 15
    }
}