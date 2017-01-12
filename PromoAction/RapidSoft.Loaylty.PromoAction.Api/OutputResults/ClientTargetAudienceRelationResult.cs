using System.Runtime.Serialization;
using RapidSoft.Loaylty.PromoAction.Api.OutputResults;

namespace RapidSoft.Loaylty.PromoAction.Api.InputParameters
{
    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    [DataContract]
    public class ClientTargetAudienceRelationResult
    {
        [DataMember]
        public string ClientId;

		[DataMember]
        public string PromoActionId;

		[DataMember]
	    public int AssignResultCode;

		[DataMember]
		public string ResultDescription;

        public static ClientTargetAudienceRelationResult BuildSuccess(ClientTargetAudienceRelation relation)
        {
            const int AsInt = (int)AssignClientTargetAudienceCodes.Success;
            return new ClientTargetAudienceRelationResult
                       {
                           AssignResultCode = AsInt,
                           ClientId = relation.ClientId,
                           PromoActionId = relation.PromoActionId,
                           ResultDescription = null
                       };
        }

        public static ClientTargetAudienceRelationResult BuildNotFound(ClientTargetAudienceRelation relation, string description)
        {
            const int AsInt = (int)AssignClientTargetAudienceCodes.TargetAudienceNotFound;
            return new ClientTargetAudienceRelationResult
                       {
                           AssignResultCode = AsInt,
                           ClientId = relation.ClientId,
                           PromoActionId = relation.PromoActionId,
                           ResultDescription = description
                       };
        }
    }
}
