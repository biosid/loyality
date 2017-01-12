using System.Linq;
using System.Net.Mail;
using ScheduledJobs.FeedbackByEmail.Templates;
using Vtb24.Arms.AdminServices.AdminFeedbackServiceEndpoint;

namespace ScheduledJobs.FeedbackByEmail
{
    public class ThreadMessageGenerator
    {
        public MailMessage Execute(GetThreadMessagesResult thread)
        {
            var subject = CreateSubject(thread);
            var body = CreateBody(thread);

            var mail = new MailMessage(Settings.MailUser, Settings.MailTo)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            return mail;
        }


        private string CreateSubject(GetThreadMessagesResult thread)
        {
            var intro = thread.ThreadMessages.Last().MessageType == MessageTypes.OperatorMessage
                ? "Ответ оператора. "
                : ""; 
            return string.Format("{0}{1} [ID:{2};C:{3}]", intro, thread.Thread.Title, thread.Thread.Id, thread.TotalCount);
        }

        private string CreateBody(GetThreadMessagesResult thread)
        {
            var template = new ThreadMessageBody
            {
                Title = thread.Thread.Title,
                Messages = thread.ThreadMessages.Select(ThreadMessageBody.Message.Map).ToArray()
            };

            return template.TransformText();
        }
    }
}