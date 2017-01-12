namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System;
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class SendFeedbackParameters
    {
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
        public string ClientId
        {
            get;
            set;
        }

        [DataMember]
        public string MessageTitle
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

        [DataMember]
        public FeedbackTypes Type
        {
            get;
            set;
        }

        [DataMember]
        public string MetaData
        {
            get;
            set;
        }
    }
}