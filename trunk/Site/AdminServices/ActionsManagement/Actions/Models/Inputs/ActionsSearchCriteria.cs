using System;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Inputs
{
    public class ActionsSearchCriteria
    {
        public long MechanicId { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public DateTime? Until { get; set; }

        public string Term { get; set; }

        public ActionStatus? Status { get; set; }

        public ActionType? Type { get; set; }

        public ActionsSort Sort { get; set; }

        public ActionsSortDirection SortDirection { get; set; }
    }
}
