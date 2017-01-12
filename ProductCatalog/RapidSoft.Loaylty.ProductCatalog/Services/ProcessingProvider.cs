namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System.Linq;

    using API.OutputResults;

    using Configuration;

    using Interfaces;

    using Processing.ProcessingService;

    public class ProcessingProvider : IProcessingProvider
    {
        public decimal GetUserBalance(string userId)
        {
            var options = new GetAccountsByClientRequestType
            {
                ClientId = userId, LoyaltyProgramId = ConfigHelper.LoyaltyProgramId
            }; 

            var userCreditCard = WebClientCaller.CallService<ProcessingServiceClient, GetAccountsByClientResponseType>(service => service.GetAccountsByClient(options));   
            
            if (userCreditCard.StatusCode != ResultCodes.SUCCESS)
            {
                return 0;
            }

            var userAccount = userCreditCard.Accounts.FirstOrDefault(a => a.IsPrimary);

            return userAccount == null ? 0 : userAccount.AuthLimit;
        }
    }
}