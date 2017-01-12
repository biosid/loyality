namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    public class RecommendParameters
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
        public bool IsRecommended
        {
            get;
            set;
        }
    }
}
