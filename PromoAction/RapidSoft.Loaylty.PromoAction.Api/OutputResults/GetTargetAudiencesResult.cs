namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetTargetAudiencesResult : ResultBase
    {
        [DataMember]
        public IList<DTO.TargetAudience> TargetAudiences { get; set; }

        public static GetTargetAudiencesResult BuildSuccess(IList<DTO.TargetAudience> targetAudiences)
        {
            return new GetTargetAudiencesResult
                       {
                           TargetAudiences = targetAudiences,
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true
                       };
        }
    }
}