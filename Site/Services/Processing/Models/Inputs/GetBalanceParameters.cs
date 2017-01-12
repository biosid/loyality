using FluentValidation;

namespace Vtb24.Site.Services.Processing.Models.Inputs
{
    public class GetBalanceParameters
    {
        public string ClientId { get; set; }
 
        public void ValidateAndThrow()
        {
            new InlineValidator<GetBalanceParameters>
            {
                v=>v.RuleFor(m=>m.ClientId).NotEmpty().WithMessage("Не передан ClientId")
            }.ValidateAndThrow(this);
        }
    }
}