namespace RapidSoft.Loaylty.PartnersConnector.Services.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using System.Text;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;
    using RapidSoft.Loaylty.ProductCatalog.WsClients;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

    using ImportProductsFromYmlResult = RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities.ImportProductsFromYmlResult;
    using PartnerSettings = RapidSoft.Loaylty.PartnersConnector.Interfaces.Settings.PartnerSettings;
    using ResultBase = RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService.ResultBase;
    using WsPartnerSettings = RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService.PartnerSettings;

    public class CatalogAdminServiceProvider : ICatalogAdminServiceProvider
    {
        private static readonly int Timeout;
        private static readonly object lockObj = new object();
        private static readonly string CacheTimeout = ConfigurationManager.AppSettings["PartnersSettingsCacheTimeoutSec"];

        private static readonly string UserId = ConfigurationManager.AppSettings["VtbSystemUserName"];

        private static IEnumerable<WsPartnerSettings> cachedPartnerSettings;
        private static DateTime lastPartnerSettingsLoad;

        private readonly ILog log = LogManager.GetLogger(typeof (CatalogAdminServiceProvider));

        static CatalogAdminServiceProvider()
        {
            if (!int.TryParse(CacheTimeout, out Timeout))
            {
                Timeout = 0;
            }
        }

        public SearchProductsResult SearchAllProducts(string userId = null, string[] productsIds = null)
        {
            var innerUserId = userId ?? UserId;

            var parameters = new AdminSearchProductsParameters
            {
                UserId = innerUserId,
                ProductIds = productsIds
            };

            var result =
                WebClientCaller.CallService<CatalogAdminServiceClient, SearchProductsResult>(
                    s => s.SearchProducts(parameters));

            return result;
        }

        public ImportProductsFromYmlResult ImportProductsFromYmlHttp(int partnerId, string fullFilePath, string userId = null)
        {
            var innerUserId = userId ?? UserId;

            var param = new ImportProductsFromYmlHttpParameters
                            {
                                PartnerId = partnerId,
                                FullFilePath = fullFilePath,
                                UserId = innerUserId
                            };
            var result =
                WebClientCaller.CallService<CatalogAdminServiceClient, ProductCatalog.WsClients.CatalogAdminService.ImportProductsFromYmlResult>(
                    s => s.ImportProductsFromYmlHttp(param));

            return new ImportProductsFromYmlResult
                       {
                           ResultCode = result.ResultCode,
                           ResultDescription = result.ResultDescription,
                           Success = result.Success,
                           TaskId = result.TaskId
                       };
        }

        public ImportDeliveryRatesResult ImportDeliveryRatesHttp(int partnerId, string fullFilePath, string userId = null)
        {
            var innerUserId = userId ?? UserId;

            var result =
                WebClientCaller.CallService<CatalogAdminServiceClient, ImportDeliveryRatesFromHttpResult>(
                    s => s.ImportDeliveryRatesFromHttp(partnerId, fullFilePath, innerUserId));

            return new ImportDeliveryRatesResult
                       {
                           ResultCode = result.ResultCode,
                           ResultDescription = result.ResultDescription,
                           Success = result.Success,
                           JobId = result.JobId
                       };
        }

        public PartnerCommitOrdersResult PartnerCommitOrder(
            int partnerId, PartnerOrderCommitment[] partnerOrderCommitments, string userId = null)
        {
            var innerUserId = userId ?? UserId;
            log.Info("Информирование \"Каталога товаров\" о подтверждении заказа/ов");

            var retVal =
                WebClientCaller.CallService<CatalogAdminServiceClient, PartnerCommitOrdersResult>(
                    cl => cl.PartnerCommitOrder(innerUserId, partnerId, partnerOrderCommitments));
            log.Info("Полученный ответ на информирование о подтверждении заказа/ов: " + retVal.Serialize());
            return retVal;
        }

        public PartnerSettings GetPartnerSettings(int partnerId, string userId = null)
        {
            var retVal = InternalGetPartnersSettigns(userId);
            var partnerSettings = retVal.Where(x => x.PartnerId == partnerId).ToDictionary(x => x.Key, v => v.Value);
            return new PartnerSettings(partnerId, partnerSettings);
        }

        public PartnerSettings GetPartnerSettingsByCertificateThumbprint(string thumbprint, string userId = null)
        {
            var all = InternalGetPartnersSettigns(userId).ToArray();
            var found = all.Where(x => x.Key == "CertificateThumbprint").FirstOrDefault(x => CompareThumbprints(x.Value, thumbprint));
            if (found == null)
            {
                return null;
            }

            var partnerSettigns = all.Where(x => x.PartnerId == found.PartnerId)
                                     .ToDictionary(x => x.Key, v => v.Value);
            return new PartnerSettings(found.PartnerId, partnerSettigns);
        }

        private static bool CompareThumbprints(string thumbprint1, string thumbprint2)
        {
            if (thumbprint1 == null || thumbprint2 == null)
            {
                return false;
            }

            var retVal = string.Compare(
                thumbprint1.Replace(" ", string.Empty),
                thumbprint2.Replace(" ", string.Empty),
                StringComparison.OrdinalIgnoreCase) == 0;
            return retVal;
        }

        private IEnumerable<WsPartnerSettings> InternalGetPartnersSettigns(string userId)
        {
            var innerUserId = userId ?? UserId;
            if (cachedPartnerSettings == null || lastPartnerSettingsLoad.AddSeconds(Timeout) < DateTime.Now)
            {
                lock (lockObj)
                {
                    if (cachedPartnerSettings == null || lastPartnerSettingsLoad.AddSeconds(Timeout) < DateTime.Now)
                    {
                        log.Info("Получение настроек партнеров из \"Каталога товаров\"");
                        var retVal =
                            WebClientCaller.CallService<CatalogAdminServiceClient, PartnersSettignsResult>(
                                cl => cl.GetPartnersSettings(innerUserId, null));
                        log.Info("Полученный ответ на запрос настроек партнера из \"Каталога товаров\": " + retVal.Serialize());

                        if (!retVal.Success)
                        {
                            var mess = string.Format("Не удалось получить настройки партнеров: {0}", retVal.ResultDescription);
                            throw new ApplicationException(mess);
                        }

                        cachedPartnerSettings = retVal.Settings;
                        lastPartnerSettingsLoad = DateTime.Now;
                    }
                }
            }
            else
            {
                log.Info("Настройки партнеров не загружаем из \"Каталога товаров\", используем закэшированные");
            }

            return cachedPartnerSettings;
        }
    }
}