using System;
using System.Collections.Generic;
using Action = Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Action;

namespace Vtb24.Arms.Actions.Models
{
    public class ActionModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; }

        public string Traits { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public DateTime Updated { get; set; }

        public string UpdatedBy { get; set; }

        public Statuses Status { get; set; }

        public ApproveStatuses ApproveStatus { get; set; }

        public static ActionModel Map(Action original)
        {
            return new ActionModel
            {
                Id = original.Id,
                Name = original.Name,
                Priority = original.Priority,
                Traits = string.Join(", ", GetTraits(original)),
                From = original.From,
                To = original.To,
                Updated = original.Updated,
                UpdatedBy = original.UpdatedBy,
                Status = original.Status.Map(),
                ApproveStatus = original.ApproveStatus.Map()
            };
        }

        private static IEnumerable<string> GetTraits(Action action)
        {
            if (action.IsExclusive)
                yield return "Исключающая";
            if (action.IsNotExcludedBy)
                yield return "Неисключаемая";
        }
    }
}
