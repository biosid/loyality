using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Steps
{
	using System.Globalization;
	using System.Text.RegularExpressions;
	using System.Threading;

	using OpenPop.Mime;
	using OpenPop.Mime.Decode;
	using OpenPop.Mime.Header;
	using OpenPop.Pop3;

	//todo: add DownloadByteStatistics
    [Serializable]
    public sealed class EtlReceiveMailStep : EtlStep
    {
        #region Constants

        #endregion

        #region Constructors

		public EtlReceiveMailStep()
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

        private void Connect(Pop3Client popClient, EtlContext context, IEtlLogger logger)
        {
            Log(logger, context, EtlMessageType.Information, String.Format("Начато подключение к серверу {0}:{1}", this.Source.HostName, this.Source.Port));
            popClient.Connect(this.Source.HostName, this.Source.Port, this.Source.UseSSL);
            Log(logger, context, EtlMessageType.Information, String.Format("Установлено подключение к серверу {0}:{1}", this.Source.HostName, this.Source.Port));

            Log(logger, context, EtlMessageType.Information, String.Format("Начата аутентификация \"{2}\" на сервере {0}:{1}", this.Source.HostName, this.Source.Port, this.Source.Credential.UserName));
            popClient.Authenticate(this.Source.Credential.UserName, this.Source.Credential.Password);
            Log(logger, context, EtlMessageType.Information, String.Format("Завершена аутентификация \"{2}\" на сервере {0}:{1}", this.Source.HostName, this.Source.Port, this.Source.Credential.UserName));
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

			if (String.IsNullOrEmpty(this.Source.HostName))
			{
				throw new ArgumentException("Property \"Host\" of parameter \"step\" cannot be null", "step");
			}
			if (this.Source.Port < IPEndPoint.MinPort || this.Source.Port > IPEndPoint.MaxPort)
			{
				var errorMessage = String.Format("Property \"Port\" of parameter \"step\" should be greater then {0} and less then {1}", IPEndPoint.MinPort, IPEndPoint.MaxPort);
				throw new ArgumentException(errorMessage, "step");
			}
			if (String.IsNullOrEmpty(this.Source.Credential.UserName))
			{
				throw new ArgumentException("Property \"Login\" of parameter \"step\" cannot be null", "step");
			}
			if (String.IsNullOrEmpty(this.Source.Credential.Password))
			{
				throw new ArgumentException("Property \"Password\" of parameter \"step\" cannot be null", "step");
			}


            try
            {
                int messagesCount;

                using (var popClient = new Pop3Client())
                {
                    Connect(popClient, context, logger);

                    Log(
                        logger,
                        context,
                        EtlMessageType.Information,
                        String.Format(
                            "Начато получение количества писем c сервера {0}:{1}",
                            this.Source.HostName,
                            this.Source.Port));
                    messagesCount = popClient.GetMessageCount();
                    Log(
                        logger,
                        context,
                        EtlMessageType.Information,
                        String.Format(
                            "Завершено получение количества писем с сервера {0}:{1}, получено писем: {2}",
                            this.Source.HostName,
                            this.Source.Port,
                            messagesCount));
                }

                Log(
                    logger,
                    context,
                    EtlMessageType.Information,
                    String.Format(
                        "Начато получение заголовков писем c сервера {0}:{1}", this.Source.HostName, this.Source.Port));

                var startDate = DateTime.Now.Subtract(EtlTimeSpan.ToSystemTimeSpan(this.Source.timeSpan));

                for (int i = 1; i <= messagesCount; i++)
                {
                    using (var popClient = new Pop3Client())
                    {
                        Connect(popClient, context, logger);

                        var header = popClient.GetMessageHeaders(i);

                        if (header == null)
                        {
                            var errorMessage =
                                String.Format(
                                    "Не удалось получить заголовок письма номер {2} c сервера {0}:{1}",
                                    this.Source.HostName,
                                    this.Source.Port,
                                    i);
                            throw new InvalidOperationException(errorMessage);
                        }

                        var date = ToNullableDateTime(header.Date, null, null);
                        if (date == null)
                        {
                            var errorMessage =
                                String.Format(
                                    "Невозможно распознать заголовок \"Date\" имеющий значение \"{0}\", укажите \"MailDateFormat\"",
                                    header.Date);
                            throw new FormatException(errorMessage);
                        }

                        if (!FilterPassed(header, this.Source.Filters))
                        {
                            continue;
                        }

                        if (date < startDate)
                        {
                            continue;
                        }

                        Log(
                            logger,
                            context,
                            EtlMessageType.Information,
                            String.Format("Начато получение письма номер {0} от {1} за {2}", i, header.From, date));
                        var message = popClient.GetMessage(i);
                        Log(
                            logger,
                            context,
                            EtlMessageType.Information,
                            String.Format("Завершено получение письма номер {0} от {1} за {2}", i, header.From, date));

                        if (!message.Headers.From.Address.Contains(Source.HostName))
                        {
                            // игнорирование отправленных писем
                            Log(
                                logger,
                                context,
                                EtlMessageType.Information,
                                String.Format(
                                    "Начато сохранение вложений из письма номер {0} от {1} за {2}", i, header.From, date));
                            var savedCount = SaveAttachments(
                                message.FindAllAttachments(), this.Destination.FilePath, this.Source.AttachmentRegExp);
                            Log(
                                logger,
                                context,
                                EtlMessageType.Information,
                                String.Format(
                                    "Завершено сохранение вложений из письма номер {0} от {1} за {2}. Сохранено файлов: {3}",
                                    i,
                                    header.From,
                                    date,
                                    savedCount));
                        }
                    }
                }

                Log(
                    logger,
                    context,
                    EtlMessageType.Information,
                    String.Format(
                        "Завершено получение заголовков писем c сервера {0}:{1}", this.Source.HostName, this.Source.Port));
            }
            catch (Exception ex)
            {
                Log(
                    logger,
                    context,
                    EtlMessageType.Error,
                    String.Format(
                        "Ошибка взаимодействия c серверов {0}:{1}: {2}", this.Source.HostName, this.Source.Port, ex));
            }
            finally
			{
				//popClient.Disconnect();
				Log(logger, context, EtlMessageType.Information, String.Format("Соединение с сервером {0}:{1} завершено", this.Source.HostName, this.Source.Port));
			}


            return new EtlStepResult(EtlStatus.Succeeded, null);
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

		private void Log(IEtlLogger logger, EtlContext context, EtlMessageType type, string message)
		{
			logger.LogEtlMessage(new EtlMessage
			{
				EtlPackageId = context.EtlPackageId,
				EtlSessionId = context.EtlSessionId,
				LogDateTime = DateTime.Now,
				LogUtcDateTime = DateTime.UtcNow,
				MessageType = type,
				Text = message,
				EtlStepName = this.Name,
			});
		}

        #endregion
    }
}
