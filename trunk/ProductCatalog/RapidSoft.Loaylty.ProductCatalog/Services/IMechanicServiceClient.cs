namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System.Collections.Generic;
    using System.ServiceModel;

    using PromoAction.WsClients.MechanicsService;

    public interface IMechanicServiceClient
    {
        GenerateResult GenerateSql(GenerateSqlParameters param);

        CalculateResult CalculateSingleValue(string ruleDomainName, decimal initialNumber, Dictionary<string, string> context);

        FactorsResult CalculateFactors(string ruleDomainName, Dictionary<string, string> context);
    }
}