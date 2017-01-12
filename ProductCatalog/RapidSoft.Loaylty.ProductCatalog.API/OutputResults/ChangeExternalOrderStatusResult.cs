namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ChangeExternalOrderStatusResult : ChangeOrderStatusResult
    {
        [DataMember]
        public string ExternalOrderId { get; set; }

        public new static ChangeExternalOrderStatusResult BuildFail(int code, string description)
        {
            return new ChangeExternalOrderStatusResult
                   {
                       ResultCode = code,
                       ResultDescription = description,
                       Success = false
                   };
        }
    }
}