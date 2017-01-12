namespace Rapidsoft.Loyalty.NotificationSystem.API.Entities
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ThreadMessagesToMarkAsNotified
    {
        [DataMember]
        public Guid ThreadId { get; set; }

        [DataMember]
        public int LastMessageIndex { get; set; }
    }
}
