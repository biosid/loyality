using System;
using System.Globalization;
using System.Web;
using Vtb24.Site.Security;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Security.SecurityTokenService.Models.Inputs;
using Vtb24.Site.Services.GiftShop.Catalog.Models;
using Vtb24.Site.Services.OnlineCategory.Models.Inputs;
using Vtb24.Site.Services.PartnerSecurityService;

namespace Vtb24.Site.Services.OnlineCategory
{
    public class OnlineCategoryService : IOnlineCategoryService, IDisposable
    {
        public OnlineCategoryService(ClientPrincipal principal, IGiftShop shop, ISecurityTokenService token)
        {
            _principal = principal;
            _shop = shop;
            _token = token;
        }

        private readonly ClientPrincipal _principal;
        private readonly IGiftShop _shop;
        private readonly ISecurityTokenService _token;


        #region API

        public string GetIframeUrl(int categoryId)
        {
            var category = _shop.GetCategoryInfo(categoryId);

            if (category.Category.CategoryType != CategoryType.Online)
            {
                throw new InvalidOperationException(
                    string.Format("Невозможно получить URL категории. Категория не является онлайн. Id {0}", categoryId)
                );
            }

            var ticket = _token.Create(
                new CreateSecurityTokenParameters
                {
                    CategoryId = categoryId,
                    ExternalId = category.Category.OnlineCategoryPartnerId.ToString(),
                    PrincipalId = _principal.ClientId
                }
            );
            return AddQueryToUrl(category.Category.OnlineCategoryUrl, "UserTicket=" + ticket.SecurityToken);
        }

        public string GetOrderSuccessUrl(ReturnUrlParameters parameters)
        {
            parameters.ValidateAndThrow();
            return CreateReturnUrl(0, parameters);
        }

        public string GetOrderErrorUrl(ReturnUrlParameters parameters, string message)
        {
            parameters.ValidateAndThrow();
            return CreateReturnUrl(1, parameters, message);
        }

        public string GetOrderCancelUrl(ReturnUrlParameters parameters)
        {
            parameters.ValidateAndThrow();
            return CreateReturnUrl(3, parameters);
        }

        public string GetNotEnoughPointsUrl(ReturnUrlParameters parameters)
        {
            parameters.ValidateAndThrow();
            return CreateReturnUrl(4, parameters);
        }

        public int? GetCategoryIdByUserTicket(string userTicket)
        {
            var data = _token.Validate(new ValidateSecurityTokenParameters { SecurityToken = userTicket });
            return data.CategoryId;
        }

        #endregion


        private string CreateReturnUrl(int status, ReturnUrlParameters options, string error = null)
        {
            var data = _token.Validate(new ValidateSecurityTokenParameters { SecurityToken = options.UserTicket });
            if (!data.CategoryId.HasValue)
            {
                throw new InvalidOperationException(
                    "Невозможно сформировать URL возврата для онлайн категории. Неверно заполнен токен. Токен: " 
                    + options.UserTicket
                );
            }
            var category = _shop.GetCategoryInfo(data.CategoryId.Value);

            var url = category.Category.OnlineCategoryReturnUrl;
            var query = CreateQuery(status, options);

            return AddQueryToUrl(url, query);
        }

        private static string CreateQuery(int status, ReturnUrlParameters options, string error = null)
        {
            var discount = options.Total.ToString("F2", CultureInfo.InvariantCulture);
            var time = DateTime.UtcNow.ToString("o");

            var query =  string.Format(
                "OrderId={0}&Status={1}&Discount={2}&Error={3}&UtcDateTime={4}",
                HttpUtility.UrlEncode(options.ExternalOrderId),
                status,
                HttpUtility.UrlEncode(discount),
                HttpUtility.UrlEncode(error),
                HttpUtility.UrlEncode(time)
            );

            var signString = string.Format(
                "{0}{1}{2}{3}{4}",
                options.ExternalOrderId,
                status,
                discount,
                error,
                time
            );

            using (var service = new PartnerSecurityServiceClient())
            {
                var signature = service.CreateBankSignature(signString);
                return query + "&Signature=" + HttpUtility.UrlEncode(signature.Signature);
            }
        }

        private static string AddQueryToUrl(string url, string query)
        {
            var separator = url.Contains("?") ? '&' : '?';
            return url + separator + query;
        }

        public void Dispose()
        {
            // Do nothing
        }
    }
}