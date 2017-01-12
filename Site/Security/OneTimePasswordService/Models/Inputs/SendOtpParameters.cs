using FluentValidation;

namespace Vtb24.Site.Security.OneTimePasswordService.Models.Inputs
{
    public class SendOtpParameters
    {
        public string OtpType { get; set; }

        public string To { get; set; }

        public string ExternalId { get; set; }

        public string MessageTemplate { get; set; }

        public OtpDeliveryMeans DeliveryMeans { get; set; }

        public bool IsFake { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<SendOtpParameters>
            {
                v=>v.RuleFor(m=>m.OtpType).NotEmpty().WithMessage("Не указан тип одноразового пароля (OtpType)"),
                v=>v.RuleFor(m=>m.To).NotEmpty().WithMessage("Не указан адресат (To)"),
                v=>v.RuleFor(m=>m.MessageTemplate).NotEmpty().WithMessage("Не задан шаблон сообщения"),
                v=>v.RuleFor(m=>m.MessageTemplate).Matches(@"\{0\}")
                    .WithMessage("Неверный формат шаблона сообщения. Шаблон должен содержать токен \"{0}\" для подстановки одноразового пароля")
            }.ValidateAndThrow(this);
        }
    }
}