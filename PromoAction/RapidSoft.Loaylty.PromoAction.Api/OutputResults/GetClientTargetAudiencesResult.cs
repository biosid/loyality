namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetClientTargetAudiencesResult : ResultBase
    {
        [DataMember]
        public IList<DTO.TargetAudience> ClientTargetAudiences
        {
            get;
            set;
        }

        public static GetClientTargetAudiencesResult BuildSuccess(IList<DTO.TargetAudience> targetAudiences)
        {
            return new GetClientTargetAudiencesResult
                       {
                           ClientTargetAudiences = targetAudiences,
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true
                       };
        }
    }
}