using FluentValidation;

namespace Vtb24.Site.Services.Profile.Models.Inputs
{
    public class GetProfileParameters
    {
        public string ClientId { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<GetProfileParameters>()
            {
                v=>v.RuleFor(m=>m.ClientId).NotEmpty().WithMessage("Не заполнен ClientId")
            }.ValidateAndThrow(this);
        }
    }
}