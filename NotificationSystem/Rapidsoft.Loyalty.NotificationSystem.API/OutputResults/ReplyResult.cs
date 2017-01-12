namespace Rapidsoft.Loyalty.NotificationSystem.API.OutputResults
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class ReplyResult : ResultBase
    {
        [DataMember]
        public Thread Thread
        {
            get;
            set;
        }
    }
}