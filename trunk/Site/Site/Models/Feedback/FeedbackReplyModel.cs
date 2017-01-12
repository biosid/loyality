using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Models.Feedback
{
    public class FeedbackReplyModel
    {
        [Required(ErrorMessage = "Не указан Id переписки")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Введите текст ответа")]
        [StringLength(1000, ErrorMessage = "Превышена допустимая длина сообщения (1000 символов)")]
        [AllowHtml]
        public string Text { get; set; }

        [Required(ErrorMessage = "Введите код с картинки")]
        [CaptchaValidator(ErrorMessage = "Код с картинки введен не верно")]
        [StringLength(8)]
        public string Captcha { get; set; }
        
        public HttpPostedFileBase[] Files { get; set; }
         
    }
}