using FluentValidation;

namespace Vtb24.Site.SecurityWebServices.PublicProfile.Inputs
{
    public class GetPublicProfileParameters
    {
        public string SecurityToken { get; set; }

        public string ShopId { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<GetPublicProfileParameters>
            {
                v=>v.RuleFor(m=>m.SecurityToken).NotEmpty().WithMessage("Не задан токен (SecurityToken)"),
                v=>v.RuleFor(m=>m.ShopId).NotEmpty().WithMessage("Не задан идентификатор партнера (ShopId)")
            }.ValidateAndThrow(this);
        }
    }
}