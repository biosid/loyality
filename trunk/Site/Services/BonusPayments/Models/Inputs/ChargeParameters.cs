using System;
using FluentValidation;

namespace Vtb24.Site.Services.BonusPayments.Models.Inputs
{
    /// <summary>
    /// Информация о покупке, необходимая для БПШ.
    /// </summary>
    public class ChargeParameters
    {
        public string ClientId { get; set; }

        public string ChequeNumber { get; set; }

        public ChequeItem[] Items { get; set; }

        public decimal Sum { get; set; }

        public DateTime ChequeTime { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<ChargeParameters>
            {
                v=>v.RuleFor(m=>m.ClientId).NotEmpty().WithMessage("Не указан ClientId")
            }.ValidateAndThrow(this);
        }
    }
}
