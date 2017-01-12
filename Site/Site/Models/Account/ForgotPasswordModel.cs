using System.ComponentModel.DataAnnotations;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Models.Account
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Введите номер телефона")]
        [Mask("+7 (999) 999-9999", ErrorMessage = "Номер телефона должен быть в формате +7 (xxx) xxx-xxxx")]
        public string Phone { get; set; } 
    }
}