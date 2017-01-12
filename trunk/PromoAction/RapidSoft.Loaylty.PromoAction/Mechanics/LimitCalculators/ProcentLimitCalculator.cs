namespace RapidSoft.Loaylty.PromoAction.Mechanics.LimitCalculators
{
    using System.Globalization;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Converters;

    public class ProcentLimitCalculator : LimitCalculatorBase, ILimitCalculator
    {
        private const string LimitSqlFormat = "CASE WHEN ({0}) > {3} THEN ({0}) ELSE ({2} * {1} / 100) END";

        private readonly ILog log = LogManager.GetLogger(typeof(ProcentLimitCalculator));
        private readonly decimal limitFactor;
        private readonly IStopLimitCalculator stopLimitCalculator;

        public ProcentLimitCalculator(decimal limitFactor, IStopLimitCalculator stopLimitCalculator)
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

            var retVal = initalNumber * this.limitFactor / 100;

            log.DebugFormat(
                "Вычисленное число {0} меньше граничного значения {1}, возвращаем лимитное значение: {2}",
                result,
                stopLimit,
                retVal);
            return retVal;
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
                numberAlias,
                this.stopLimitCalculator.GenerateSql(numberAlias));
            return retVal;
        }
    }
}