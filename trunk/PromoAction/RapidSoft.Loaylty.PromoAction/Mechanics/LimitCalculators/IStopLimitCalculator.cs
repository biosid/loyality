namespace RapidSoft.Loaylty.PromoAction.Mechanics.LimitCalculators
{
    public interface IStopLimitCalculator
    {
        decimal Calculate(decimal initalNumber);

        string GenerateSql(string numberAlias);
    }
}