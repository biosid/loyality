namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class ChangeModerationStatusParameters
    {
        [DataMember]
        public string UserId
        {
            get;
            set;
        }

        [DataMember]
        public string[] ProductIds
        {
            get;
            set;
        }

        [DataMember]
        public ProductModerationStatuses ProductModerationStatus
        {
            get;
            set;
        }
    }
}
