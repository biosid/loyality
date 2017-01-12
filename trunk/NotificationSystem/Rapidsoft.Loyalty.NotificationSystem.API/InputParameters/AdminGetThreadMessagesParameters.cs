namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class AdminGetThreadMessagesParameters : GetThreadMessagesParameters
    {
        [DataMember]
        public string UserId
        {
            get;
            set;
        }
    }
}
