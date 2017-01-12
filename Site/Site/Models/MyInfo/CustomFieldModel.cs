using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Vtb24.Site.Services.MyInfoService.Models;

namespace Vtb24.Site.Models.MyInfo
{
    public class CustomFieldModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [StringLength(250, ErrorMessage = "Слишком длинное значение")]
        [AllowHtml]
        public string Value { get; set; }

        public static CustomFieldModel Map(CustomField field)
        {
            return new CustomFieldModel
            {
                Id = field.Id,
                Title = field.Title,
                Value = field.Value
            };
        }
    }
}