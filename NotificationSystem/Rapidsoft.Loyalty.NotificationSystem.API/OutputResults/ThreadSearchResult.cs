namespace Rapidsoft.Loyalty.NotificationSystem.API.OutputResults
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class ThreadSearchResult
    {
        [DataMember]
        public Thread Thread
        {
            get;
            set;
        }

        [DataMember]
        public int[] MessageMatchIndexes
        {
            get;
            set;            
        }
    }
}