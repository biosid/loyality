namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    public class FactorsResult : ResultBase
    {
        public decimal BaseMultiplicationFactor { get; set; }

        public decimal BaseAdditionFactor { get; set; }

        public decimal MultiplicationFactor { get; set; }

        public decimal AdditionFactor { get; set; }

        public bool IsBaseApplied { get; set; }

        public bool IsNotBaseApplied { get; set; }

        public string[] TraceMessages { get; set; }

        /// <summary>
        /// Статический конструктор объекта-результат с фактом применения правил "Не успешно".
        /// </summary>
        /// <param name="description">
        /// Описание факта применения правил.
        /// </param>
        /// <returns>
        /// Созданный объект-результат.
        /// </returns>
        public static FactorsResult BuildFail(string description)
        {
            return new FactorsResult
            {
                ResultCode = ResultCodes.UNKNOWN_ERROR,
                Success = true,
                ResultDescription = description
            };
        }

        public static FactorsResult BuildSuccess(
            decimal baseMultiplicationFactor,
            decimal baseAdditionFactor,
            decimal multiplicationFactor,
            decimal additionFactor,
            bool isBaseApplied,
            bool isNotBaseApplied,
            string[] traceMessages)
        {
            return new FactorsResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           BaseMultiplicationFactor = baseMultiplicationFactor,
                           BaseAdditionFactor = baseAdditionFactor,
                           MultiplicationFactor = multiplicationFactor,
                           AdditionFactor = additionFactor,
                           IsBaseApplied = isBaseApplied,
                           IsNotBaseApplied = isNotBaseApplied,
                           ResultDescription = null,
                           TraceMessages = traceMessages
                       };
        }
    }
}