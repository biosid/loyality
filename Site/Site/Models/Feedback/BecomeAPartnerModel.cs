using System.ComponentModel.DataAnnotations;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Models.Feedback
{
    public class BecomeAPartnerModel
    {
        [Required(ErrorMessage = "Укажите город")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Укажите юридическое лицо")]
        public string LegalEntity { get; set; }

        [Required(ErrorMessage = "Укажите контактное лицо")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "Укажите номер телефона")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Укажите E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите отрасль компании")]
        public string Branch { get; set; }

        [Required(ErrorMessage = "Укажите группу товаров")] 
        public string GroupOfGoods { get; set; }

        [Required(ErrorMessage = "Укажите адрес сайта")]
        public string Site { get; set; }

        [Required(ErrorMessage = "Укажите количество торговых точек")]
        public string PosCount { get; set; }

        public string Comment { get; set; }

        [Required(ErrorMessage = "Введите код с картинки")]
        [CaptchaValidator(ErrorMessage = "Код с картинки введен не верно")]
        [StringLength(8)]
        public string Captcha { get; set; }

    }
}