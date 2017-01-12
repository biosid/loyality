namespace Rapidsoft.Loyalty.NotificationSystem.API.OutputResults
{
    using System.Runtime.Serialization;

    using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

    [DataContract]
    public class GetMessagesToNotifyResult : ResultBase
    {
        [DataMember]
        public ThreadMessagesToNotify[] MessagesByThreadId { get; set; }
    }
}
