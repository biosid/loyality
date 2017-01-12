namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    [DataContract]
    public class RuleDomainsResult : ResultBase
    {
        [DataMember]
        public IList<RuleDomain> RuleDomains { get; set; }

        public static RuleDomainsResult BuildSuccess(IList<RuleDomain> ruleDomains)
        {
            return new RuleDomainsResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           ResultDescription = null,
                           RuleDomains = ruleDomains
                       };
        }
    }
}