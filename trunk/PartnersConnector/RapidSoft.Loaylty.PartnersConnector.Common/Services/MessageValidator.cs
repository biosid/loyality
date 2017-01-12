namespace RapidSoft.Loaylty.PartnersConnector.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using PartnersConnector.Services;

    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;

    public class MessageValidator
    {
        public static void Validate(object messageObject)
        {
            ValidationContext context = new ValidationContext(messageObject, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(messageObject, context, results, true))
            {
                var description = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage).ToArray());
                throw new OperationException((int)ResultCodes.InvalidPartnerResponse, description);
            }
        }
    }
}