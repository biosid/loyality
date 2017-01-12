namespace RapidSoft.Loaylty.PromoAction.Api.InputParameters
{
    public class Approve
    {
        public long RuleId { get; set; }
        public bool IsApproved { get; set; }
        public string Reason { get; set; }
    }
}