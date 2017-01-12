namespace RapidSoft.Loaylty.PromoAction.Mechanics.LimitCalculators
{
    public abstract class LimitCalculatorBase
    {
        private const string BaseSqlFormat = "{0} * {1} + {2}";
        private const string ActionSqlFormat = "({0}) * {1} + {2}";

        public string GenerateBaseSql(string numberAlias, string baseMultiplication, string baseAddition)
        {
            var retVal = string.Format(BaseSqlFormat, numberAlias, baseMultiplication, baseAddition);
            return retVal;
        }

        public string GenerateActionSql(
            string numberAlias,
            string baseMultiplication,
            string baseAddition,
            string actionMultiplication,
            string actionAddition)
        {
            var baseSql = this.GenerateBaseSql(numberAlias, baseMultiplication, baseAddition);
            var retVal = string.Format(ActionSqlFormat, baseSql, actionMultiplication, actionAddition);
            return retVal;
        }
    }
}