namespace RapidSoft.Loaylty.PromoAction.Mechanics.LimitCalculators
{
    using System.Globalization;

    using RapidSoft.Loaylty.PromoAction.Mechanics.Converters;

    public class FixedStopLimitCalculator : IStopLimitCalculator
    {
        private readonly decimal stopFactor;

        public FixedStopLimitCalculator(decimal stopFactor)
        {
            this.stopFactor = stopFactor;
        }

        public decimal Calculate(decimal initalNumber)
        {
            return this.stopFactor;
        }

        public string GenerateSql(string numberAlias)
        {
            return this.stopFactor.ToString(CultureInfo.InvariantCulture).TrimDecimal();
        }
    }
}