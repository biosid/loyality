namespace RapidSoft.VTB24.BankConnector.EtlExecutionWrapper.Email
{
    using System;

    using RapidSoft.Etl.Runtime.Steps;

    using S22.Imap;

    internal class EmailSearcher
    {
        private readonly EtlEmailServer imapSettings;

        public EmailSearcher(EtlEmailServer imapSettings)
        {
            this.imapSettings = imapSettings;
        }

        public EmailMessage Find(string subjectTerm)
        {
            var useSsl = this.imapSettings.UseSSL != null && Convert.ToBoolean(this.imapSettings.UseSSL);

            using (var client = new ImapClient(this.imapSettings.Host, Convert.ToInt32(this.imapSettings.Port), useSsl))
            {
                client.Login(this.imapSettings.UserName, this.imapSettings.Password, AuthMethod.Auto);
                client.DefaultMailbox = "inbox";

                var messagesUid = client.Search(SearchCondition.Undeleted());

                foreach (var uid in messagesUid)
                {
                    var mailMessage = client.GetMessage(uid, FetchOptions.HeadersOnly);

                    if (mailMessage == null)
                    {
                        var errorMessage =
                            string.Format(
                                "Не удалось получить заголовок письма номер {2} c сервера {0}:{1}",
                                this.imapSettings.Host,
                                this.imapSettings.Port,
                                uid);
                        throw new InvalidOperationException(errorMessage);
                    }

                    if (mailMessage.Subject.Contains(subjectTerm))
                    {
                        return new EmailMessage()
                               {
                                   Header = mailMessage.Subject
                               };
                    }
                }
            }

            return null;
        }
    }
}