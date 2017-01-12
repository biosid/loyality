namespace RapidSoft.Etl.LogSender
{
    using System.Net.Mail;
    using System.Text;

    public class LogEmailSender : ILogEmailSender
    {
        public void SendMail(string subject, string[] recipients, string body)
        {
            var message = new MailMessage
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
            };

            foreach (var recipient in recipients)
            {
                message.To.Add(recipient);
            }

            var client = new SmtpClient();

            client.Send(message);
        }
    }
}