namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System.Runtime.Serialization;
    using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

    [DataContract]
    public class MarkMessagesAsNotifiedParameters
    {
        [DataMember]
        public ThreadMessagesToMarkAsNotified[] MessagesByThreadId { get; set; }
    }
}
