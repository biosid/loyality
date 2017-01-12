namespace RapidSoft.Loaylty.ProductCatalog.OrdersNotifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime;
    using RapidSoft.Loaylty.ProductCatalog.Configuration;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Entities;
    using RapidSoft.Loaylty.ProductCatalog.OrdersNotifications.Templates;

    public class OrdersNotificationsSteps
    {
        private readonly EtlContext etlContext;
        private readonly IEtlLogger etlLogger;
        private readonly IOrdersNotificationsRepository ordersNotificationsRepository;
        private readonly IPartnerRepository partnerRepository;

        private OrdersNotificationsSteps(
            EtlContext etlContext,
            IEtlLogger etlLogger,
            IOrdersNotificationsRepository ordersNotificationsRepository = null,
            IPartnerRepository partnerRepository = null)
        {
            this.etlContext = etlContext;
            this.etlLogger = etlLogger;
            this.ordersNotificationsRepository = ordersNotificationsRepository ?? new OrdersNotificationsRepository();
            this.partnerRepository = partnerRepository ?? new PartnerRepository();
        }

        private string EtlSessionId
        {
            get { return etlContext.EtlSessionId; }
        }

        #region Шаги

        public static void ComposeEmails(EtlContext context, IEtlLogger logger)
        {
            var instance = new OrdersNotificationsSteps(context, logger);

            instance.ExecuteComposeEmails();
        }

        public static void SendEmails(EtlContext context, IEtlLogger logger)
        {
            var instance = new OrdersNotificationsSteps(context, logger);

            instance.ExecuteSendEmails();
        }

        #endregion

        #region Шаг для создания писем

        private static string ComposeEmailSubject(string partnerName)
        {
            return string.Format("Нотификации по заказам партнёра \"{0}\"", partnerName);
        }

        private static string ComposeEmailBody(string partnerName, IEnumerable<OrderNotification> ordersNotifications)
        {
            var body = new OrdersNotificationsEmailBody
            {
                PartnerName = partnerName,
                OrdersNotifications = ordersNotifications
            };

            return body.TransformText();
        }

        private void ExecuteComposeEmails()
        {
            var ordersNotifications = ordersNotificationsRepository.GetOrdersNotifications(EtlSessionId);

            var emptyOrderIds = ordersNotifications.Where(n => n.Items == null || n.Items.Length == 0)
                                                   .Select(n => n.OrderId)
                                                   .ToArray();

            if (emptyOrderIds.Length > 0)
            {
                LogWarning("Обнаружено {0} заказов, которые не содержат продуктов. (id: {1})", emptyOrderIds.Length, string.Join(", ", emptyOrderIds));

                ordersNotifications = ordersNotifications.Where(n => n.Items != null && n.Items.Length > 0).ToArray();
            }

            var emails = ComposeEmails(ordersNotifications).ToArray();

            ordersNotificationsRepository.SaveEmails(emails);
        }

        private IEnumerable<OrdersNotificationsEmail> ComposeEmails(IEnumerable<OrderNotification> ordersNotifications)
        {
            var totalNotificationsCount = 0;
            var emailsCount = 0;

            foreach (var group in ordersNotifications.GroupBy(on => on.PartnerId))
            {
                var notificationsCount = group.Count();
                totalNotificationsCount += notificationsCount;

                var partnerId = group.Key;

                var partner = partnerRepository.GetById(partnerId);

                if (partner == null)
                {
                    LogError("не удалось найти партнера (id = {0})", partnerId);
                    continue;
                }

                var recipients = GetRecipientsListForPartner(partnerId);
                if (recipients == null)
                {
                    continue;
                }

                emailsCount++;

                LogCounter("Нотификации", string.Format("по партнеру {0}", partnerId), notificationsCount);

                yield return new OrdersNotificationsEmail
                {
                    EtlSessionId = EtlSessionId,
                    Recipients = recipients,
                    Subject = ComposeEmailSubject(partner.Name),
                    Body = ComposeEmailBody(partner.Name, group),
                    Status = OrdersNotificationsEmailStatus.ReadyToSend
                };
            }

            LogCounter("Нотификации", string.Format("всего"), totalNotificationsCount);
            LogCounter("Письма", string.Format("создано"), emailsCount);
        }

        private string GetRecipientsListForPartner(int partnerId)
        {
            var settingName = ConfigHelper.OrdersNotificationsRecipientsSettingName;

            var recipientsSetting = partnerRepository.GetSettings(partnerId)
                                                     .FirstOrDefault(s => s.Key == settingName);

            if (recipientsSetting == null)
            {
                LogWarning("список получателей нотификаций по заказам партнера не найден (PartnerId = {0})", partnerId);
                return null;
            }

            var recipients = recipientsSetting.Value;

            if (string.IsNullOrWhiteSpace(recipients))
            {
                LogWarning("пустой список получателей нотификаций по заказам партнера (PartnerId = {0})", partnerId);
                return null;
            }

            return recipients;
        }

        #endregion

        #region Шаг для отправки писем

        private void ExecuteSendEmails()
        {
            var emails = ordersNotificationsRepository.GetOrdersNotificationsEmails(EtlSessionId);

            var client = new SmtpClient();

            var sentEmailsCount = emails.Count(email => SendEmail(client, email));

            LogCounter("Письма", "готово к отправке", emails.Length);
            LogCounter("Письма", "успешно отправлено", sentEmailsCount);
        }

        private bool SendEmail(SmtpClient client, OrdersNotificationsEmail email)
        {
            var message = new MailMessage
            {
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8
            };
            message.To.Add(email.Recipients);

            var success = true;
            try
            {
                client.Send(message);
            }
            catch (Exception e)
            {
                success = false;
                LogError("При отправке письма произошла ошибка (EmailId = {0}): {1}", email.Id, e);
            }

            ordersNotificationsRepository.UpdateEmailStatus(email.Id, success ? OrdersNotificationsEmailStatus.Sent : OrdersNotificationsEmailStatus.Error);

            return success;
        }

        #endregion

        private void LogCounter(string entityName, string counterName, long counterValue)
        {
            var now = DateTime.Now;
            var counter = new EtlCounter
            {
                DateTime = now,
                UtcDateTime = now.ToUniversalTime(),
                EtlPackageId = etlContext.EtlPackageId,
                EtlSessionId = etlContext.EtlSessionId,
                EntityName = entityName,
                CounterName = counterName,
                CounterValue = counterValue
            };
            etlLogger.LogEtlCounter(counter);
        }

        private void LogMessage(EtlMessageType type, string format, params object[] args)
        {
            var now = DateTime.Now;
            var message = new EtlMessage
            {
                EtlPackageId = etlContext.EtlPackageId,
                EtlSessionId = etlContext.EtlSessionId,
                LogDateTime = now,
                LogUtcDateTime = now.ToUniversalTime(),
                MessageType = type,
                Text = string.Format(format, args)
            };
            etlLogger.LogEtlMessage(message);
        }

        private void LogWarning(string format, params object[] args)
        {
            LogMessage(EtlMessageType.Warning, format, args);
        }

        private void LogError(string format, params object[] args)
        {
            LogMessage(EtlMessageType.Error, format, args);
        }
    }
}