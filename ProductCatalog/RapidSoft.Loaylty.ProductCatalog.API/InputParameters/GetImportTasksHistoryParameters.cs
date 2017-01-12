namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetImportTasksHistoryParameters
    {
        [DataMember]
        public int? CountToSkip { get; set; }

        [DataMember]
        public int? CountToTake { get; set; }

        [DataMember]
        public bool? CalcTotalCount { get; set; }

        [DataMember]
        public int? PartnerId { get; set; }

        [DataMember]
        public string UserId { get; set; }
    }
}