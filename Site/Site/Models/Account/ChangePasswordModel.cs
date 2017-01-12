using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Vtb24.Site.Models.Account
{
    public class ChangePasswordModel
    {
        [AllowHtml]
        [Required(ErrorMessage = "Не указан старый пароль")]
        public string Password { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Введите новый пароль")]
        [RegularExpressionAttribute(Constants.PASSWORD_REGEX, ErrorMessage = Constants.PASSWORD_ERROR_MESSAGE)]
        public string NewPassword { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Подтвердите пароль")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Введённые пароли не совпадают")]
        public string ConfirmNewPassword { get; set; } 
    }
}