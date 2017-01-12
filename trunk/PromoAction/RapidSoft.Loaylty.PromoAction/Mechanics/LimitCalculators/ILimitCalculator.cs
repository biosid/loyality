namespace RapidSoft.Loaylty.PromoAction.Mechanics.LimitCalculators
{
    public interface ILimitCalculator
    {
        decimal Calculate(decimal initalNumber, decimal result);

        string GenerateBaseSql(string numberAlias, string baseMultiplication, string baseAddition);

        string GenerateActionSql(string numberAlias, string baseMultiplication, string baseAddition, string actionMultiplication, string actionAddition);

        string GenerateLimitedSql(
            string numberAlias,
            string baseMultiplication,
            string baseAddition,
            string actionMultiplication,
            string actionAddition);
    }
}