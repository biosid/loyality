using System;
using Vtb24.Site.Services.BankProducts.Models;
using Vtb24.Site.Services.BankProducts.Models.Outputs;
using Vtb24.Site.Services.ClientService.Models;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Processing.Models;
using Vtb24.Site.Services.Profile.Models;

namespace Vtb24.Site.Services
{
    public interface IClientService
    {
        MechanicsContext GetMechanicsContext();

        decimal GetBalance();

        ProcessingOperationsHistoryResult GetOperationsHistory(DateTime from, DateTime to, PagingSettings paging);

        ClientProfile GetProfile();

        ClientStatus GetStatus();

        void SetEmail(string email);

        string GetEmail();

        void SetUserLocation(string kladrCode);

        UserLocation GetUserLocation();

        BankProductsResult GetBankProducts(PagingSettings pagingSettings);

        BankProduct GetBankProduct(string id);
    }
}