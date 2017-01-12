namespace RapidSoft.GeoPoints.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public class CheckKladrCodeResult : ResultBase
    {
        [DataMember]
        public string[] ExistKladrCodes { get; set; }

        public static CheckKladrCodeResult BuildSuccess(IEnumerable<string> existKladrCodes)
        {
            var asArray = existKladrCodes.ToArray();
            return new CheckKladrCodeResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           ResultDescription = null,
                           ExistKladrCodes = asArray
                       };
        }
    }
}