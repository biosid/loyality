using FluentValidation;

namespace Vtb24.Site.Security.SecurityService.Models.Inputs
{
    public class ChangeUserPhoneNumberOptions
    {
        public int UserId { get; set; }

        public string NewPhoneNumber { get; set; }

        public string ChangedBy { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<ChangeUserPhoneNumberOptions>
            {
                v=>v.RuleFor(m=>m.NewPhoneNumber).NotEmpty().WithMessage("Не передан новый телефон"),
                v=>v.RuleFor(m=>m.NewPhoneNumber).Matches(@"7\d{10}").WithMessage("Неверный формат телефона (11 цифр, первая -- 7)"),
            }.ValidateAndThrow(this);
        }
    }
}