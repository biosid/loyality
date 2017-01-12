namespace Vtb24.Site.Security.SecurityService.Models.Exceptions
{
    public class OtpViaSmsLimitReached : SecurityServiceException
    {
        public OtpViaSmsLimitReached() : base("Достигнут предел отправки одноразового пароля по СМС")
        {
        }
    }
}
