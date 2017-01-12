using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using RapidSoft.Loaylty.Logging;
using RapidSoft.Loaylty.PartnersConnector.Litres.DataAccess.Repositories;
using RapidSoft.Loaylty.PartnersConnector.Litres.Models;
using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

using Order = RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities.Order;
using OrderItem = RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities.OrderItem;

namespace RapidSoft.Loaylty.PartnersConnector.Litres
{
    public class LitresOrderManagement
    {
        private static readonly int MinRemainingCodesCount = Configuration.MinRemainingCodesCount;

        private readonly ILog _log = LogManager.GetLogger(typeof(LitresOrderManagement));

        public void CommitOrder(Order order)
        {
            if (!Configuration.LitresPartnerId.HasValue)
            {
                throw new InvalidOperationException("ID партнера \"Литрес\" не задан в конфигурации");
            }

            var partnerId = Configuration.LitresPartnerId.Value;

            // проверить ID партнера
            if (order.PartnerId != partnerId)
            {
                throw new InvalidOperationException("метод предназначен только для заказов партнера \"Литрес\"");
            }

            // получить PartnerProductId и кол-во для каждой позиции
            var bindings = order.Items.Select(ToDownloadCodeBinding).ToArray();

            // привязать коды
            BindCodes(bindings, order.Id);

            // проверить количество оставшихся кодов
            CheckRemainingCount(bindings, partnerId);

            // сформировать текст "как получить вознаграждение"
            var instructions = CreateInstructions(bindings);

            // добавить в заказ текст "как получить вознаграждение"
            SaveInstructions(order.Id, instructions);
        }

        private static DownloadCodeBinding ToDownloadCodeBinding(OrderItem item)
        {
            if (item == null)
            {
                throw new ArgumentException("позиция заказа не может быть null", "item");
            }

            return new DownloadCodeBinding
            {
                PartnerProductId = item.OfferId,
                Count = item.Amount
            };
        }

        private void BindCodes(IEnumerable<DownloadCodeBinding> bindings, int orderId)
        {
            var repository = new LitresDownloadCodesRepository();

            try
            {
                foreach (var binding in bindings)
                {
                    binding.Codes = repository.BindCodes(binding.PartnerProductId, binding.Count, orderId);
                }

                repository.Save();
            }
            catch (Exception e)
            {
                _log.Error("не удалось привязать код(ы), orderId = " + orderId.ToString("D"), e);
                throw;
            }
            finally
            {
                repository.Dispose();
            }
        }

        private void CheckRemainingCount(IEnumerable<DownloadCodeBinding> bindings, int partnerId)
        {
            var repository = new LitresDownloadCodesRepository();

            var partnerProductIdsToDeactivate = new List<string>();
            var lowCodesCountWarnings = new List<string>();

            try
            {
                foreach (var partnerProductId in bindings.Select(b => b.PartnerProductId))
                {
                    var remainingCount = repository.GetRemainingCount(partnerProductId);

                    if (remainingCount == 0)
                    {
                        partnerProductIdsToDeactivate.Add(partnerProductId);
                    }

                    if (remainingCount <= MinRemainingCodesCount)
                    {
                        lowCodesCountWarnings.Add(FormatLowCodesCountWarning(partnerProductId, remainingCount));
                    }
                }

                if (partnerProductIdsToDeactivate.Count > 0)
                {
                    DeactivateProducts(partnerProductIdsToDeactivate, partnerId);
                }

                if (lowCodesCountWarnings.Count > 0)
                {
                    SendWarningOnLowCodesCount(lowCodesCountWarnings);
                }
            }
            catch (Exception e)
            {
                _log.Error("ошибка при проверке количества оставшихся кодов Литрес", e);
            }
            finally
            {
                repository.Dispose();
            }
        }

        private void DeactivateProducts(IEnumerable<string> partnerProductIds, int partnerId)
        {
            var ids = partnerProductIds.ToArray();

            var service = new CatalogAdminServiceClient();

            try
            {
                var parameters = new ChangeStatusByPartnerParameters
                {
                    UserId = Configuration.CatalogAdminUserId,
                    PartnerId = partnerId,
                    ProductStatus = ProductStatuses.NotActive,
                    PartnerProductIds = ids
                };

                var response = service.ChangeProductsStatusByPartner(parameters);

                if (!response.Success)
                {
                    _log.Error(
                        "не удалось деактивировать вознаграждения Литрес из-за отсутствия кодов, partnerProductIds = " +
                        string.Join(", ", ids) + ": " + response.ResultDescription);
                }
            }
            catch (Exception e)
            {
                _log.Error(
                    "ошибка при деактивации вознаграждения Литрес из-за отсутствия кодов, partnerPrpoductIds = " +
                    string.Join(", ", ids), e);
            }
            finally
            {
                ((IDisposable)service).Dispose();
            }
        }

        private static string FormatLowCodesCountWarning(string partnerProductId, int remainingCount)
        {
            return string.Format("Для вознаграждения с артикулом {0} осталось мало кодов скачивания: {1}",
                                 partnerProductId, remainingCount);
        }

        private void SendWarningOnLowCodesCount(IEnumerable<string> warnings)
        {
            const string SUBJECT = "Заканчиваются коды для вознаграждений Литрес";

            var body = string.Join(Environment.NewLine, warnings);

            var client = new SmtpClient();

            try
            {
                var message = new MailMessage
                {
                    Subject = SUBJECT,
                    Body = body,
                    IsBodyHtml = false,
                    BodyEncoding = Encoding.UTF8
                };
                message.To.Add(Configuration.LowCodesCountWarningTo);

                client.Send(message);
            }
            catch (Exception e)
            {
                _log.Error("ошибка при отправке предупреждения о малом остатке кодов Литрес", e);
            }
            finally
            {
                client.Dispose();
            }
        }

        private static string CreateInstructions(IEnumerable<DownloadCodeBinding> bindings)
        {
            var codes = string.Join(Environment.NewLine, bindings.SelectMany(b => b.Codes));

            var instructions = codes.Length > 1
                                   ? "Ваш личный код для загрузки вознаграждения: "
                                   : "Ваши личные коды для загрузки вознаграждений: " + Environment.NewLine;

            instructions += codes;

            instructions += Environment.NewLine + Environment.NewLine + "Для скачивания перейдите по ссылке http://www.litres.ru/vtb24";

            return instructions;
        }

        private void SaveInstructions(int orderId, string instructions)
        {
            var service = new CatalogAdminServiceClient();

            try
            {
                var response = service.ChangeOrderDeliveryInstructions(Configuration.CatalogAdminUserId, orderId, instructions);

                if (!response.Success)
                {
                    throw new Exception("не удалось сохранить инструкции по получению заказа, orderId = " + orderId.ToString("D") + ": " + response.ResultDescription);
                }
            }
            catch (Exception e)
            {
                _log.Error("ошибка при сохранении инструкции по получению заказа, orderId = " + orderId.ToString("D"), e);
                throw;
            }
            finally
            {
                ((IDisposable)service).Dispose();
            }
        }
    }
}
