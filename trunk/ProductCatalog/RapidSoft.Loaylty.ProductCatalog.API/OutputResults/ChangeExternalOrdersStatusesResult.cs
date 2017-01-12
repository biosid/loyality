namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ChangeExternalOrdersStatusesResult : ResultBase
    {
        [DataMember]
        public ChangeExternalOrderStatusResult[] ChangeExternalOrderStatusResults
        {
            get;
            set;
        }
    }
}