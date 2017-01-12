using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Steps
{
    using System.Data.Common;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    using OpenPop.Mime;
    using OpenPop.Mime.Decode;
    using OpenPop.Mime.Header;
    using OpenPop.Pop3;

    //todo: add DownloadByteStatistics
    [Serializable]
    public sealed class EtlReceiveMailPopStep : EtlStep
    {
        #region Constructors

        public EtlReceiveMailPopStep()
        {
        }

        #endregion

        #region Properties

        [Category("2. Source")]
        public EtlEmailResourceInfo Source
        {
            get;
            set;
        }

        [Category("3. Destination")]
        public EtlFileInfo Destination
        {
            get;
            set;
        }

        #endregion

        #region Methods

        private DateTime? senseDateTime = null;

        private DateTime GetSinseDateTime()
        {
            if (!this.senseDateTime.HasValue)
            {
                this.senseDateTime = DateTime.Now.Subtract(EtlTimeSpan.ToSystemTimeSpan(this.Source.timeSpan));
            }

            return this.senseDateTime.Value;
        }

        private void Connect(Pop3Client popClient, StepLogger stepLogger)
        {
            stepLogger.LogInfo(string.Format("Начато подключение к серверу {0}:{1}", this.Source.HostName, this.Source.Port));
            popClient.Connect(this.Source.HostName, this.Source.Port, this.Source.UseSSL);
            stepLogger.LogInfo(
                string.Format("Установлено подключение к серверу {0}:{1}", this.Source.HostName, this.Source.Port));

            stepLogger.LogInfo(
                string.Format(
                    "Начата аутентификация \"{2}\" на сервере {0}:{1}",
                    this.Source.HostName,
                    this.Source.Port,
                    this.Source.Credential.UserName));
            popClient.Authenticate(this.Source.Credential.UserName, this.Source.Credential.Password);
            stepLogger.LogInfo(
                string.Format(
                    "Завершена аутентификация \"{2}\" на сервере {0}:{1}",
                    this.Source.HostName,
                    this.Source.Port,
                    this.Source.Credential.UserName));
        }

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

            if (string.IsNullOrEmpty(this.Source.HostName))
            {
                throw new ArgumentException("Property \"Host\" of parameter \"step\" cannot be null", "step");
            }

            if (this.Source.Port < IPEndPoint.MinPort || this.Source.Port > IPEndPoint.MaxPort)
            {
                var errorMessage = string.Format("Property \"Port\" of parameter \"step\" should be greater then {0} and less then {1}", IPEndPoint.MinPort, IPEndPoint.MaxPort);
                throw new ArgumentException(errorMessage, "step");
            }

            if (string.IsNullOrEmpty(this.Source.Credential.UserName))
            {
                throw new ArgumentException("Property \"Login\" of parameter \"step\" cannot be null", "step");
            }

            if (string.IsNullOrEmpty(this.Source.Credential.Password))
            {
                throw new ArgumentException("Property \"Password\" of parameter \"step\" cannot be null", "step");
            }

            var stepLogger = new StepLogger(logger, context, this.Name, this.Source.HostName, this.Source.Port);

            try
            {
                int messagesCount;

                using (var popClient = new Pop3Client())
                {
                    this.Connect(popClient, stepLogger);

                    stepLogger.BeginGetMessageCount();
                    messagesCount = popClient.GetMessageCount();
                    stepLogger.EndGetMessageCount(messagesCount);
                }

                for (var i = 1; i <= messagesCount; i++)
                {
                    this.ProcessMessage(i, stepLogger);
                }
            }
            catch (Exception ex)
            {
                stepLogger.LogError(ex);
            }
            finally
            {
                stepLogger.LogComplete();
            }

            return new EtlStepResult(EtlStatus.Succeeded, null);
        }

        private void ProcessMessage(int messageNumber, StepLogger stepLogger)
        {
            using (var popClient = new Pop3Client())
            {
                this.Connect(popClient, stepLogger);

                stepLogger.BeginGetMessageHeaders();
                var header = popClient.GetMessageHeaders(messageNumber);
                stepLogger.EndGetMessageHeaders();
                
                if (header == null)
                {
                    var errorMessage = string.Format(
                        "Не удалось получить заголовок письма номер {2} c сервера {0}:{1}",
                        this.Source.HostName,
                        this.Source.Port,
                        messageNumber);
                    throw new InvalidOperationException(errorMessage);
                }

                if (this.CheckMessageHeaders(header))
                {
                    return;
                }

                stepLogger.BeginGetMessage(messageNumber, header);
                Message message = popClient.GetMessage(messageNumber);
                var str = Encoding.Default.GetString(message.RawMessage);
                stepLogger.EndGetMessage(messageNumber, header);

                stepLogger.BeginSaveAttachments(messageNumber, header);
                var savedCount = SaveAttachments(
                    message.FindAllAttachments(), this.Destination.FilePath, this.Source.AttachmentRegExp);
                stepLogger.EndSaveAttachments(messageNumber, header, savedCount);

            }
        }

        private bool CheckMessageHeaders(MessageHeader header)
        {
            var date = ToNullableDateTime(header.Date, null, null);
            if (date == null)
            {
                var errorMessage =
                    string.Format(
                        "Невозможно распознать заголовок \"Date\" имеющий значение \"{0}\", укажите \"MailDateFormat\"",
                        header.Date);
                throw new FormatException(errorMessage);
            }

            // игнорирование отправленных писем
            if (header.From.Address.Contains(this.Source.HostName))
            {
                return true;
            }

            if (!FilterPassed(header, this.Source.Filters))
            {
                return true;
            }

            if (date < this.GetSinseDateTime())
            {
                return true;
            }

            return false;
        }

        private static DateTime? ToNullableDateTime(string s, string format, string cultureName)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            s = s.Trim();
            DateTime dateTime;
            if (string.IsNullOrEmpty(format))
            {
                if (string.IsNullOrEmpty(cultureName))
                {
                    if (DateTime.TryParse(s, out dateTime))
                    {
                        return dateTime;
                    }
                }
                else
                {
                    if (DateTime.TryParse(s, new CultureInfo(cultureName), DateTimeStyles.None, out dateTime))
                    {
                        return dateTime;
                    }
                }

                return Rfc2822DateTime.StringToDate(s);
            }

            var culture = !string.IsNullOrEmpty(cultureName) ? new CultureInfo(cultureName) : Thread.CurrentThread.CurrentCulture;
            return DateTime.TryParseExact(s, format, culture, DateTimeStyles.None, out dateTime) ? dateTime : (DateTime?)null;
        }

        private static int SaveAttachments(List<MessagePart> attachments, string attachmentsPath, string fileNameMask)
        {
            var count = 0;
            foreach (var attachment in attachments)
            {
                if (string.IsNullOrEmpty(fileNameMask) || Regex.IsMatch(attachment.FileName, fileNameMask))
                {
                    attachment.Save(new FileInfo(Path.Combine(attachmentsPath, attachment.FileName)));
                    count++;
                }
            }

            return count;
        }

        private static bool FilterPassed(MessageHeader header, List<EtlReceiveMailFilter> filters)
        {
            /*if (filters.Count == 0)
            {
                return true;
            }*/
            // логика недостаточно гибкая, например, если требуется от одного отправителя обрабатывать все письма, а от другого - с учетом заголовка
            /*var fromPassed = filters.Any(f => string.IsNullOrWhiteSpace(f.From) || (headers.From != null && headers.From.Address == f.From));
            var subjectContainsPassed = filters.Any(f => string.IsNullOrWhiteSpace(f.SubjectContains) || (headers.Subject != null && headers.Subject.Contains(f.SubjectContains)));
            var subjectStartsWithPassed = filters.Any(f => string.IsNullOrWhiteSpace(f.SubjectStartsWith) || (headers.Subject != null && headers.Subject.StartsWith(f.SubjectStartsWith)));
             */

            return filters == null || filters.Count == 0
                   || filters.Exists(
                       x =>
                       (string.IsNullOrEmpty(x.From) || (header.From != null && header.From.Address == x.From))
                       && (string.IsNullOrEmpty(x.SubjectContains)
                           || (header.Subject != null && header.Subject.Contains(x.SubjectContains)))
                       && (string.IsNullOrEmpty(x.SubjectStartsWith)
                           || (header.Subject != null && header.Subject.StartsWith(x.SubjectStartsWith))));
        }

        #endregion

        internal class StepLogger
        {
            private readonly IEtlLogger logger;

            private readonly EtlContext context;

            private readonly string stepName;

            private readonly string hostName;

            private readonly int port;

            public StepLogger(IEtlLogger logger, EtlContext context, string stepName, string hostName, int port)
            {
                this.logger = logger;
                this.context = context;
                this.stepName = stepName;
                this.hostName = hostName;
                this.port = port;
            }

            public void BeginGetMessage(int messageNumber, MessageHeader header)
            {
                this.LogInfo(
                    string.Format(
                        "Начато получение письма номер {0} от {1} за {2}", messageNumber, header.From, header.Date));
            }

            public void EndGetMessage(int messageNumber, MessageHeader header)
            {
                this.LogInfo(
                    string.Format(
                        "Завершено получение письма номер {0} от {1} за {2}", messageNumber, header.From, header.Date));
            }

            public void BeginSaveAttachments(int messageNumber, MessageHeader header)
            {
                this.LogInfo(
                    string.Format(
                        "Начато сохранение вложений из письма номер {0} от {1} за {2}",
                        messageNumber,
                        header.From,
                        header.Date));
            }

            public void EndSaveAttachments(int messageNumber, MessageHeader header, int savedCount)
            {
                this.LogInfo(
                    string.Format(
                        "Завершено сохранение вложений из письма номер {0} от {1} за {2}. Сохранено файлов: {3}",
                        messageNumber,
                        header.From,
                        header.Date,
                        savedCount));
            }

            public void BeginGetMessageHeaders()
            {
                this.LogInfo(
                    string.Format("Начато получение заголовков писем c сервера {0}:{1}", this.hostName, this.port));
            }

            public void EndGetMessageHeaders()
            {
                this.LogInfo(
                    string.Format("Завершено получение заголовков писем c сервера {0}:{1}", this.hostName, this.port));
            }

            public void BeginGetMessageCount()
            {
                this.LogInfo(
                    string.Format("Начато получение количества писем c сервера {0}:{1}", this.hostName, this.port));
            }

            public void EndGetMessageCount(int messagesCount)
            {
                this.LogInfo(
                    string.Format(
                        "Завершено получение количества писем с сервера {0}:{1}, получено писем: {2}",
                        this.hostName,
                        this.port,
                        messagesCount));
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

            public void LogError(string message)
            {
                this.Log(EtlMessageType.Error, message);
            }

            public void LogInfo(string message)
            {
                this.Log(EtlMessageType.Information, message);
            }

            public void Log(EtlMessageType type, string message)
            {
                var mess = new EtlMessage
                               {
                                   EtlPackageId = this.context.EtlPackageId,
                                   EtlSessionId = this.context.EtlSessionId,
                                   LogDateTime = DateTime.Now,
                                   LogUtcDateTime = DateTime.UtcNow,
                                   MessageType = type,
                                   Text = message,
                                   EtlStepName = this.stepName,
                               };

                this.logger.LogEtlMessage(mess);
            }
        }
    }

    
}


