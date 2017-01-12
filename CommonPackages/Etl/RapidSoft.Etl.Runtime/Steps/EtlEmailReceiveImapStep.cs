using System;
using System.ComponentModel;
using System.IO;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Steps
{
    using System.Globalization;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;
    using System.Text.RegularExpressions;

    using RapidSoft.Etl.Runtime.DataSources.DB;

    using S22.Imap;

    [Serializable]
    public sealed class EtlEmailReceiveImapStep : EtlStep
    {
        private int maxEmailsToDownload = 1;

        public const string PassedMailsCountVarName = "PassedMailsCount";

        #region Properties

		[Category("1. General")]
		public bool EndSessionOnEmptySource { get; set; }

        [Category("2. Source")]
        public EtlEmailServer EmailServer { get; set; }

        [Category("3. Destination")]
        public EtlEmailDbStorage EmailDbStorage { get; set; }

        [Category("2. Source")]
        public EtlEmailReceiveMessage Message { get; set; }

        [Category("3. Destination")]
        public EtlFileInfo Destination { get; set; }

        [Category("2. Source")]
        public int MaxEmailsToDownload
        {
            get
            {
                return this.maxEmailsToDownload;
            }
            set
            {
                this.maxEmailsToDownload = value;
            }
        }

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

            if (this.EmailServer == null)
            {
                throw new InvalidOperationException("EmailServer cannot be null");
            }

            this.EmailServer.ThrowIfInvalid();

            if (this.EmailDbStorage == null)
            {
                throw new InvalidOperationException("EmailDbStorage cannot be null");
            }

            this.EmailDbStorage.ThrowIfInvalid();

            if (this.Message == null)
            {
                throw new InvalidOperationException("Message cannot be null");
            }

            this.Message.ThrowIfInvalid();

            if (this.Destination != null && string.IsNullOrEmpty(this.Destination.FilePath))
            {
                throw new InvalidOperationException("Destination.FilePath cannot be empty");
            }

            if (this.Destination != null)
            {
                var destinationFolderPath = this.Destination.FilePath;

                if (!string.IsNullOrEmpty(destinationFolderPath))
                {
                    if (!Directory.Exists(destinationFolderPath))
                    {
                        Directory.CreateDirectory(destinationFolderPath);
                    }
                }
            }

            var stepLogger = new StepLogger(logger, context, this.Name, this.EmailServer.Host, this.EmailServer.Port);

            try
            {
                stepLogger.ImapConnect();
                using (var client = new ImapClient(this.EmailServer.Host, this.EmailServer.IntPort, this.EmailServer.BoolUseSSL))
                using (var ds = new MailDataSource(this.EmailDbStorage.ConnectionString, this.EmailDbStorage.SchemaName))
                {
                    var userName = this.EmailServer.UserName;
                    stepLogger.Login(userName);
                    client.Login(userName, this.EmailServer.Password, AuthMethod.Auto);

                    const string MailboxName = "inbox";
                    stepLogger.SelectMailbox(MailboxName);
                    client.DefaultMailbox = MailboxName;

                    var maxUid = ds.GetMaxDeletedIncomingMessageUid(context.EtlPackageId);

	                var uids = client.Search(SearchCondition.Undeleted().And(SearchCondition.GreaterThan(maxUid))).OrderBy(x => x).ToArray();

                    stepLogger.MessagesFound(maxUid, uids.Length);

                    var attachmentsCount = 0;
                    var passedMailsCount = 0;                    

                    foreach (var uid in uids)
                    {
                        stepLogger.GetMessageHeaders(uid);
                        var header = client.GetMessage(uid, FetchOptions.HeadersOnly);

                        if (!this.FilterPassed(header))
                        {
                            continue;
                        }

                        attachmentsCount += this.SaveMessage(context, stepLogger, uid, client, ds, ref passedMailsCount);
                    }

                    var now = DateTime.Now;

                    logger.LogEtlVariable(new EtlVariable()
                                          {
                                              EtlPackageId = context.EtlPackageId,
                                              EtlSessionId = context.EtlSessionId,
                                              Modifier = EtlVariableModifier.Output,
                                              Name = PassedMailsCountVarName,
                                              Value = passedMailsCount.ToString(CultureInfo.InvariantCulture),
                                              DateTime = now,
                                              UtcDateTime = now.ToUniversalTime()
                                          });

                    stepLogger.LogComplete();

                    if (attachmentsCount == 0)
                    {
                        stepLogger.NoAttachmentsFound(maxUid);

                        if (this.EndSessionOnEmptySource)
                        {
                            return new EtlStepResult(
                                EtlStatus.FinishedWithSessionEnd,
                                "Отсутствуют новые письма. Выполнение процесса прервано.");
                        }
                    }

                    return new EtlStepResult(EtlStatus.Succeeded, null);
                }
            }
            catch (Exception ex)
            {
                stepLogger.LogError(ex);
                throw;
            }
        }

        private int SaveMessage(EtlContext context, StepLogger stepLogger, uint uid, ImapClient client, MailDataSource ds, ref int passedMailsCount)
        {
            stepLogger.GetMessage(uid);

            var messageRaw = client.GetRawMessage(uid);
            var message = MessageBuilder.FromMIME822(messageRaw);

            var passedAttachments = message.Attachments.Where(this.AttachmentsPassedByName).ToArray();

            if (passedAttachments.Length == 0)
            {
                return 0;
            }
            else
            {
                passedMailsCount++;
            }

            if (passedMailsCount > MaxEmailsToDownload)
            {
                return 0;
            }

            var savedAttachmentsCount = 0;

            foreach (var attachment in passedAttachments)
            {
                this.SaveAttachmentToFile(stepLogger, uid, attachment);
                ds.SaveIncomingMessageAttachment(context.EtlPackageId, context.EtlSessionId, attachment.Name);
                savedAttachmentsCount++;
            }

            stepLogger.SaveMessageInDb(uid);
            ds.SaveIncomingMessage(context.EtlPackageId, context.EtlSessionId, uid, messageRaw);

            return savedAttachmentsCount;
        }

        private void SaveAttachmentToFile(StepLogger stepLogger, uint uid, Attachment attachment)
        {
            stepLogger.SaveAttachment(uid, attachment.Name);
            var fileName = Path.Combine(this.Destination.FilePath, attachment.Name);

			//NOTE: при перезаписи файла возникнет проблема, если новый файл меньше старого
			if (File.Exists(fileName))
			{
				stepLogger.FileRewrite(fileName, uid);
				File.Delete(fileName);
			}

            using (Stream file = File.OpenWrite(fileName))
            {
                var buffer = new byte[8 * 1024];
                int len;
                while ((len = attachment.ContentStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    file.Write(buffer, 0, len);
                }
            }
        }

        private bool AttachmentsPassedByName(Attachment x)
        {
            return string.IsNullOrEmpty(this.Message.AttachmentRegExp)
                   || Regex.IsMatch(x.Name.ToLower(), this.Message.AttachmentRegExp.ToLower(), RegexOptions.IgnoreCase);
        }

        private bool FilterPassed(MailMessage header)
        {
            var filters = this.Message.Filters;
            return filters == null || filters.Count == 0
                   || filters.Exists(
                       x =>
                          FilterPassedByFrom(x, header)
                       && FilterPassedBySubjectContains(x, header)
                       && FilterPassedBySubjectStartsWith(x, header));
            ////return string.IsNullOrEmpty(this.Message.AttachmentRegExp)
            ////       || Regex.IsMatch(header.Subject.ToLower(), this.Message.AttachmentRegExp.ToLower(), RegexOptions.IgnoreCase);
        }

        private static bool FilterPassedBySubjectStartsWith(EtlEmailReceiveFilter filter, MailMessage header)
        {
            return string.IsNullOrEmpty(filter.SubjectStartsWith)
                   || (header.Subject != null && header.Subject.ToLower().StartsWith(filter.SubjectStartsWith.ToLower()));
        }

        private static bool FilterPassedBySubjectContains(EtlEmailReceiveFilter filter, MailMessage header)
        {
            return string.IsNullOrEmpty(filter.SubjectContains)
                    || (header.Subject != null && header.Subject.ToLower().Contains(filter.SubjectContains.ToLower()));
        }

        private static bool FilterPassedByFrom(EtlEmailReceiveFilter filter, MailMessage header)
        {
            return string.IsNullOrEmpty(filter.From)
                   || (header.From.User.Equals(filter.From, StringComparison.InvariantCultureIgnoreCase)
                       || header.From.Address.Equals(filter.From, StringComparison.InvariantCultureIgnoreCase)
                       || header.From.DisplayName.Equals(filter.From, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion

        private class StepLogger : StepLoggerBase
        {
            private readonly string hostName;

            private readonly string port;

            public StepLogger(IEtlLogger logger, EtlContext context, string stepName, string hostName, string port)
                : base(logger, context, stepName)
            {
                this.hostName = hostName;
                this.port = port;
            }

            public void LogComplete()
            {
                this.LogInfo(string.Format("Соединение с сервером {0}:{1} завершено", this.hostName, this.port));
            }

            public void LogError(Exception ex)
            {
                this.LogError(
                    string.Format("Ошибка взаимодействия c серверов {0}:{1}: {2}", this.hostName, this.port, ex));
            }
            
            public void ImapConnect()
            {
                this.LogInfo(string.Format("Подключение к серверу {0}:{1}", this.hostName, this.port));
            }

            public void Login(string userName)
            {
                this.LogInfo(
                    string.Format("Аутентификация \"{2}\" на сервере {0}:{1}", this.hostName, this.port, userName));
            }

            public void SelectMailbox(string mailboxName)
            {
                this.LogInfo(
                    string.Format("Выбор папки \"{2}\" на сервере {0}:{1}", this.hostName, this.port, mailboxName));
            }

            public void GetMessageHeaders(uint uid)
            {
                this.LogInfo(
                    string.Format("Получение заголовков письма {2} c сервера {0}:{1}", this.hostName, this.port, uid));
            }

            public void GetMessage(uint uid)
            {
                this.LogInfo(
                    string.Format("Получение письма {2} c сервера {0}:{1}", this.hostName, this.port, uid));
            }

            public void SaveAttachment(uint uid, string filename)
            {
                this.LogInfo(
                    string.Format("Сохранение файла {2} письма {3} c сервера {0}:{1}", this.hostName, this.port, filename, uid));
            }

            public void SaveMessageInDb(uint uid)
            {
                this.LogInfo(
                    string.Format("Сохранение в БД письма {2} c сервера {0}:{1}", this.hostName, this.port, uid));
            }

            public void MessageTrimInDb(uint uid)
            {
                this.LogInfo(
                    string.Format("Письмо {2}  c сервера {0}:{1} при сохранении обрезано", this.hostName, this.port, uid));
            }

            public void NoAttachmentsFound(uint maxUid)
            {
                this.LogInfo(string.Format("В почтовом ящике нет новых писем с необходимым аттачем (с uid больше {0})", maxUid));
            }

            public void MessagesFound(uint maxUid, int count)
            {
                this.LogInfo(
                    string.Format("Новых писем {1} (с uid больше {0})", maxUid, count));
            }

			public void FileRewrite(string fileName, uint messageUid)
			{
				this.LogInfo(string.Format("Файл ({0}) был перезаписан версией из письма ({1})", fileName, messageUid));
			}
		}
    }
}

