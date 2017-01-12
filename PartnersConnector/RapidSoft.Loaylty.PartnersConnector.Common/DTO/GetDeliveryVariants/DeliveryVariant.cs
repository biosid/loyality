namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.GetDeliveryVariants
{
    using System.ComponentModel.DataAnnotations;

    public class DeliveryVariant
    {
        [Required]
        public string DeliveryVariantName { get; set; }

        [Required]
        public string ExternalDeliveryVariantId { get; set; }

        [ValidateObject]
        public PickupPoint[] PickupPoints { get; set; }
        
        public string Description { get; set; }

        [Required]
        public decimal? ItemsCost { get; set; }

        [Required]
        public decimal? DeliveryCost { get; set; }

        [Required]
        public decimal? TotalCost { get; set; }
    }
}