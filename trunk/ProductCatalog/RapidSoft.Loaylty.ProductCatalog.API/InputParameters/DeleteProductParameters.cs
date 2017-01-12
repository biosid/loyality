namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    public class DeleteProductParameters
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
    }
}
