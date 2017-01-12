using System;

namespace Vtb24.Site.Models.MyPoints
{
    public class MyPointsModel
    {
        public OperationModel[] Operations { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string DateLabel { get; set; }

        public string NextMonthUrl { get; set; }

        public string PrevMonthUrl { get; set; }

        public bool ShowMonthFilter { get; set; }

        public decimal ClientBalance { get; set; }

        public decimal BonusTotal { get; set; }

        public int TotalPages { get; set; }

        public int Page { get; set; }
    }
}