using System;
using Vtb24.Site.Services.BankProducts.Models;
using Vtb24.Site.Services.BankProducts.Models.Outputs;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services
{
    public interface IBankProductsService
    {
        BankProductsResult GetProducts(string clientId, DateTime minExpirationDate, PagingSettings pagingSettings);

        BankProduct GetProduct(string id, string clientId, DateTime minExpirationDate);

        void DeactivateProduct(string id);
    }
}