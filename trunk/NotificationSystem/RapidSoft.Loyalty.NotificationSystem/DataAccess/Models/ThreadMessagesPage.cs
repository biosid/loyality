using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

namespace Rapidsoft.Loyalty.NotificationSystem.DataAccess.Models
{
    public class ThreadMessagesPage
    {
        public ThreadMessage[] ThreadMessages
        {
            get;
            set;
        }

        public int TotalCount
        {
            get;
            set;
        }
    }
}
