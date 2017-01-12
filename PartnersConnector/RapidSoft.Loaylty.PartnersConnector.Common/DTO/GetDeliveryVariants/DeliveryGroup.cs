namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.GetDeliveryVariants
{
    using System.ComponentModel.DataAnnotations;

    public class DeliveryGroup
    {        
        [Required]
        public string GroupName { get; set; }

        [Required, ValidateObject]
        public DeliveryVariant[] DeliveryVariants { get; set; }
    }
}