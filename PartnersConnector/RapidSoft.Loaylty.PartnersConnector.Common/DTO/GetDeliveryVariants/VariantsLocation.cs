namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.GetDeliveryVariants
{
    using System.ComponentModel.DataAnnotations;

    public class VariantsLocation
    {
        [Required]
        public string LocationName { get; set; }

        public string KladrCode { get; set; }

        [Required]
        public string PostCode { get; set; }

        public string ExternalLocationId { get; set; }
    }
}