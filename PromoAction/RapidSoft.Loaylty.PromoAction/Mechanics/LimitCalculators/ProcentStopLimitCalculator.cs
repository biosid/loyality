namespace RapidSoft.Loaylty.PromoAction.Mechanics.LimitCalculators
{
    using System.Globalization;

    using RapidSoft.Loaylty.PromoAction.Mechanics.Converters;

    public class ProcentStopLimitCalculator : IStopLimitCalculator
    {
        private const string StopLimitSqlFormat = "({1} * {0} / 100)";
        private readonly decimal stopFactor;

        public ProcentStopLimitCalculator(decimal stopFactor)
        {
            this.stopFactor = stopFactor;
        }

        public decimal Calculate(decimal initalNumber)
        {
            var retVal = initalNumber * this.stopFactor / 100;
            return retVal;
        }

        public string GenerateSql(string numberAlias)
        {
            var retVal = string.Format(
                StopLimitSqlFormat, this.stopFactor.ToString(CultureInfo.InvariantCulture).TrimDecimal(), numberAlias);
            return retVal;
        }
    }
}
