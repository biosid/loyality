using System.ComponentModel.DataAnnotations;

namespace Vtb24.Arms.AdminSecurity.Models.Users
{
    public class ResetUserPasswordModel
    {
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо указать пароль")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Неверная длина пароля (допускается от 8 до 64 символов)")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])[0-9A-Za-z]+$", ErrorMessage = "Пароль должен состоять из латинских букв и цифр. Пароль должен включать хотя бы одну цифру, хотя бы одну маленькую букву и хотя бы одну большую букву.")]
        public string Password { get; set; }
    }
}
