namespace RapidSoft.Loaylty.ProductCatalog.Interfaces
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public interface IPartnersSearcher
    {
        /// <summary>
        /// Поиск партнеров
        /// </summary>
        /// <returns>Возвращает всех партнеров</returns>
        Partner[] Search();
    }
}
