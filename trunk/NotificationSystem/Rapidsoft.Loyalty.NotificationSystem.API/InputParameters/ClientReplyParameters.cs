namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System;
    using System.Runtime.Serialization;

    using Entities;

    public class ClientReplyParameters
    {
        [DataMember]
        public Guid ThreadId
        {
            get;
            set;
        }

        [DataMember]
        public string ClientId
        {
            get;
            set;
        }

        [DataMember]
        public string ClientFullName
        {
            get;
            set;
        }

        [DataMember]
        public string ClientEmail
        {
            get;
            set;
        }

        [DataMember]
        public string MessageBody
        {
            get;
            set;
        }
        
        [DataMember]
        public MessageAttachment[] Attachments
        {
            get;
            set;
        }
    }
}