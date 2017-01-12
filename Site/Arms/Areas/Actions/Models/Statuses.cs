using System.ComponentModel;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models;

namespace Vtb24.Arms.Actions.Models
{
    public enum Statuses
    {
        // ReSharper disable InconsistentNaming

        [Description("-")]
        unknown,

        [Description("Создана")]
        not_active,

        [Description("Активна")]
        active

        // ReSharper restore InconsistentNaming
    }

    public static class StatusesExtensions
    {
        public static ActionStatus? Map(this Statuses? original)
        {
            if (!original.HasValue)
                return null;

            switch (original.Value)
            {
                case Statuses.active:
                    return ActionStatus.Active;
                case Statuses.not_active:
                    return ActionStatus.NotActive;
            }

            return null;
        }

        public static Statuses Map(this ActionStatus original)
        {
            switch (original)
            {
                case ActionStatus.Active:
                    return Statuses.active;
                case ActionStatus.NotActive:
                    return Statuses.not_active;
            }

            return Statuses.unknown;
        }

        public static ActionStatus Map(this Statuses original)
        {
            switch (original)
            {
                case Statuses.active:
                    return ActionStatus.Active;
                case Statuses.not_active:
                    return ActionStatus.NotActive;
            }

            return ActionStatus.Unknown;
        }
    }
}
