namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public interface IClientContextParameters
    {
        [DataMember]
        Dictionary<string, string> ClientContext { get; set; }
    }
}