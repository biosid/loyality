using Vtb24.Site.Services.OnlineCategory.Models.Inputs;

namespace Vtb24.Site.Services
{
    public interface IOnlineCategoryService
    {
        string GetIframeUrl(int categoryId);

        string GetOrderSuccessUrl(ReturnUrlParameters parameters);

        string GetOrderErrorUrl(ReturnUrlParameters parameters, string message);

        string GetOrderCancelUrl(ReturnUrlParameters parameters);

        string GetNotEnoughPointsUrl(ReturnUrlParameters parameters);

        int? GetCategoryIdByUserTicket(string userTicket);
    }
}