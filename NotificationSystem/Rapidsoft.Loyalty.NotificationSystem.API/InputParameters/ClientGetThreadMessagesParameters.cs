namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System;
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class ClientGetThreadMessagesParameters : GetThreadMessagesParameters
    {
        [DataMember]
        public string ClientId
        {
            get;
            set;
        }
    }
}