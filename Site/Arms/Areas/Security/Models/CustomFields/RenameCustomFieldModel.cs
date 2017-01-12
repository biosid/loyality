using System.ComponentModel.DataAnnotations;

namespace Vtb24.Arms.Security.Models.CustomFields
{
    public class RenameCustomFieldModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите наименование")]
        [StringLength(128, ErrorMessage = "Превышена допустимая длина наименования (128 символов)")]
        public string Name { get; set; }
    }
}
