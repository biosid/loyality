namespace RapidSoft.Loaylty.PromoAction.Mechanics.LimitCalculators
{
    using System.Globalization;

    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Converters;

    public class FixedLimitCalculator : LimitCalculatorBase, ILimitCalculator
    {
        private const string LimitSqlFormat = "CASE WHEN ({0}) > ({2}) THEN ({0}) ELSE {1} END";

        private readonly ILog log = LogManager.GetLogger(typeof(FixedLimitCalculator));
        private readonly decimal limitFactor;
        private readonly IStopLimitCalculator stopLimitCalculator;

        public FixedLimitCalculator(decimal limitFactor, IStopLimitCalculator stopLimitCalculator)
        {
            stopLimitCalculator.ThrowIfNull("stopLimitCalculator");
            this.limitFactor = limitFactor;
            this.stopLimitCalculator = stopLimitCalculator;
        }

        public decimal Calculate(decimal initalNumber, decimal result)
        {
            var stopLimit = this.stopLimitCalculator.Calculate(initalNumber);

            if (result > stopLimit)
            {
                return result;
            }

            log.DebugFormat(
                "Вычисленное число {0} меньше граничного значения {1}, возвращаем лимитное значение: {2}",
                result,
                stopLimit,
                this.limitFactor);
            return this.limitFactor;
        }

        public string GenerateLimitedSql(
            string numberAlias,
            string baseMultiplication,
            string baseAddition,
            string actionMultiplication,
            string actionAddition)
        {
            var actionSql = this.GenerateActionSql(
                numberAlias, baseMultiplication, baseAddition, actionMultiplication, actionAddition);
            var retVal = string.Format(
                LimitSqlFormat,
                actionSql,
                this.limitFactor.ToString(CultureInfo.InvariantCulture).TrimDecimal(),
                this.stopLimitCalculator.GenerateSql(numberAlias));
            return retVal;
        }
    }
}