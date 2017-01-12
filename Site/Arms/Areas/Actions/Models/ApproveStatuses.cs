using System.ComponentModel;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models;

namespace Vtb24.Arms.Actions.Models
{
    public enum ApproveStatuses
    {
        // ReSharper disable InconsistentNaming

        [Description("-")]
        unknown,

        [Description("Не утверждена")]
        not_approved,

        [Description("Утверждена")]
        approved,

        [Description("Отклонена")]
        correction

        // ReSharper restore InconsistentNaming
    }

    public static class ApproveStatusExtensions
    {
        public static ApproveStatuses Map(this ActionApproveStatus original)
        {
            switch (original)
            {
                case ActionApproveStatus.NotApproved:
                    return ApproveStatuses.not_approved;
                case ActionApproveStatus.Approved:
                    return ApproveStatuses.approved;
                case ActionApproveStatus.Correction:
                    return ApproveStatuses.correction;
            }

            return ApproveStatuses.unknown;
        }
    }
}
