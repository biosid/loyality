namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetStatisticsParameters
    {
        [DataMember]
        public string ClientId
        {
            get;
            set;
        } 
    }
}