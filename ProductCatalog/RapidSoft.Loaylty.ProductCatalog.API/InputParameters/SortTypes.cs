namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    /// <summary>
    /// Тип сортировки
    /// </summary>
    public enum SortTypes
    {
        ByNameAsc = 0,
        ByNameDesc = 1,
        ByPriceAscByNameAsc = 2,
        ByPriceDescByNameAsc = 3,
        ByInsertedDateDescByNameAsc = 4,
        ByPartnerProductIdAsc = 5,
        ByPopularityDesc = 6,
	    Random = 7,
        Recommended = 8,
        RecommendedByPriceRange = 9,
        RecommendedByCategory = 10,
        ByDiscountDesc = 11
    }
}