namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GenerateResult : ResultBase
    {
        [DataMember(EmitDefaultValue = true)]
        public string BaseSql { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string ActionSql { get; set; }

        /// <summary>
        /// Код факта применения правил
        /// </summary>
        [DataMember]
        public RuleApplyStatuses RuleApplyStatus { get; private set; }

        public static GenerateResult BuildFail(string description)
        {
            return new GenerateResult
                       {
                           ResultCode = ResultCodes.UNKNOWN_ERROR,
                           Success = false,
                           ResultDescription = description,
                           RuleApplyStatus = RuleApplyStatuses.RulesNotExecuted
                       };
        }

        public static GenerateResult BuildFail(ResultBase resultBase)
        {
            return new GenerateResult
                       {
                           ResultCode = resultBase.ResultCode,
                           Success = false,
                           ResultDescription = resultBase.ResultDescription,
                           RuleApplyStatus = RuleApplyStatuses.RulesNotExecuted
                       };
        }

        /// <summary>
        /// Статический конструктор результата генерации SQL.
        /// </summary>
        /// <returns>
        /// Результат генерации SQL.
        /// </returns>
        public static GenerateResult BuildSuccess(string baseSql, string actionSql)
        {
            return new GenerateResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           RuleApplyStatus = RuleApplyStatuses.RulesExecuted,
                           BaseSql = baseSql,
                           ActionSql = actionSql
                       };
        }
    }
}