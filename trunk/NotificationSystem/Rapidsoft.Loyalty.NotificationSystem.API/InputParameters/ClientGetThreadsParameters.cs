namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ClientGetThreadsParameters
    {
        [DataMember]
        public string ClientId
        {
            get;
            set;
        }

        [DataMember]
        public ReadFilters Filter
        {
            get;
            set;
        }

        [DataMember]
        public int CountToSkip { get; set; }

        [DataMember(IsRequired = true)]
        public int CountToTake { get; set; }
    }
}