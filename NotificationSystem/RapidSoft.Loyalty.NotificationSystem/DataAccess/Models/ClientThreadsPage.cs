using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

namespace Rapidsoft.Loyalty.NotificationSystem.DataAccess.Models
{
    public class ClientThreadsPage
    {
        public Thread[] Threads
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
