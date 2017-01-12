using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Vtb24.Arms.Catalog.Models.Shared;

namespace Vtb24.Arms.Catalog.Models.Categories
{
    public class CreateCategoryModel
    {
        [Required(ErrorMessage = "Укажите название категории")]
        [StringLength(256, ErrorMessage = "Превышна допустимая длина названия (256 символов)")]
        [RegularExpression("[^/]+", ErrorMessage = "Недупустимый символ \"/\" в имени категории")]
        public string Title { get; set; }

        public int? ParentId { get; set; }

        public Types Type { get; set; }

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

        public CategoriesLevelModel RootCategoryItem { get; set; }

        public SelectListItem[] OnlinePartners { get; set; }

        public enum Types
        {
            [Description("Статическая категория")]
            Static = 0,
            [Description("Онлайн категория")]
            Online = 1
        }
    }
}