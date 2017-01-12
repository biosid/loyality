using System.Linq;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Processing.Models;
using Vtb24.Site.Services.Processing.Models.Exceptions;
using Vtb24.Site.Services.Processing.Models.Inputs;
using Vtb24.Site.Services.ProcessingService;

namespace Vtb24.Site.Services.Processing
{
    public class Processing : IProcessing
    {
        private static int LoyaltyProgramId
        {
            get { return AppSettingsHelper.Int("points_loyalty_program_id", 0); }
        }

        public decimal GetBalance(GetBalanceParameters parameters)
        {
            parameters.ValidateAndThrow();

            var account = GetPrimaryBonusAccount(parameters.ClientId);
            return account.AuthLimit; // AuthLimit = Balance - Holds
        }

        public ProcessingOperationsHistoryResult GetOperationsHistory(GetOperationHistoryParameters parameters, PagingSettings paging)
        {

            parameters.ValidateAndThrow();

            using (var serviceClient = new ProcessingServiceClient())
            {
                var account = GetPrimaryBonusAccount(parameters.ClientId);
                var options = new GetOperationHistoryRequestType
                {
                    ClientId = parameters.ClientId,
                    AccountId = account.AccountId,
                    RowCount = paging.Take,
                    SkipCount = paging.Skip,
                    FromDate = parameters.From,
                    ToDate = parameters.To,

                    LoyaltyProgramId = LoyaltyProgramId
                };
                var response = serviceClient.GetOperationHistory(options);

                AssertResponse(response.StatusCode, response.Error);

                var operations = response.OperationHistory.MaybeSelect(MappingsFromService.ToProcessingOperationInfo).MaybeToArray();
                var total = response.TotalCount ?? response.OperationHistory.Length;
                var outcome = total > 0 ? response.AccountsTotal.First().TotalWithdraw : 0;
                var income = total > 0 ? response.AccountsTotal.First().TotalDeposit : 0;
                var result = new ProcessingOperationsHistoryResult(operations, outcome, income, total, paging);
                return result;
            }

        }

        private static GetAccountsByClientResponseTypeAccountInfo GetPrimaryBonusAccount(string clientId)
        {
            using (var service = new ProcessingServiceClient())
            {
                var options = new GetAccountsByClientRequestType
                {
                    ClientId = clientId, LoyaltyProgramId = LoyaltyProgramId
                };
                var response = service.GetAccountsByClient(options);

                AssertResponse(response.StatusCode, response.Error);

                return response.Accounts.First(a=>a.IsPrimary);
            }
        }

        private static void AssertResponse(int code, string message)
        {
            if (code == 0)
            {
                return;
            }

            throw new ProcessingException(code, message);
        }
    }
}