namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System;
    using System.Runtime.Serialization;

    using Entities;

    public class ChangeAnsweredStatusParameters
    {
        [DataMember]
        public string UserId
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
        public bool IsAnswered
        {
            get;
            set;
        }
    }
}