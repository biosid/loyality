namespace RapidSoft.Etl.Runtime.Steps
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime.DataSources.DB;

    [Serializable]
    public sealed class EtlEmailSendStep : EtlStep
    {
        #region Properties

        [Category("2. Source")]
        public EtlFileInfo Source { get; set; }

        [Category("3. Destination")]
        public EtlEmailServer EmailServer { get; set; }

        [Category("3. Destination")]
        public EtlEmailSendMessage Message { get; set; }

        [Category("3. Destination")]
        public EtlEmailDbStorage EmailDbStorage { get; set; }
        #endregion

        #region Methods

        public override EtlStepResult Invoke(EtlContext context, IEtlLogger logger)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (this.Source == null)
            {
                throw new InvalidOperationException("Source cannot be null");
            }

            if (this.EmailServer == null)
            {
                throw new InvalidOperationException("EmailServer cannot be null");
            }

            this.EmailServer.ThrowIfInvalid();

            if (this.Message == null)
            {
                throw new InvalidOperationException("Message cannot be null");
            }

            this.Message.ThrowIfInvalid();

            //if (this.To == null || this.To.Length == 0)
            //{
            //    throw new InvalidOperationException("RecipientAddress cannot be null or empty");
            //}

            var stepLogger = new StepLogger(logger, context, this.Name);

            var message = new MailMessage();

            string[] files;
            var filePath = this.Source.FilePath;
            var fileResponseMask = this.Message.AttachmentFileMask;

            if (!string.IsNullOrEmpty(fileResponseMask))
            {
                stepLogger.LogGetFilesListByMask(filePath, fileResponseMask);
                files = Directory.GetFiles(filePath, fileResponseMask);
            }
            else
            {
                stepLogger.LogGetFilesList(filePath);
                files = Directory.GetFiles(this.Source.FilePath);
            }

            stepLogger.LogCountFoundFiles(files.Length, filePath);
            
            foreach (var fileName in files)
            {
                message.Attachments.Add(new Attachment(fileName));
            }

            stepLogger.LogSendMail(this.Message);
            message = this.SendEmail(message);

            using (var ds = new MailDataSource(this.EmailDbStorage.ConnectionString, this.EmailDbStorage.SchemaName))
            {
                ds.SaveOutcomingMessageWithAttachments(
                    context.EtlPackageId,
                    context.EtlSessionId,
                    message.Subject,
                    message.From.Address,
                    message.To.Select(x => x.Address).ToArray(),
                    message.Attachments.Select(x => x.Name).ToArray());
            }

            return new EtlStepResult(EtlStatus.Succeeded, null);
        }

        private MailMessage SendEmail(MailMessage message)
        {
            var client = new SmtpClient
                             {
                                 Host = this.EmailServer.Host,
                                 Port = this.EmailServer.IntPort,
                                 EnableSsl = this.EmailServer.BoolUseSSL,
                                 Credentials =
                                     new NetworkCredential(this.EmailServer.UserName, this.EmailServer.Password)
                             };

            message.Subject = this.Message.Subject ?? string.Empty;
            message.From = new MailAddress(this.Message.From);
            message.Sender = new MailAddress(this.Message.From);

            if (!string.IsNullOrWhiteSpace(this.Message.Body))
            {
                message.Body = this.Message.Body;
                message.IsBodyHtml = false;
            }

            foreach (var to in this.Message.To)
            {
                message.To.Add(to);
            }

            client.Send(message);

            return message;
        }

        #endregion

        private class StepLogger : StepLoggerBase
        {
            public StepLogger(IEtlLogger logger, EtlContext context, string stepName)
                : base(logger, context, stepName)
            {
            }

            internal void LogGetFilesListByMask(string filePath, string fileResponseMask)
            {
                var mess =
                    string.Format(
                        "Получение списка файлов в папке \"{0}\", с именем, соответствующим маске \"{1}\"",
                        filePath,
                        fileResponseMask);
                this.LogInfo(mess);
            }

            internal void LogGetFilesList(string filePath)
            {
                var mess = string.Format("Получение списка всех файлов в папке \"{0}\"", filePath);
                this.LogInfo(mess);
            }

            internal void LogCountFoundFiles(int filesCount, string filePath)
            {
                var mess = string.Format("Список файлов из каталога ({0}) включает элементов ({1})", filePath, filesCount);
                this.LogInfo(mess);
            }
            
            public void LogSendMail(EtlEmailSendMessage message)
            {
                var mess = string.Format(
                    "Отправка сообщения \"{0}\" на адрес {1}", message.Subject, string.Join(", ", message.To));
                this.LogInfo(mess);
            }
        }
    }
}
