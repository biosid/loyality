namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System;
    using System.Runtime.Serialization;

    public class MarkThreadAsReadParameters
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
    }
}