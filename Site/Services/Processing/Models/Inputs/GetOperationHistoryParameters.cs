using System;
using FluentValidation;

namespace Vtb24.Site.Services.Processing.Models.Inputs
{
    public class GetOperationHistoryParameters
    {
        public string ClientId { get; set; } 

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<GetOperationHistoryParameters>
            {
                v=>v.RuleFor(m=>m.ClientId).NotEmpty().WithMessage("Не передан ClientId")
            }.ValidateAndThrow(this);
        }
    }
}