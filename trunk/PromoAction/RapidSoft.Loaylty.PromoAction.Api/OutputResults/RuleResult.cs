namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    [DataContract]
    public class RuleResult : ResultBase
    {
        [DataMember]
        public Rule Rule { get; set; }

        public static RuleResult BuildSuccess(Rule rule)
        {
            return new RuleResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           ResultDescription = null,
                           Rule = rule
                       };
        }

        public static new RuleResult BuildFail(int code, string message)
        {
            return new RuleResult { ResultCode = code, Success = false, ResultDescription = message };
        }
    }
}