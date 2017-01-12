namespace Rapidsoft.Loyalty.NotificationSystem.API.OutputResults
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class MarkThreadAsReadResult : ResultBase
    {
        [DataMember]
        public Thread Thread
        {
            get;
            set;
        }
    }
}