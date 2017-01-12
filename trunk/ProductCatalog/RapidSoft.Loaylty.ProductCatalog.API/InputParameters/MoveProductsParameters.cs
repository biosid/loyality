namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class MoveProductsParameters
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
        public int TargetCategoryId
        {
            get;
            set;
        }
    }
}
