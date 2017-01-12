namespace Rapidsoft.Loyalty.NotificationSystem.API.Entities
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Thread
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public ThreadTypes Type { get; set; }

        [DataMember]
        public ThreadClientTypes ClientType { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public bool IsClosed { get; set; }

        [DataMember]
        public string ClientFullName { get; set; }

        [DataMember]
        public string ClientEmail { get; set; }

        [DataMember]
        public ThreadMessage TopicMessage { get; set; }

        [DataMember]
        public bool IsAnswered { get; set; }

        [DataMember]
        public DateTime InsertedDate { get; set; }

        [DataMember]
        public int MessagesCount { get; set; }

        [DataMember]
        public int UnreadMessagesCount { get; set; }

        [DataMember]
        public DateTime FirstMessageTime { get; set; }

        [DataMember]
        public DateTime LastMessageTime { get; set; }

        [DataMember]
        public string FirstMessageBy { get; set; }

        [DataMember]
        public string LastMessageBy { get; set; }

        [DataMember]
        public DateTime? ShowSince { get; set; }

        [DataMember]
        public DateTime? ShowUntil { get; set; }

        [DataMember]
        public MessageTypes FirstMessageType { get; set; }

        [DataMember]
        public MessageTypes LastMessageType { get; set; }

        [DataMember]
        public string MetaData { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }
    }
}