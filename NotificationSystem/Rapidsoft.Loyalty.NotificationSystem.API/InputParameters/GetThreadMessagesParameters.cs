namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetThreadMessagesParameters
    {
        [DataMember]
        public Guid ThreadId
        {
            get;
            set;
        }

        [DataMember]
        public int CountToSkip
        {
            get;
            set;
        }

        [DataMember(IsRequired = true)]
        public int CountToTake
        {
            get;
            set;
        } 
    }
}