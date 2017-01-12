using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Vtb24.Arms.Security.Models.Feedback
{
    public class FeedbackThreadReplyModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Введите текст сообщения")]
        [StringLength(1000, ErrorMessage = "Превышена допустимая длина сообщения (1000 символов)")]
        [AllowHtml]
        public string Text { get; set; }

        public bool IgnoreThread { get; set; }

        public HttpPostedFileBase[] Files { get; set; }
    }
}