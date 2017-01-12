using System;
using System.Linq;
using Vtb24.Site.Services.BankConnectorService;
using Vtb24.Site.Services.BankProducts.Models;
using Vtb24.Site.Services.BankProducts.Models.Exceptions;
using Vtb24.Site.Services.BankProducts.Models.Outputs;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.BankProducts
{
    public class BankProductsService : IBankProductsService
    {
        public BankProductsResult GetProducts(string clientId, DateTime minExpirationDate, PagingSettings pagingSettings)
        {
            var parameters = new BankOffersServiceParameter
            {
                ClientId = clientId,
                ExpirationDate = minExpirationDate,
                SkipCount = pagingSettings.Skip,
                TakeCount = pagingSettings.Take,
                CountTotal = true
            };

            using (var service = new BankConnectorClient())
            {
                var response = service.GetBankOffers(parameters);

                AssertResponse(response.ResultCode, response.Error);

                var products = response.Result.Offers.Select(MappingsFromService.ToBankProduct);

                return new BankProductsResult(products, response.Result.TotalCount, pagingSettings);
            }
        }

        public BankProduct GetProduct(string id, string clientId, DateTime minExpirationDate)
        {
            var parameters = new BankOffersServiceParameter
            {
                ClientId = clientId,
                Id = id,
                ExpirationDate = minExpirationDate,
                SkipCount = 0,
                TakeCount = 1,
                CountTotal = false
            };

            using (var service = new BankConnectorClient())
            {
                var response = service.GetBankOffers(parameters);

                AssertResponse(response.ResultCode, response.Error);

                return response.Result.Offers != null && response.Result.Offers.Length > 0
                           ? MappingsFromService.ToBankProduct(response.Result.Offers[0])
                           : null;
            }
        }

        public void DeactivateProduct(string id)
        {
            using (var service = new BankConnectorClient())
            {
                var response = service.DisableOffer(id);

                AssertResponse(response.ResultCode, response.Error);
            }
        }

        private static void AssertResponse(int code, string message)
        {
            switch (code)
            {
                case 0:
                    return;
            }

            throw new BankProductsServiceException(code, message);
        }
    }
}
