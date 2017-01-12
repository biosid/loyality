using FluentValidation;

namespace Vtb24.Site.Services.Profile.Models.Inputs
{
    public class SetLocationParameters
    {
        public string ClientId { get; set; }

        public string LocationKladr { get; set; }

        public string LocationTitle { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<SetLocationParameters>
            {
                v=>v.RuleFor(m=>m.ClientId).NotEmpty().WithMessage("Не указан ClientId"),
                v=>v.RuleFor(m=>m.LocationKladr).NotEmpty().WithMessage("Не указан КЛАДР код (LocationKladr)"),
                v=>v.RuleFor(m=>m.LocationKladr).Length(13).WithMessage("Неверный формат КЛАДР кода"),
                v=>v.RuleFor(m=>m.LocationTitle).NotEmpty().WithMessage("Не указано имя города присутствия (LocationTitle)")
            }.ValidateAndThrow(this);
        }
    }
}