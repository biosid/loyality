namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System.Collections.Generic;
    using System.ServiceModel;

    using PromoAction.WsClients.MechanicsService;

    public class MechanicServiceClientAdapter : IMechanicServiceClient
    {
        public GenerateResult GenerateSql(GenerateSqlParameters param)
        {
            using (var mechanicsServiceClient = new MechanicsServiceClient())
            {
                return mechanicsServiceClient.GenerateSql(param);
            }
        }

        public CalculateResult CalculateSingleValue(string ruleDomainName, decimal initialNumber, Dictionary<string, string> context)
        {
            using (var mechanicsServiceClient = new MechanicsServiceClient())
            {
                return mechanicsServiceClient.CalculateSingleValue(ruleDomainName, initialNumber, context);
            }
        }

        public FactorsResult CalculateFactors(string ruleDomainName, Dictionary<string, string> context)
        {
            using (var mechanicsServiceClient = new MechanicsServiceClient())
            {
                return mechanicsServiceClient.CalculateFactors(ruleDomainName, context);
            }
        }
    }
}