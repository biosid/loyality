namespace RapidSoft.Loaylty.PromoAction.Api.InputParameters
{
    public class AssignClientTargetAudienceParameters
    {
        public ClientTargetAudienceRelation[] ClientAudienceRelations
        {
            get; set;
        }

        public string UserId { get; set; }
    }
}
