namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults 
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Объект-результат вычисления значения.
    /// </summary>
    [DataContract]
    public class CalculateResult : ResultBase
    {
        /// <summary>
        /// Число, получившееся после применения всех базовых правил.
        /// Если никакие правила не применялись, то возвращается исходное число.
        /// </summary>
        [DataMember]
        public decimal BaseResult { get; set; }

        /// <summary>
        /// Окончательный результат. Число, получившееся после применения всех правил.
        /// Если никакие правила не применялись, то возвращается исходное число.
        /// </summary>
        [DataMember]
        public decimal PromoResult { get; set; }

        /// <summary>
        /// Код факта применения правил
        /// </summary>
        [DataMember]
        public RuleApplyStatuses RuleApplyStatus { get; set; }

        [DataMember]
        public string[] TraceMessages { get; set; }

        /// <summary>
        /// Статический конструктор объекта-результат с фактом применения правил "Не успешно".
        /// </summary>
        /// <param name="result">
        /// Вычислений результат механики.
        /// </param>
        /// <param name="description">
        /// Описание факта применения правил.
        /// </param>
        /// <returns>
        /// Созданный объект-результат.
        /// </returns>
        public static CalculateResult BuildFail(decimal result, string description)
        {
            return new CalculateResult
                       {
                           ResultCode = ResultCodes.UNKNOWN_ERROR,
                           Success = true,
                           RuleApplyStatus = RuleApplyStatuses.RulesNotExecuted, 
                           ResultDescription = description
                       };
        }

        /// <summary>
        /// Статический конструктор объекта-результат с фактом применения правил "Успешно".
        /// </summary>
        /// <param name="retBaseVal">
        ///     Вычислений результат механики.
        /// </param>
        /// <param name="promoResult"></param>
        /// <param name="isBaseApplied">
        ///     Признак применились ли базовые правила.
        /// </param>
        /// <param name="isNotBaseApplied">
        ///     Признак применились ли не базовые (акционные) правила.
        /// </param>
        /// <returns>
        /// Созданный объект-результат.
        /// </returns>
        public static CalculateResult BuildSuccess(decimal retBaseVal, decimal promoResult, bool isBaseApplied, bool isNotBaseApplied, string[] traceMessages)
        {
            RuleApplyStatuses code;

            if (isBaseApplied)
            {
                code = isNotBaseApplied ? RuleApplyStatuses.RulesExecuted : RuleApplyStatuses.BaseOnlyRulesExecuted;
            }
            else
            {
                code = isNotBaseApplied ? RuleApplyStatuses.RulesExecuted : RuleApplyStatuses.RulesNotExecuted;
            }

            return new CalculateResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           BaseResult = retBaseVal,
                           PromoResult = promoResult,
                           RuleApplyStatus = code,
                           ResultDescription = null,
                           TraceMessages = traceMessages
                       };
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Result: {0}, RuleApplyStatus: {1}, ResultDescription: {2}", this.PromoResult, this.RuleApplyStatus, this.ResultDescription);
        }
    }
}