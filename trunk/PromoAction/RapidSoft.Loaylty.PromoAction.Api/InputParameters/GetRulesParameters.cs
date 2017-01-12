namespace RapidSoft.Loaylty.PromoAction.Api.InputParameters
{
    using System;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    public class GetRulesParameters
    {
        public int? CountSkip { get; set; }

        public int? CountTake { get; set; }

        public DateTime? DateTimeFrom { get; set; }

        public DateTime? DateTimeTo { get; set; }

        public string SearchTerm { get; set; }

        public RuleStatuses? Status { get; set; }

        public long? RuleDomainId { get; set; }

        public RuleTypes? Type { get; set; }

        public SortProperty? SortProperty { get; set; }

        public ApproveStatus[] ApproveStatuses { get; set; }

        /// <summary>
        /// Направление сортировки
        /// </summary>
        public SortDirections? SortDirect { get; set; }

        public bool? CalcTotalCount { get; set; }

        /// <summary>
        /// Если он не null, то нужно возвращать только акции, которые уже завершены к этой дате
        /// </summary>
        public DateTime? DateTimeUntil { get; set; }

        public string UserId { get; set; }
    }
}