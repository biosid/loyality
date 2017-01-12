namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    public interface IPagingParameters
    {
        [DataMember]
        int? CountToTake { get; set; }

        [DataMember]
        int? CountToSkip { get; set; }

        [DataMember]
        bool? CalcTotalCount { get; set; }
    }
}