using System.Net.Mail;

namespace ScheduledJobs.Infrastructure.Mail
{
    public class FetchedMessage
    {
        public FetchedMessage(string id, MailMessage message)
        {
            Id = id;
            Message = message;
        }

        public string Id { get; private set; } 

        public MailMessage Message { get; private set; }
    }
}