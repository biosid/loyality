using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using Vtb24.Site.Services.Infrastructure;

namespace ScheduledJobs.Infrastructure.Mail
{
    public static class AttachmentExtensions
    {
        /// <summary>
        /// Сохранить вложения на сайт в публичную папку закачек.
        /// Получить ссылку на вложение можно, используя SiteUploadsHelper.GetUploadUrl(fileId, fileName);
        /// </summary>
        public static IEnumerable<SiteUploadsHelper.UploadInfo> SaveAsSiteUpload(this AttachmentCollection attachments)
        {
            var uploads = new List<SiteUploadsHelper.UploadInfo>();

            if (attachments == null)
            {
                return uploads;
            }

            foreach (var attach in attachments.Where(f => f != null))
            {
                uploads.Add(SiteUploadsHelper.SaveUpload(attach.Name, attach.ContentStream));
            }

            return uploads;
        }

        /// <summary>
        /// Приложить сообщение в качестве *.eml вложения
        /// </summary>
        public static void Add(this AttachmentCollection attachments, MailMessage message, string name)
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);

            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = dir
            };
            client.Send(message);

            var path = Directory.EnumerateFiles(dir).First();
            var attach = new Attachment(path, new ContentType{ Name = name + ".eml" });

            attachments.Add(attach);
        }
    }
}