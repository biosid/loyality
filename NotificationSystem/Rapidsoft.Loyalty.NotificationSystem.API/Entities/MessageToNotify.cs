namespace Rapidsoft.Loyalty.NotificationSystem.API.Entities
{
    using System;

    public class MessageToNotify
    {
        public Guid Id { get; set; }

        public Guid ThreadId { get; set; }

        public int MessageIndex { get; set; }

        public DateTime MessageTime { get; set; }
    }
}
