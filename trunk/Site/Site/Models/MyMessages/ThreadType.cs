using Vtb24.Site.Services.ClientMessageService;

namespace Vtb24.Site.Models.MyMessages
{
    public enum ThreadType
    {
        Unknown = 0,

        Notification,
        Suggestion,
        Issue,
        OrderIncident
    }

    public static class ThreadTypeExtensions
    {
        public static ThreadType Map(this ThreadTypes original)
        {
            switch (original)
            {
                case ThreadTypes.Notification:
                    return ThreadType.Notification;
                case ThreadTypes.Suggestion:
                    return ThreadType.Suggestion;
                case ThreadTypes.Issue:
                    return ThreadType.Issue;
                case ThreadTypes.OrderIncident:
                    return ThreadType.OrderIncident;
            }

            return ThreadType.Unknown;
        }
    }
}
