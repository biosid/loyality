using System;
using System.Web;
using Vtb24.Site.Controllers;
using Vtb24.Site.Services.ClientMessageService;

namespace Vtb24.Site.Models.MyMessages
{
    public class ThreadModel
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public bool IsRead { get; set; }

        public ThreadType Type { get; set; }

        public bool IsConversation { get; set; }

        public int Pages { get; set; }

        public static ThreadModel Map(Thread original)
        {
            var isConversation = original.Type == ThreadTypes.Issue || original.Type == ThreadTypes.Suggestion || original.Type == ThreadTypes.OrderIncident;
            string subject;
            switch (original.Type)
            {
                case ThreadTypes.Issue:
                case ThreadTypes.Suggestion:
                    subject = string.Format("Ответ на ваше обращение от {0:dd.MM.yyy H:mm}", original.InsertedDate);
                break;
                case ThreadTypes.OrderIncident:
                    var metadata = HttpUtility.ParseQueryString(original.MetaData ?? "");
                    var orderId = metadata["OrderId"];
                    var externalOrderId = string.IsNullOrWhiteSpace(metadata["ExternalOrderId"])
                        ? "-"
                        : metadata["ExternalOrderId"];
                    subject = string.Format("Ответ на ваш вопрос по оформленному заказу (№{1}\\{2}) от {0:dd.MM.yyy H:mm}", original.InsertedDate, orderId, externalOrderId);
                    break;
                default:
                    subject = original.Title;
                    break;

            }
            return new ThreadModel
            {
                Id = original.Id,
                Date = original.InsertedDate,
                Subject = subject,
                IsRead = original.UnreadMessagesCount == 0,
                Type = original.Type.Map(),
                Text = original.TopicMessage != null ? original.TopicMessage.MessageBody : string.Empty,
                IsConversation = isConversation,
                Pages = (int) Math.Ceiling((decimal) original.MessagesCount / FeedbackController.PAGE_SIZE)
            };
        }

    }
}
