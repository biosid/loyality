namespace Vtb24.Site.Security.SecurityService.Models.Exceptions
{
    public class OtpDeliveryModeNotSupported : SecurityServiceException
    {
        public OtpDeliveryModeNotSupported() : base("Неподдерживаемый режимо отправки одноразового пароля")
        {
        }
    }
}
