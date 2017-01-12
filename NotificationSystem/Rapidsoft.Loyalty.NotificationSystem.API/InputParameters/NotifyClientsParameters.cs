namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class NotifyClientsParameters
    {
        [DataMember]
        public Notification[] Notifications { get; set; }
    }
}
