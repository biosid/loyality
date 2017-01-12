using Vtb24.Site.Services.ClientMessageService;

namespace Vtb24.Arms.Security.Models.Users
{
    public enum UserMessageType
    {
        Unknown = 0,

        Notification,
        Suggestion,
        Issue,
        OrderIncident
    }

    internal static class UserMessageTypeExtensions
    {
        public static UserMessageType Map(this ThreadTypes original)
        {
            switch (original)
            {
                case ThreadTypes.Notification:
                    return UserMessageType.Notification;
                case ThreadTypes.Suggestion:
                    return UserMessageType.Suggestion;
                case ThreadTypes.Issue:
                    return UserMessageType.Issue;
               case ThreadTypes.OrderIncident:
                    return UserMessageType.OrderIncident;
            }

            return UserMessageType.Unknown;
        }
    }
}
