using FluentValidation;

namespace Vtb24.Site.SecurityWebServices.Security.Models.Inputs
{
    public class ChangePhoneNumberOptions
    {
        public string Login { get; set; }

        public string NewPhoneNumber { get; set; }

        public string ChangedBy { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<ChangePhoneNumberOptions>
            {
                v=>v.RuleFor(m=>m.Login).NotEmpty().WithMessage("Не передан логин"),
                v=>v.RuleFor(m=>m.Login).Matches(@"7\d{10}").WithMessage("Неверный формат логина (11 цифр, первая -- 7)"),
                v=>v.RuleFor(m=>m.NewPhoneNumber).NotEmpty().WithMessage("Не передан новый телефон"),
                v=>v.RuleFor(m=>m.NewPhoneNumber).Matches(@"7\d{10}").WithMessage("Неверный формат телефона (11 цифр, первая -- 7)"),
            }.ValidateAndThrow(this);
        }
    }
}