namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;

    [DataContract]
    public class GetRuleHistoryResult : ResultBase
    {
        [DataMember]
        public IList<RuleHistory> RuleHistories { get; set; }

        [DataMember]
        public int? TotalCount { get; set; }

        [DataMember]
        public int MaxCountToTake { get; set; }

        public static GetRuleHistoryResult BuildSuccess(IList<RuleHistory> ruleHistories, int? totalCount, int maxCountToTake)
        {
            return new GetRuleHistoryResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           ResultDescription = null,
                           RuleHistories = ruleHistories,
                           TotalCount = totalCount,
                           MaxCountToTake = maxCountToTake
                       };
        }

        public static new GetRuleHistoryResult BuildFail(int code, string message)
        {
            return new GetRuleHistoryResult { ResultCode = code, Success = false, ResultDescription = message };
        }
    }
}