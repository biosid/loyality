using System;
using System.IO;
using System.Linq;
using System.Web;
using Vtb24.Site.Services.ClientMessageService;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Site.Models.Feedback
{
    public class FeedbackThreadModel
    {
        public int TotalPages { get; set; }

        public int Page { get; set; }

        public string MvcAction { get; set; }

        public bool IsAnonymous { get; set; }

        public int MaxFileSizeMb { get; set; }

        public int MaxTotalFilesSizeMb { get; set; }

        public string Title { get; set; }

        public Message[] Messages { get; set; }

        public bool ShowClientMenu { get; set; }

        public bool ShowReplyForm { get; set; }

        public bool AllowUploads { get; set; }

        public bool ShowCaptcha { get; set; }

        public FeedbackReplyModel ReplyForm { get; set; }

        public static FeedbackThreadModel Map(Thread original, ThreadMessage[] messages)
        {
            if (original.Type != ThreadTypes.Issue && original.Type != ThreadTypes.Suggestion && original.Type != ThreadTypes.OrderIncident)
            {
                throw new InvalidOperationException(string.Format("Тип веток {0} не поддерживается", original.Type));
            }
            return new FeedbackThreadModel
            {
                IsAnonymous = original.ClientType == ThreadClientTypes.Guest,
                MaxFileSizeMb = AppSettingsHelper.Int("feedback_max_single_upload_size_mb", 10),
                MaxTotalFilesSizeMb = AppSettingsHelper.Int("feedback_max_total_upload_size_mb", 100),
                Title = MapTitle(original),
                ShowReplyForm = !original.IsClosed,
                Messages = messages.Select(Message.Map).ToArray(),
                ReplyForm = new FeedbackReplyModel
                {
                    Id = original.Id
                }
            };
        }

        public static string MapTitle(Thread original)
        {
            if (original.Type == ThreadTypes.Issue)
            {
                return string.Format("Претензия от {0: dd.MM.yyyy H:mm}", original.InsertedDate);
            }

            if (original.Type == ThreadTypes.Suggestion)
            {
                return string.Format("Предложение от {0: dd.MM.yyyy H:mm}", original.InsertedDate);
            }

            if (original.Type == ThreadTypes.OrderIncident)
            {
                var metadata = HttpUtility.ParseQueryString(original.MetaData ?? "");
                var orderId = metadata["OrderId"];
                var externalOrderId = string.IsNullOrWhiteSpace(metadata["ExternalOrderId"])
                    ? "-"
                    : metadata["ExternalOrderId"];
                return string.Format("Вопрос по оформленному заказу (№{1}\\{2}) от {0: dd.MM.yyyy H:mm}", original.InsertedDate, orderId, externalOrderId);
            }

            return string.Format("Обращение от {0: dd.MM.yyyy H:mm}", original.InsertedDate);
        }

        public class Message
        {
            public int Index { get; set; }

            public DateTime Time { get; set; }

            public string Text { get; set; }

            public bool IsReply { get; set; }

            public Attachment[] Attachments { get; set; }

            public static Message Map(ThreadMessage original)
            {
                return new Message
                {
                    Index = original.Index,
                    IsReply = original.MessageType == MessageTypes.OperatorMessage,
                    Text = original.MessageBody,
                    Time = original.InsertedDate,
                    Attachments = original.Attachments.Select(Attachment.Map).ToArray()
                };
            }
            
        }

        public class Attachment
        {
            public string Title { get; set; }

            public string Link { get; set; }

            public string Type { get; set; }

            public static Attachment Map(MessageAttachment original)
            {
                return new Attachment
                {
                    Title = original.FileName,
                    Link = SiteUploadsHelper.GetUploadUrl(original.Id, original.FileName),
                    Type = Path.GetExtension(original.FileName)
                };
            }
        }
    }
}