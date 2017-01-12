namespace Rapidsoft.Loyalty.NotificationSystem.Email
{
    using System.Collections.Generic;
    using System.Net.Mail;

    public class Sender : ISender
    {
        #region ISender Members

        public void SendEmail(IList<string> to, string from, string subject, string body)
        {
            var message = new MailMessage();

            if (!string.IsNullOrEmpty(from))
            {
                message.From = new MailAddress(@from);
            }

            foreach (var addressee in to)
            {
                message.To.Add(new MailAddress(addressee));
            }

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            var client = new SmtpClient();
            client.Send(message);
        }

        public void SendEmail(MailMessage message)
        {
            var client = new SmtpClient();
            client.Send(message);
        }

        #endregion
    }
}