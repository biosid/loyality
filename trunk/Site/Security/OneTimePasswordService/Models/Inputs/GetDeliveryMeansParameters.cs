using FluentValidation;

namespace Vtb24.Site.Security.OneTimePasswordService.Models.Inputs
{
    public class GetDeliveryMeansParameters
    {
        public string OtpToken { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<GetDeliveryMeansParameters>
            {
                v=>v.RuleFor(m=>m.OtpToken).NotEmpty().WithMessage("Не указан токен одноразового пароля (OtpToken)")
            }.ValidateAndThrow(this);
        }
    }
}
