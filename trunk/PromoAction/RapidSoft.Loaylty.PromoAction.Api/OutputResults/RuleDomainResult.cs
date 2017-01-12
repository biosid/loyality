namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    [DataContract]
    public class RuleDomainResult : ResultBase
    {
        [DataMember]
        public RuleDomain RuleDomain { get; set; }

        public static RuleDomainResult BuildSuccess(RuleDomain ruleDomain)
        {
            return new RuleDomainResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           ResultDescription = null,
                           RuleDomain = ruleDomain
                       };
        }
    }
}
