using System.ComponentModel.DataAnnotations;
using Vtb24.Arms.Infrastructure;

namespace Vtb24.Arms.Security.Models.Users
{
    public class ChangeEmailModel
    {
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите E-mail")]
        [StringLength(250, ErrorMessage = "Превышена допустимая длина E-mail (250 символов)")]
        [SimpleEmailAddress(ErrorMessage = "Неверный формат E-mail")]
        public string Email { get; set; }
    }
}
