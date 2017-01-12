namespace RapidSoft.Loaylty.PromoAction.Api.InputParameters
{
    public class GetRuleHistoryParameters
    {
        public int? CountSkip { get; set; }

        public int? CountTake { get; set; }

        public long RuleId { get; set; }

        public bool? CalcTotalCount { get; set; }

        public string UserId { get; set; }
    }
}