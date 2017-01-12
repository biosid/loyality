using System;
using System.Linq;
using System.Web;
using Vtb24.Arms.AdminServices.AdminFeedbackServiceEndpoint;
using Vtb24.Site.Services.Infrastructure;

namespace ScheduledJobs.FeedbackByEmail.Templates
{
    public partial class ThreadMessageBody
    {
        public string Title { get; set; }

        public Message[] Messages { get; set; }

        public class Message
        {
            public string Text { get; set; }

            public bool IsOperator { get; set; }

            public DateTime Time { get; set; }

            public Attach[] Attachments { get; set; }

            public static Message Map(ThreadMessage original)
            {
                return new Message
                {
					Text = HttpUtility.HtmlEncode(original.MessageBody),
					IsOperator = original.MessageType == MessageTypes.OperatorMessage,
					Time = original.InsertedDate,
                    Attachments = original.Attachments == null 
                        ? new Attach[0] 
                        : original.Attachments.Select(Attach.Map).ToArray() 
                };
            }
        }

        public class Attach
        {
            public string Title { get; set; }

            public string Url { get; set; }

            public static Attach Map(MessageAttachment original)
            {
                return new Attach
                {
                    Title = original.FileName,
                    Url = SiteUploadsHelper.GetUploadUrl(original.Id, original.FileName)
                };
            }
        }
    }
}