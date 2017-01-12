namespace RapidSoft.Loaylty.ProductCatalog.Import.ProductCorrectingFilter
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    /// <summary>
    /// Интерфейс корректирующего фильтра - инкапсулирует логику проверки продукта и принимает решение:
    /// 1. O игнорировании продукта как ошибочного
    /// 2. О корректировании данных продукта и последующей обработке
    /// 3. О последующей обработке без вмешательства
    /// </summary>
    internal interface IProductCorrectingFilter
    {
        Result Execute(Product product);
    }
}
