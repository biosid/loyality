namespace Rapidsoft.Loyalty.NotificationSystem.API.OutputResults
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class AdminGetThreadsResult : ResultBase
    {
        [DataMember]
        public int TotalCount
        {
            get;
            set;
        }

        [DataMember]
        public ThreadSearchResult[] Result
        {
            get;
            set;
        }
    }
}