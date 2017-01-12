using System;
using System.Linq;
using Vtb24.Site.Services.BankProducts.Models;
using Vtb24.Site.Services.BankProducts.Models.Outputs;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.BankProducts.Stubs
{
    public class BankProductsServiceStub : IBankProductsService
    {
        private readonly BankProduct[] _products = new[]
        {
            new BankProduct
            {
                Id = "1234",
                Description = "Списание комиссии за годовое обслуживание по карте *7415",
                ExpirationDate = new DateTime(2015, 5, 4),
                ProductId = "SERVICE",
                Cost = 120000
            },
            new BankProduct
            {
                Id = "2345",
                Description = "Списание комиссии за годовое обслуживание по карте *8162",
                ExpirationDate = new DateTime(2015, 12, 12),
                ProductId = "SERVICE",
                Cost = 125500
            },
            new BankProduct
            {
                Id = "3456",
                Description = "Списание комиссии за покупку СМС-пакета по карте *7415",
                ExpirationDate = new DateTime(2015, 5, 4),
                ProductId = "SMS",
                Cost = 100
            },
            new BankProduct
            {
                Id = "4567",
                Description = "Списание комиссии за покупку СМС-пакета по карте *8162",
                ExpirationDate = new DateTime(2015, 12, 12),
                ProductId = "SMS",
                Cost = 120
            }
        };

        public BankProductsResult GetProducts(string clientId, DateTime minExpirationDate, PagingSettings pagingSettings)
        {
            return new BankProductsResult(_products, 1, pagingSettings);
            //return new BankProductsResult(Enumerable.Empty<BankProduct>(), 0, pagingSettings);
        }

        public BankProduct GetProduct(string id, string clientId, DateTime minExpirationDate)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void DeactivateProduct(string id)
        {
            throw new NotImplementedException();
        }
    }
}
