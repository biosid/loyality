using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RapidSoft.VTB24.BankConnector.Tests.Helpers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Linq.Expressions;

    using RapidSoft.Etl.Runtime.Steps;
    using RapidSoft.VTB24.BankConnector.Configuration;

    using S22.Imap;

    public static class EmailHelper
    {        
        public static string DownloadFilesFromTeradata(string subject, string tempFolder)
        {
            return DownloadFilesFromImapServer(subject, tempFolder, GetTeradataImapSettings());
        }

        public static void DownloadFilesFromLoyalty(string subject, string tempFolder)
        {
            DownloadFilesFromImapServer(subject, tempFolder, GetLoyaltyImapSettings());
        }

        private static string DownloadFilesFromImapServer(string subject, string tempFolder, EtlEmailServer imapSettings)
        {
            const int AttemptsCount = 5;
            var attemptsCounter = 0;

            while (attemptsCounter++ < AttemptsCount)
            {
                var filePath = TryDownloadFilesFromImapServer(subject, tempFolder, imapSettings);
                if (filePath != null)
                {
                    return filePath;
                }

                Thread.Sleep(200);
            }

            return null;
        }

        private static string TryDownloadFilesFromImapServer(string subject, string tempFolder, EtlEmailServer imapSettings)
        {
            var useSsl = imapSettings.UseSSL != null && Convert.ToBoolean(imapSettings.UseSSL);

            using (var client = new ImapClient(imapSettings.Host, Convert.ToInt32(imapSettings.Port), useSsl))
            {
                client.Login(imapSettings.UserName, imapSettings.Password, AuthMethod.Auto);
                client.DefaultMailbox = "inbox";

                var messagesUid = client.Search(SearchCondition.Undeleted());

                ////client.ListMailboxes()
                ////      .ToList()
                ////      .ForEach(mb => messagesUid.AddRange(client.Search(SearchCondition.Undeleted(), mb)));

                foreach (var uid in messagesUid)
                {
                    var header = client.GetMessage(uid, FetchOptions.HeadersOnly);

                    if (header == null)
                    {
                        var errorMessage =
                            string.Format(
                                "Не удалось получить заголовок письма номер {2} c сервера {0}:{1}",
                                imapSettings.Host,
                                imapSettings.Port,
                                uid);
                        throw new InvalidOperationException(errorMessage);
                    }

                    if (!header.Subject.Contains(subject))
                    {
                        ////Console.WriteLine(
                        ////    "Письмо было исключено из обработки, header ({0}), messageUid({1})", header.Subject, uid);
                        continue;
                    }

                    ////Console.WriteLine(
                    ////    "Обработка письма с подходящим заголовком ({0}), messageUid({1})", header.Subject, uid);

                    var message = client.GetMessage(uid);
                    foreach (var attachment in message.Attachments)
                    {
                        ////Console.WriteLine(
                        ////    "Обработка вложения ({0})", attachment.Name);
						var fileName = Path.Combine(tempFolder, attachment.Name);

                        if (!Directory.Exists(tempFolder))
                        {
                            Directory.CreateDirectory(tempFolder);
                        }

                        Console.WriteLine("Сохранение вложения письма ({0}) в файл ({1})", message.Subject, fileName);
						using (Stream file = File.OpenWrite(fileName))
                        {
                            var buffer = new byte[8 * 1024];
                            int len;
                            while ((len = attachment.ContentStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                file.Write(buffer, 0, len);
                            }
                        }
                        return fileName;
                    }
                }
            }

            return null;
        }

        public static void UploadFileToTeradataSmtpServer(string filePath, string subject)
        {
            UploadFileToSmtpServer(filePath, subject, EtlConfigSection.GetEtlVar("MailToTeradataTo"), EtlConfigSection.GetEtlVar("MailToTeradataTo"));
        }

        public static void UploadFileToLoyaltySmtpServer(string filePath, string subject)
        {
            UploadFileToSmtpServer(filePath, subject, EtlConfigSection.GetEtlVar("MailToLoyaltyFrom"), EtlConfigSection.GetEtlVar("MailToLoyaltyTo"));
        }

        private static void UploadFileToSmtpServer(string filePath, string subject, string @from, string to)
        {
            var message = new MailMessage
                          {
                              Subject = subject
                          };

            if (filePath != null)
            {
            message.Attachments.Add(new Attachment(filePath));
            }
            
            SendEmail(message, from, to);
        }

        public static void CleanupTeradataMailBox(string subject)
        {
            CleanupMailBoxImap(subject, GetTeradataImapSettings());
        }

        public static void CleanupLoyaltyMailBox(string subject)
        {
            CleanupMailBoxImap(subject, GetLoyaltyImapSettings());
        }

        private static void CleanupMailBoxImap(string subject, EtlEmailServer imapSettings)
        {
            using (var client = new ImapClient(imapSettings.Host, imapSettings.IntPort, imapSettings.BoolUseSSL))
            {
                client.Login(imapSettings.UserName, imapSettings.Password, AuthMethod.Auto);
                //client.DefaultMailbox = "inbox";

                var messagesUid = new List<uint>();

                client.ListMailboxes()
                      .ToList()
                      .ForEach(mb => messagesUid.AddRange(client.Search(SearchCondition.Undeleted(), mb)));

                foreach (var uid in messagesUid)
                {
                    if (string.IsNullOrEmpty(subject))
                    {
                        client.DeleteMessage(uid);
                        continue;
                    }

                    var header = client.GetMessage(uid, FetchOptions.HeadersOnly);

                    if (header == null)
                    {
                        continue;
                    }

                    if (header.Subject.Contains(subject))
                    {
                        client.DeleteMessage(uid);
                    }
                }
            }
        }

        private static void SendEmail(MailMessage message, string @from, string to)
        {
            using (var client = new SmtpClient())
            {
                client.Host = EtlConfigSection.GetEtlVar("SmtpHost");
                client.Port = int.Parse(EtlConfigSection.GetEtlVar("SmtpPort"));
                client.EnableSsl = false;
                var userName = EtlConfigSection.GetEtlVar("SmtpUserName");
                client.Credentials = new NetworkCredential(userName, EtlConfigSection.GetEtlVar("SmtpPassword"));

                message.From = new MailAddress(@from);
                message.Sender = new MailAddress(userName);

                message.To.Add(to);

                client.Send(message);
            }
        }

        public static EtlEmailServer GetTeradataImapSettings()
        {
            return new EtlEmailServer
            {
                Host = EtlConfigSection.GetEtlVar("TeradataImapHost"),
                Port = EtlConfigSection.GetEtlVar("TeradataImapPort"),
                UseSSL = EtlConfigSection.GetEtlVarOrDefault("TeradataImapUseSSL"),
                UserName = EtlConfigSection.GetEtlVar("TeradataImapUserName"),
                Password = EtlConfigSection.GetEtlVar("TeradataImapPassword"),
            };
        }

        public static EtlEmailServer GetLoyaltyImapSettings()
        {
            return new EtlEmailServer
            {
                Host = EtlConfigSection.GetEtlVar("LoyaltyImapHost"),
                Port = EtlConfigSection.GetEtlVar("LoyaltyImapPort"),
                UseSSL = EtlConfigSection.GetEtlVarOrDefault("LoyaltyImapUseSSL"),
                UserName = EtlConfigSection.GetEtlVar("LoyaltyImapUserName"),
                Password = EtlConfigSection.GetEtlVar("LoyaltyImapPassword"),
            };
        }

    }
}
