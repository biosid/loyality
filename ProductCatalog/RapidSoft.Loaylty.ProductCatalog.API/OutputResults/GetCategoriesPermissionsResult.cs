namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    /// <summary>
    /// Представляет результат операции <see cref="ICatalogAdminService.GetCategoriesPermissions"/>
    /// </summary>
    public class GetCategoriesPermissionsResult : ResultBase
    {
        /// <summary>
        /// Набор идентификаторов разрешенных категорий.
        /// </summary>
        public int[] CategoryIds { get; set; }

        public static GetCategoriesPermissionsResult BuildSuccess(int[] categoryIds = null)
        {
            return new GetCategoriesPermissionsResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           ResultDescription = null,
                           CategoryIds = categoryIds ?? new int[0]
                       };
        }
    }
}