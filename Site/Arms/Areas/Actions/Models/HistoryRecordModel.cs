using System;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models;

namespace Vtb24.Arms.Actions.Models
{
    public class HistoryRecordModel
    {
        public DateTime When { get; set; }

        public string Who { get; set; }

        public string What { get; set; }

        public static HistoryRecordModel Map(HistoryRecord original)
        {
            return new HistoryRecordModel
            {
                When = original.When,
                Who = original.Who,
                What = original.What
            };
        }
    }
}
