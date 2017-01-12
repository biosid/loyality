namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.PromoAction.Api.InputParameters;

    public class AssignClientAudienceResult : ResultBase
    {
        public ClientTargetAudienceRelationResult[] ClientTargetAudienceRelations { get; set; }

        public static AssignClientAudienceResult BuildSuccess(IEnumerable<ClientTargetAudienceRelationResult> assignResults)
        {
            var asArray = assignResults.ToArray();
            return new AssignClientAudienceResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           ResultDescription = null,
                           Success = false,
                           ClientTargetAudienceRelations = asArray,
                       };
        }
    }
}
