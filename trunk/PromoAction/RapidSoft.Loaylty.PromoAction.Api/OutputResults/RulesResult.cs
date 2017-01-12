namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    [DataContract]
    public class RulesResult : ResultBase
    {
        [DataMember]
        public IList<Rule> Rules { get; set; }

        [DataMember]
        public int? TotalCount { get; set; }

        [DataMember]
        public int MaxCountToTake { get; set; }

        public static RulesResult BuildSuccess(IList<Rule> rules, int? totalCount, int maxCountToTake)
        {
            return new RulesResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           ResultDescription = null,
                           Rules = rules,
                           TotalCount = totalCount,
                           MaxCountToTake = maxCountToTake
                       };
        }
    }
}