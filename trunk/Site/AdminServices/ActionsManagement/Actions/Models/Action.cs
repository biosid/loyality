using System;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models
{
    public class Action
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public int Priority { get; set; }

        public bool IsExclusive { get; set; }

        public bool IsNotExcludedBy { get; set; }

        public ActionStatus Status { get; set; }

        public ActionApproveStatus ApproveStatus { get; set; }

        public string ApproveMessage { get; set; }

        public Predicate Predicate { get; set; }

        public long MechanicId { get; set; }

        public ActionType Type { get; set; }

        public decimal Factor { get; set; }

        public ConditionalFactor[] ConditionalFactors { get; set; }

        public DateTime Updated { get; set; }

        public string UpdatedBy { get; set; }
    }
}
