using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Vtb24.Arms.Catalog.Models.Categories
{
    public class UpdateOnlineCategoryModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите URL онлайн категории")]
        [StringLength(512, ErrorMessage = "Превышена допустимая длина URL (512 символов)")]
        [RegularExpression("^https?://.*", ErrorMessage = "Введите валидный URL")]
        public string OnlineCategoryUrl { get; set; }

        [Required(ErrorMessage = "Укажите URL возврата")]
        [StringLength(512, ErrorMessage = "Превышена допустимая длина URL (512 символов)")]
        [RegularExpression("^https?://.*", ErrorMessage = "Введите валидный URL")]
        public string NotifyOrderStatusUrl { get; set; }

        [Required(ErrorMessage = "Укажите online-партнера")]
        public int OnlineCategoryPartnerId { get; set; }

        public SelectListItem[] OnlinePartners { get; set; }
    }
}