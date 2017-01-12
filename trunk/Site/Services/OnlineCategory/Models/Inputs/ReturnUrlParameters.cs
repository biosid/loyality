using FluentValidation;

namespace Vtb24.Site.Services.OnlineCategory.Models.Inputs
{
    public class ReturnUrlParameters
    {
        public string UserTicket { get; set; }

        public string ExternalOrderId { get; set; }

        public decimal Total { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<ReturnUrlParameters>()
            {
                v=>v.RuleFor(m=>m.UserTicket).NotEmpty().WithMessage("Не заполнен UserTicket"),
                v=>v.RuleFor(m=>m.ExternalOrderId).NotEmpty().WithMessage("Не заполнен Id заказа (ExternalOrderId)")
            }.ValidateAndThrow(this);
        }
    }
}