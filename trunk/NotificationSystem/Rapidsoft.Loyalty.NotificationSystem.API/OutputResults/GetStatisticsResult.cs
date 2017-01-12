namespace Rapidsoft.Loyalty.NotificationSystem.API.OutputResults
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetStatisticsResult : ResultBase
    {
        [DataMember]
        public int ThreadsCount
        {
            get;
            set;
        }

        [DataMember]
        public int UnreadThreadsCount
        {
            get;
            set;
        }
    }
}