using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Models.Account
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите номер телефона")]
        [Mask("+7 (999) 999-9999", ErrorMessage = "Номер телефона должен быть в формате +7 (xxx) xxx-xxxx")]
        public string Phone { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(25)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}