using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

namespace Rapidsoft.Loyalty.NotificationSystem.API.OutputResults
{
    using System.Runtime.Serialization;

    [DataContract]
    public class NotifyClientsResult : ResultBase
    {
        [DataMember]
        public Thread[] Threads { get; set; }
    }
}
