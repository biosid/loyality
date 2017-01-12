using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;

namespace Vtb24.Arms.Catalog.Models.Partners
{
    public class SupplierEditModel
    {
        public int SupplierId { get; set; }

        public SelectListItem[] CarriersList { get; set; }

        [Required(ErrorMessage = "Укажите наименование поставщика")]
        [StringLength(256, ErrorMessage = "Превышена допустимая длина наименования (256 символов)")]
        public string Name { get; set; }

        public Types Type { get; set; }

        public Statuses Status { get; set; }

        public TrustLevels TrustLevel { get; set; }

        public int? CarrierId { get; set; }

        [StringLength(256, ErrorMessage = "Превышена допустимая длина описания (256 символов)")]
        public string Description { get; set; }

        public PartnerEditMode EditMode { get; set; }

        public PartnerSettingModel[] Settings { get; set; }

        public Dictionary<string, string> OtherSettings { get; set; }

        public static SupplierEditModel Create()
        {
            return new SupplierEditModel
            {
                EditMode = PartnerEditMode.Create
            };
        }

        public static SupplierEditModel Map(Supplier original)
        {
            return new SupplierEditModel
            {
                EditMode = PartnerEditMode.Edit,
                SupplierId = original.Id,
                Name = original.Name,
                Type = original.Type.Map(),
                Status = original.Status.Map(),
                TrustLevel = original.TrustLevel.Map(),
                CarrierId = original.CarrierId,
                Description = original.Description
            };
        }
    }
}
