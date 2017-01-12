using System;
using System.Linq;
using Vtb24.Arms.AdminServices.AdminFeedbackServiceEndpoint;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Arms.Security.Models.Feedback
{
    public class FeedbackThreadModel
    {   
        public string Title { get; set; }

        public bool ShowMarkAsAnswered { get; set; }

        public Message[] Messages { get; set; }

        public int MaxFileSizeMb { get; set; }

        public int MaxTotalFilesSizeMb { get; set; }

        public string ClientPhone { get; set; }

        public int Page { get; set; }

        public int PagesCount { get; set; }

        public FeedbackThreadReplyModel Reply { get; set; }

        public string Filter { get; set; }

        public class Message
        {
            private string _clientId;
            
            public int Index { get; set; }

            public bool IsOperator { get; set; }

            public string Author { get; set; }

            public bool ShowAuthorLink { get; set; }

            public string AuthorMail { get; set; }

            public string Text { get; set; }

            public Attach[] Attachments { get; set; }

            public DateTime Time { get; set; }

            public string AuthorUrl(Func<string, string> foo)
            {
                return foo(_clientId);
            }

            public static Message Map(ThreadMessage message)
            {
                var isOperator = message.MessageType == MessageTypes.OperatorMessage;
                var isClient = message.MessageType == MessageTypes.ClientMessage;
                var model = new Message
                {
                    _clientId = isClient ? message.AuthorId : null,
                    ShowAuthorLink = isClient,
                    Index = message.Index,
                    IsOperator = isOperator,
                    Author = isOperator ? message.AuthorId : message.AuthorFullName,
                    AuthorMail = message.AuthorEmail,
                    Text = message.MessageBody,
                    Time = message.InsertedDate
                };

                if (message.Attachments != null && message.Attachments.Any())
                {
                    model.Attachments = message.Attachments.Select(Attach.Map).ToArray();
                }

                return model;
            }
        }

        public class Attach
        {
            public string Filename { get; set; }

            public string Link { get; set; }

            public static Attach Map(MessageAttachment attachment)
            {
                var model = new Attach
                {
                    Filename = attachment.FileName,
                    Link = SiteUploadsHelper.GetUploadUrl(attachment.Id, attachment.FileName)
                };

                return model;
            }
            
        }

        public static FeedbackThreadModel Map(
            GetThreadMessagesResult thread,
            string clientPhone)
        {
            var formattedPhone = PhoneFormatter.FormatPhoneNumber(clientPhone);

            var model = new FeedbackThreadModel
            {
                Title = MapTitle(thread.Thread, formattedPhone),
                ShowMarkAsAnswered = !thread.Thread.IsAnswered,
                ClientPhone = formattedPhone,
                Messages = thread.ThreadMessages.Select(Message.Map).ToArray()
            };
            return model;
        }

        public static string MapTitle(Thread thread, string clientPhone)
        {
            return thread.Title;
        }
    }
}