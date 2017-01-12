namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    public class VerifyResult : ResultBase
    {
        public bool Valid { get; set; }

        public static VerifyResult BuildFail(string error)
        {
            return new VerifyResult
                       {
                           ResultCode = (int)ResultCodes.UnknownError,
                           Success = false,
                           Valid = false,
                           ResultDescription = error
                       };
        }

        public static VerifyResult BuildSuccess(bool valid)
        {
            return new VerifyResult
                       {
                           ResultCode = (int)ResultCodes.Success,
                           Success = true,
                           Valid = valid,
                           ResultDescription = null
                       };
        }
    }
}