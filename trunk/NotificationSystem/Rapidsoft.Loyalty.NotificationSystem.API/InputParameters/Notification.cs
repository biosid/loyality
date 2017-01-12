namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Notification
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public DateTime? ShowSince
        {
            get;
            set;
        }

        [DataMember]
        public DateTime? ShowUntil
        {
            get;
            set;
        }
    }
}