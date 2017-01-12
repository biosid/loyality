using System;
using System.Collections.Generic;
using System.Linq;
using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

namespace RapidSoft.VTB24.BankConnector.Tests.StubServices
{
    public class CatalogAdminServiceStub : StubBase, ICatalogAdminService
    {
        public string Echo(string message)
        {
            throw new NotImplementedException();
        }

        public GetSubCategoriesResult GetAllSubCategories(GetAllSubCategoriesParameters parameters)
        {
            throw new NotImplementedException();
        }

        public CreateCategoryResult CreateCategory(CreateCategoryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public UpdateCategoryResult UpdateCategory(UpdateCategoryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ResultBase DeleteCategory(string userId, int categoryId)
        {
            throw new NotImplementedException();
        }

        public ResultBase ChangeCategoriesStatus(string userId, int[] categoryIds, ProductCategoryStatuses status)
        {
            throw new NotImplementedException();
        }

        public ResultBase MoveCategory(MoveCategoryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public GetPartnerProductCategoryLinksResult GetPartnerProductCategoryLinks(string userId, int partnerId, int[] categoryIds)
        {
            throw new NotImplementedException();
        }

        public CreatePartnerProductCategoryLinkResult SetPartnerProductCategoryLink(
            CreatePartnerProductCateroryLinkParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ResultBase SetCategoriesPermissions(SetCategoriesPermissionsParameters parameters)
        {
            throw new NotImplementedException();
        }

        public GetCategoriesPermissionsResult GetCategoriesPermissions(string userId, int partnerId)
        {
            throw new NotImplementedException();
        }

        public CreatePartnerResult CreatePartner(CreatePartnerParameters parameters)
        {
            throw new NotImplementedException();
        }

        public UpdatePartnerResult UpdatePartner(UpdatePartnerParameters parameters)
        {
            throw new NotImplementedException();
        }

        public GetPartnerResult GetPartners(int[] ids, string userId)
        {
            var result = new GetPartnerResult();
            result.ResultCode = 0;
            result.ResultDescription = GetStubDescription();
            result.Success = true;
            result.Partners =
                ids.Select(
                    x =>
                    new Partner
                        {
                            Description = GetStubDescription(),
                            Id = x,
                            Settings = new Dictionary<string, string> { { "UnitellerShopId", "vtb_partner_1" } },
                        }).ToArray();
            return result;
        }

        public GetPartnersInfoResult GetPartnersInfo(int[] ids, string userId)
        {
            throw new NotImplementedException();
        }

        public GetPartnerByIdResult GetPartnerById(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public GetPartnerInfoByIdResult GetPartnerInfoById(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public GetPartnerByIdResult GetPartnerById(string userId, int partnerId)
        {
            throw new NotImplementedException();
        }

        public ResultBase SetPartnerSettings(string userId, int partnerId, Dictionary<string, string> settings)
        {
            throw new NotImplementedException();
        }

        public ResultBase DeletePartnerSettings(string userId, int partnerId, string[] keys)
        {
            throw new NotImplementedException();
        }

        public PartnersSettignsResult GetPartnersSettings(string userId, int? partnerId)
        {
            throw new NotImplementedException();
        }

        public ImportDeliveryRatesFromHttpResult ImportDeliveryRatesFromHttp(int partnerId, string fileUrl, string userId)
        {
            throw new NotImplementedException();
        }

        public GetDeliveryRateImportTasksHistoryResult GetDeliveryRateImportTasksHistory(GetImportTasksHistoryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public DeliveryLocationsResult GetDeliveryLocations(GetDeliveryLocationsParameters parameters, string userId)
        {
            throw new NotImplementedException();
        }

        public ResultBase SetDeliveryLocationKladr(int locationId, string kladr, string userId)
        {
            throw new NotImplementedException();
        }

        public ResultBase ResetDeliveryLocation(int locationId, string userId)
        {
            throw new NotImplementedException();
        }

        public DeliveryLocationHistoryResult GetDeliveryLocationHistory(GetDeliveryLocationHistoryParameters parameters, string userId)
        {
            throw new NotImplementedException();
        }

        public ImportProductsFromYmlResult ImportProductsFromYmlHttp(ImportProductsFromYmlHttpParameters parameters)
        {
            throw new NotImplementedException();
        }

        public GetProductCatalogImportTasksHistoryResult GetProductCatalogImportTasksHistory(
            GetImportTasksHistoryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public SearchProductsResult SearchProducts(AdminSearchProductsParameters parameters)
        {
            throw new NotImplementedException();
        }

        public AdminGetProductResult GetProductById(ArmGetProductByIdParameters parameters)
        {
            throw new NotImplementedException();
        }

        public CreateProductResult CreateProduct(CreateProductParameters product)
        {
            throw new NotImplementedException();
        }

        public ResultBase UpdateProduct(UpdateProductParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ResultBase DeleteProducts(DeleteProductParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ResultBase MoveProducts(MoveProductsParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ResultBase SetProductsTargetAudiences(SetProductsTargetAudiencesParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ResultBase ChangeProductsStatus(ChangeStatusParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ResultBase ChangeProductsStatusByPartner(ChangeStatusByPartnerParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ResultBase ChangeProductsModerationStatus(ChangeModerationStatusParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ResultBase RecommendProducts(RecommendParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ResultBase DeleteCache(int seconds, string userId)
        {
            throw new NotImplementedException();
        }

	    public ResultBase SaveProductViewsForDay(DateTime date, KeyValuePairOfstringint[] views, string userId)
	    {
		    throw new NotImplementedException();
	    }

	    public SearchOrdersResult SearchOrders(SearchOrdersParameters parameters)
        {
            throw new NotImplementedException();
        }

        public GetOrderResult GetOrderById(string userId, int orderId)
        {
            throw new NotImplementedException();
        }

        public ChangeOrdersStatusesResult ChangeOrdersStatuses(string userId, OrdersStatus[] ordersStatuses)
        {
            throw new NotImplementedException();
        }

        public ResultBase ChangeOrdersStatusDescription(string userId, int orderId, string orderStatusDescription)
        {
            throw new NotImplementedException();
        }

        public ChangeOrdersStatusesResult ChangeOrdersPaymentStatuses(string userId, OrdersPaymentStatus[] statuses)
        {
            throw new NotImplementedException();
        }

        public ChangeOrdersStatusesResult ChangeOrdersDeliveryStatuses(string userId, OrdersDeliveryStatus[] statuses)
        {
            throw new NotImplementedException();
        }

        public GetOrderStatusesHistoryResult GetOrderStatusesHistory(GetOrderStatusesHistoryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public PartnerCommitOrdersResult PartnerCommitOrder(
            string userId, int partnerId, PartnerOrderCommitment[] partnerOrderCommitment)
        {
            throw new NotImplementedException();
        }

        public ResultBase ChangeOrderDeliveryInstructions(string userId, int orderId, string instructions)
        {
            throw new NotImplementedException();
        }
    }
}
