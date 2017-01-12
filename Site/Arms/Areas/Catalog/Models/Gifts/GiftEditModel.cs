using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public class GiftEditModel
    {
        // ReSharper disable InconsistentNaming
        public string query { get; set; }
        // ReSharper restore InconsistentNaming

        public string Id { get; set; }

        public int CategoryId { get; set; }

        public string CategoryPath { get; set; }

        public string SupplierName { get; set; }

        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Необходимо указать артикул")]
        [StringLength(256, ErrorMessage = "Превышена допустимая длина артикула (256 символов)")]
        public string SupplierProductId { get; set; }

        [Required(ErrorMessage = "Необходимо указать название")]
        [StringLength(256, ErrorMessage = "Превышена допустимая длина названия (256 символов)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо указать цену")]
        [RegularExpression(@"\d+([\.,]\d{1,2}){0,1}", ErrorMessage = "Неверный формат цены")]
        public decimal? PriceRUR { get; set; }

        [RegularExpression(@"\d+([\.,]\d{1,2}){0,1}", ErrorMessage = "Неверный формат цены")]
        public decimal? BasePriceRUR { get; set; }

        [Required(ErrorMessage = "Необходимо указать вес")]
        [Range(0, 300000, ErrorMessage = "Вес должен быть в диапазоне от {1} до {2} грамм")]
        public int? Weight { get; set; }

		[Required(ErrorMessage = "Необходимо указать признак доставки по e-mail")]
	    public bool IsDeliveredByEmail { get; set; }

        [StringLength(2000, ErrorMessage = "Превышена допустимая длина описания (2000 символов)")]
        [AllowHtml]
        public string Description { get; set; }

        [StringLength(256, ErrorMessage = "Превышена допустимая длина производителя (256 символов)")]
        public string Vendor { get; set; }

        public string[] PictureUrls { get; set; }

        public HttpPostedFileBase[] Pictures { get; set; }

        public Parameter[] Parameters { get; set; }

        public Statuses Status { get; set; }

        public string Segments { get; set; }

        public string ModerationStatus { get; set; }

        public bool IsRecommended { get; set; }

        public GiftsQueryModel GiftsQueryModel
        {
            get
            {
                return string.IsNullOrWhiteSpace(query) 
                    ? new GiftsQueryModel { supplier = SupplierId, category = CategoryId } 
                    : new GiftsQueryModel().MixQuery(query);
            }
        }

        public bool IsNewProduct { get; set; }

        public class Parameter
        {
            public string Name { get; set; }

            public string Value { get; set; }

            public string Unit { get; set; }
        }
    }
}