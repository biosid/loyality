namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Объект-результат генерации SQL.
    /// </summary>
    [DataContract]
    public class GenerateDetailedResult : ResultBase
    {
        /// <summary>
        /// SQL Выражение для мультипликативного базового правила.
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string BaseMultiplicationSql { get; private set; }

        /// <summary>
        /// SQL Выражение для аддитивного базового правила.
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string BaseAdditionSql { get; private set; }

        /// <summary>
        /// SQL Выражение для мультипликативных правил.
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string MultiplicationSql { get; private set; }

        /// <summary>
        /// SQL Выражение для аддитивных правил.
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string AdditionSql { get; private set; }

        /// <summary>
        /// Код факта применения правил
        /// </summary>
        [DataMember]
        public RuleApplyStatuses RuleApplyStatus { get; private set; }

        [DataMember]
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
        public static GenerateDetailedResult BuildFail(string description)
        {
            return new GenerateDetailedResult
                       {
                           ResultCode = ResultCodes.UNKNOWN_ERROR,
                           Success = false,
                           ResultDescription = description,
                           RuleApplyStatus = RuleApplyStatuses.RulesNotExecuted
                       };
        }

        /// <summary>
        /// Статический конструктор результата генерации SQL.
        /// </summary>
        /// <param name="baseMultiplicationSql">
        /// Выражение для базовых мульпликативных правил.
        /// </param>
        /// <param name="baseAdditionSql">
        /// Выражение для базовых аддитивных правил.
        /// </param>
        /// <param name="multiplicationSql">
        /// Выражение для не базовых мульпликативных правил.
        /// </param>
        /// <param name="additionSql">
        /// Выражение для не базовых аддитивных правил.
        /// </param>
        /// <returns>
        /// Результат генерации SQL.
        /// </returns>
        public static GenerateDetailedResult BuildSuccess(
            string baseMultiplicationSql, 
            string baseAdditionSql, 
            string multiplicationSql, 
            string additionSql, 
            string[] traceMessages)
        {
            return new GenerateDetailedResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           RuleApplyStatus = RuleApplyStatuses.RulesExecuted,
                           BaseMultiplicationSql = baseMultiplicationSql,
                           BaseAdditionSql = baseAdditionSql,
                           MultiplicationSql = multiplicationSql,
                           AdditionSql = additionSql,
                           TraceMessages = traceMessages
                       };
        }
    }
}