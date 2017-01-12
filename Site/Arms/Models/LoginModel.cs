using System.ComponentModel.DataAnnotations;

namespace Vtb24.Arms.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
