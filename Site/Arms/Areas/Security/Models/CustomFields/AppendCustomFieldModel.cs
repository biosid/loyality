using System.ComponentModel.DataAnnotations;

namespace Vtb24.Arms.Security.Models.CustomFields
{
    public class AppendCustomFieldModel
    {
        [Required(ErrorMessage = "Укажите наименование")]
        [StringLength(128, ErrorMessage = "Превышена допустимая длина наименования (128 символов)")]
        public string Name { get; set; }
    }
}
