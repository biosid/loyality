namespace Rapidsoft.Loyalty.NotificationSystem.Email
{
    using System.Collections.Generic;
    using System.Net.Mail;

    public interface ISender
    {
        void SendEmail(IList<string> to, string from, string subject, string body);

        void SendEmail(MailMessage message);
    }
}