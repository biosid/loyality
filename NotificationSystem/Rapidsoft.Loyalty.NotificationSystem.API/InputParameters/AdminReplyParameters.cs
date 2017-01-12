namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System;
    using System.Runtime.Serialization;

    using Entities;

    public class AdminReplyParameters
    {
        [DataMember]
        public string UserId
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
        public Guid ThreadId
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