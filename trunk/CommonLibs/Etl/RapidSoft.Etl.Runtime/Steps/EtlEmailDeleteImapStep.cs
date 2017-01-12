namespace RapidSoft.Etl.Runtime.Steps
{
    using System;
    using System.ComponentModel;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime.DataSources.DB;

    using S22.Imap;

    [Serializable]
    public class EtlEmailDeleteImapStep : EtlStep
    {
        #region Constructors

        public EtlEmailDeleteImapStep()
        {
        }

        #endregion

        #region Properties

        [Category("3. Destination")]
        public EtlEmailServer EmailServer { get; set; }

        [Category("2. Source")]
        public EtlEmailDbStorage EmailDbStorage { get; set; }

        [Category("4. Destination")]
        public string MailBox { get; set; }

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

            var stepLogger = new StepLogger(logger, context, this.Name, this.EmailServer.Host, this.EmailServer.Port);

            try
            {
                stepLogger.ImapConnect();
                using (var ds = new MailDataSource(this.EmailDbStorage.ConnectionString, this.EmailDbStorage.SchemaName))
                {
                    var messagesUid = ds.GetIncomingMessagesUid(context.EtlSessionId);
                    
                    if (messagesUid == null || messagesUid.Count == 0)
                    {
                        stepLogger.MailNotFound();
						return new EtlStepResult(EtlStatus.Succeeded, null);
					}

                    using (var client = new ImapClient(this.EmailServer.Host, this.EmailServer.IntPort, this.EmailServer.BoolUseSSL))
                    {
                        var userName = this.EmailServer.UserName;
                        stepLogger.Login(userName);
                        client.Login(userName, this.EmailServer.Password, AuthMethod.Auto);

                        MailBox = MailBox ?? "inbox";

                        foreach (var uid in messagesUid)
                        {
                            stepLogger.Delete(uid);
                            client.DeleteMessage(uid, MailBox);
                            ds.MarkIncomingMessageAsDeleted(context.EtlSessionId, uid);
                        }

                        stepLogger.LogComplete();
                    }
                }
            }
            catch (Exception ex)
            {
                stepLogger.LogError(ex);
                throw;
            }

			return new EtlStepResult(EtlStatus.Succeeded, null);
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

            public void LogError(Exception ex)
            {
                this.LogError(
                    string.Format("Ошибка взаимодействия c серверов {0}:{1}: {2}", this.hostName, this.port, ex));
            }

            public void LogComplete()
            {
                this.LogInfo(string.Format("Соединение с сервером {0}:{1} завершено", this.hostName, this.port));
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

            public void MailNotFound()
            {
                this.LogInfo(string.Format("Письма не найдены, удалять нечего"));
            }

            public void Delete(uint uid)
            {
                this.LogInfo(
                    string.Format(
                        "Удаление письма {0} c сервера {1}:{2}", uid, this.hostName, this.port));
            }
        }
    }
}