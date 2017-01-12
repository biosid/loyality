using FluentValidation;

namespace Vtb24.Site.SecurityWebServices.Security.Models.Inputs
{
    public class DenyRegistrationRequestOptions
    {
        public string PhoneNumber { get; set; }

        public int RegistrationRequestBankStatus { get; set; }

        internal void ValidateAndThrow()
        {
            new InlineValidator<DenyRegistrationRequestOptions>
                {
                    v => v.RuleFor(m => m.PhoneNumber).NotEmpty().WithMessage("Не указан телефон"),
                    v => v.RuleFor(m => m.PhoneNumber).Matches(@"7\d{10}").WithMessage("Неверный формат телефона (11 цифр, первая -- 7)")
                }
                .ValidateAndThrow(this);
        }

    }
}