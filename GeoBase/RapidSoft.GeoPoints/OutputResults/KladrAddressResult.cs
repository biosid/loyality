namespace RapidSoft.GeoPoints.OutputResults
{
    using System.Runtime.Serialization;

    using RapidSoft.Kladr.Model;

    [DataContract]
    public class KladrAddressResult : ResultBase
    {
        [DataMember]
        public KladrAddress KladrAddress { get; set; }

        public static KladrAddressResult BuildSuccess(KladrAddress address)
        {
            return new KladrAddressResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           ResultDescription = null,
                           KladrAddress = address
                       };
        }
    }
}