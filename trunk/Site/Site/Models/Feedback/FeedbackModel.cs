using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Models.Feedback
{
    public class FeedbackModel
    {
        [Required(ErrorMessage = "Укажите ФИО")]
        [StringLength(152, ErrorMessage = "Превышена допустимая длина ФИО (152 символа)")]
        public string Fio { get; set; }

        [Required(ErrorMessage = "Укажите E-mail адрес")]
        [StringLength(250, ErrorMessage = "Превышена допустимая длина E-mail (250 символов)")]
        [SimpleEmailAddressAttribute(ErrorMessage = "Неверный формат E-mail")]
        public string Email { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Укажите текст сообщения")]
        [StringLength(1000, ErrorMessage = "Превышена допустимая длина сообщения (1000 символов)")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Выберите заказ из списка")]
        public int? OrderId { get; set;  }

        [Required(ErrorMessage = "Введите код с картинки")]
        [CaptchaValidator(ErrorMessage = "Код с картинки введен не верно")]
        [StringLength(8)]
        public string Captcha { get; set; }

        public FeedbackType SelectedType { get; set; }

        public FeedbackType[] Types { get; set; }

        public SelectListItem[] Orders { get; set; }

        public bool ValidateCaptcha { get; set; }

        public bool IsClient { get; set; }

        public bool IsFioReadonly { get; set; }

    }
}
