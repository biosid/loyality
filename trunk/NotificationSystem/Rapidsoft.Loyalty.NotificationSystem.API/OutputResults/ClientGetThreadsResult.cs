namespace Rapidsoft.Loyalty.NotificationSystem.API.OutputResults
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class ClientGetThreadsResult : ResultBase
    {
        [DataMember]
        public int TotalCount
        {
            get;
            set;
        }

        [DataMember]
        public Thread[] Threads
        {
            get;
            set;
        }
    }
}