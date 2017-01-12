namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SendEmailParameters
    {
        [DataMember]
        public string EmailTo { get; set; }

        [DataMember]
        public string EmailFrom { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public string Body { get; set; }
    }
}