namespace RapidSoft.Loaylty.PartnersConnector.Common.Services.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;

    public class OfflineOrderStatusAttribute : ValidationAttribute
    {
        private static readonly int[] AllowedStatuses = new[]
        {
            (int)OrderStatuses.CancelledByPartner,
            (int)OrderStatuses.DeliveryWaiting,
            (int)OrderStatuses.Delivery,
            (int)OrderStatuses.Delivered,
            (int)OrderStatuses.DeliveredWithDelay,
            (int)OrderStatuses.NotDelivered
        };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var status = Convert.ToInt32(value);

                if (!AllowedStatuses.Contains(status))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
