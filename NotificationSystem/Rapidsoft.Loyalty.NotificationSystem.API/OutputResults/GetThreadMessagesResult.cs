namespace Rapidsoft.Loyalty.NotificationSystem.API.OutputResults
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class GetThreadMessagesResult : ResultBase
    {
        [DataMember]
        public int TotalCount
        {
            get;
            set;
        }

        [DataMember]
        public Thread Thread
        {
            get;
            set;
        }

        [DataMember]
        public ThreadMessage[] ThreadMessages
        {
            get;
            set;
        }
    }
}