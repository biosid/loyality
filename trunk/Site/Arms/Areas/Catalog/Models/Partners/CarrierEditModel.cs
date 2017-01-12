using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;

namespace Vtb24.Arms.Catalog.Models.Partners
{
    public class CarrierEditModel
    {
        public int CarrierId { get; set; }

        [Required(ErrorMessage = "Укажите наименование курьера")]
        [StringLength(256, ErrorMessage = "Превышена допустимая длина наименования (256 символов)")]
        public string Name { get; set; }

        public Statuses Status { get; set; }

        [StringLength(256, ErrorMessage = "Превышена допустимая длина описания (256 символов)")]
        public string Description { get; set; }

        public PartnerEditMode EditMode { get; set; }

        public PartnerSettingModel[] Settings { get; set; }

        public Dictionary<string, string> OtherSettings { get; set; }

        public static CarrierEditModel Create()
        {
            return new CarrierEditModel
            {
                EditMode = PartnerEditMode.Create
            };
        }

        public static CarrierEditModel Map(Carrier original)
        {
            return new CarrierEditModel
            {
                EditMode = PartnerEditMode.Edit,
                CarrierId = original.Id,
                Name = original.Name,
                Status = original.Status.Map(),
                Description = original.Description
            };
        }
    }
}
