using System.Net;
using System.Net.Mail;
using Vtb24.Site.Services.Infrastructure;

namespace ScheduledJobs.Infrastructure.Mail
{
    static class SmtpFactory
    {
        public static SmtpClient CreateSmtpClient(string user, string password)
        {
            var host = AppSettingsHelper.String("mailbox_host", "localhost");
            var port = AppSettingsHelper.Int("mailbox_smtp_port", 25);

            var smtp = new SmtpClient(host, port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, password)
            };
            return smtp;
        }
    }
}