namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using API.Entities;
    using API.InputParameters;
    using API.OutputResults;

    using AutoMapper;

    using DataSources;
    using DataSources.Interfaces;

    using Interfaces;

    using Settings;

    public class CategoriesSearcher : ICategoriesSearcher
    {
        private readonly IProductCategoriesDataSource categoriedDataSource;
        private readonly IMechanicsProvider mechanicsProvider;

        static CategoriesSearcher()
        {
            Mapper.CreateMap<GetAllSubCategoriesParameters, GetPublicSubCategoriesParameters>();
        }

        public CategoriesSearcher()
        {
            categoriedDataSource = new ProductCategoriesDataSource();
            mechanicsProvider = new MechanicsProvider();
        }

        public CategoriesSearcher(IProductCategoriesDataSource categoriedDataSource = null, IMechanicsProvider mechanicsProvider = null)
        {
            this.categoriedDataSource = categoriedDataSource ?? new ProductCategoriesDataSource();
            this.mechanicsProvider = mechanicsProvider ?? new MechanicsProvider();
        }

        #region ICategoriesSearcher Members

        public GetSubCategoriesResult GetPublicSubCategories(GetPublicSubCategoriesParameters parameters)
        {
            var countToTake = parameters.CountToTake.NormalizeByHeight(ApiSettings.MaxResultsCountCategories);

            var parser = new ClientContextParser();
            var location = parser.GetLocationKladrCode(parameters.ClientContext);
            var audiences = parser.GetAudienceIds(parameters.ClientContext);

            var priceSql = mechanicsProvider.GetPriceSql(parameters.ClientContext);

            var searchCategoriesResult = categoriedDataSource.GetPublicCategories(
                priceSql,
                location,
                categoriesStatus: parameters.Status,
                parentId: parameters.ParentId,
                nestingLevel: parameters.NestingLevel, 
                countToTake: countToTake, 
                countToSkip: parameters.CountToSkip, 
                calcTotalCount: parameters.CalcTotalCount, 
                audienceIds: audiences, 
                includeParent: parameters.IncludeParent, 
                type: parameters.Type,
                categoriesIds: parameters.CategoryIds);

            var result = GetSubCategoriesResult.BuildSuccess(
                searchCategoriesResult.Categories,
                ApiSettings.MaxResultsCountCategories,
                searchCategoriesResult.TotalCount,
                searchCategoriesResult.ChildrenCount);

            return result;
        }

        public GetSubCategoriesResult GetAllSubCategories(GetAllSubCategoriesParameters parameters)
        {
            var countToTake = parameters.CountToTake.NormalizeByHeight(ApiSettings.MaxResultsCountCategories);

            var searchCategoriesResult = categoriedDataSource.AdminGetCategories(
                categoriesStatus: parameters.Status,
                parentId: parameters.ParentId,
                nestingLevel: parameters.NestingLevel,
                countToTake: countToTake,
                countToSkip: parameters.CountToSkip,
                calcTotalCount: parameters.CalcTotalCount,
                includeParent: parameters.IncludeParent,
                type: parameters.Type);

            var result = GetSubCategoriesResult.BuildSuccess(
                searchCategoriesResult.Categories,
                ApiSettings.MaxResultsCountCategories,
                searchCategoriesResult.TotalCount,
                searchCategoriesResult.ChildrenCount);
            return result;
        }

        public GetCategoryInfoResult GetCategoryInfo(GetCategoryInfoParameters parameters)
        {
            var result = new GetCategoryInfoResult();

            var cats = this.categoriedDataSource.GetParentCategoriesPath(parameters.CategoryId);

            if (cats == null || !cats.Any())
            {
                result.ResultCode = ResultCodes.CATEGORY_NOT_FOUND;
                result.ResultDescription = "Категория не найдена";
                return result;
            }

            var ids = cats.Select(x => x.Id).ToArray();
            var categoriesParameters = new GetPublicSubCategoriesParameters
                               {
                                   ClientContext = parameters.ClientContext,
                                   CategoryIds = ids,
                                   Status = null
                               };
            var categoriesResult = this.GetPublicSubCategories(categoriesParameters);

            if (!categoriesResult.Success)
            {
                throw new OperationException(categoriesResult.ResultCode, result.ResultDescription);
            }

            var sortedCatd = categoriesResult.Categories.OrderBy(c => c.NestingLevel).ToArray();

            var productCategory = sortedCatd.Last();
            
            if (productCategory.Status == ProductCategoryStatuses.NotActive)
            {
                result.ResultCode = ResultCodes.CATEGORY_NOT_FOUND;
                result.ResultDescription = "Категория деактивирована";
                return result;
            }

            result.Category = productCategory;
            result.CategoryPath = sortedCatd;
            result.ResultCode = ResultCodes.SUCCESS;

            return result;
        }

        #endregion
    }
}