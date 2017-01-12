namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    public class SignatureResult : ResultBase
    {
        public string Signature { get; set; }

        public static SignatureResult BuildFail(string error)
        {
            return new SignatureResult
                       {
                           ResultCode = (int)ResultCodes.UnknownError,
                           Success = false,
                           Signature = null,
                           ResultDescription = error
                       };
        }

        public static SignatureResult BuildSuccess(string signature)
        {
            return new SignatureResult
                       {
                           ResultCode = (int)ResultCodes.Success,
                           Success = true,
                           Signature = signature,
                           ResultDescription = null
                       };
        }
    }
}