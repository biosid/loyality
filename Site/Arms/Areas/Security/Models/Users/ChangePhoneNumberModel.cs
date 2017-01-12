using System.ComponentModel.DataAnnotations;
using Vtb24.Arms.Infrastructure;

namespace Vtb24.Arms.Security.Models.Users
{
    public class ChangePhoneNumberModel
    {
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите новый номер")]
        [Mask("+7 (999) 999-9999", Constants.PHONE_REGEX, ErrorMessage = "Введённый номер телефона не может быть использован. Пожалуйста, укажите номер мобильного телефона.")]
        public string PhoneNumber { get; set; }
    }
}
