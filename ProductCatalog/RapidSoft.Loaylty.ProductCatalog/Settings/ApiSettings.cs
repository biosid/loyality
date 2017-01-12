namespace RapidSoft.Loaylty.ProductCatalog.Settings
{
    using System;
    using System.Configuration;

    using Extensions;

    public class ApiSettings
    {
        public static string EtlSchemaName
        {
            get
            {
                return ConfigurationManager.AppSettings["EtlSchemaName"] ?? "dbo";
            }
        }

        public static string ClientSiteUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["VtbSystemUserName"];
            }
        }

        public static int MaxResultsCountBasketItems
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountBasketItems", 1000);
            }
        }

        /// <summary>
        /// Максимальное кол-во единиц товара в элементе корзины.
        /// </summary>
        public static int MaxBasketItemProductsQuantity
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxBasketItemProductsQuantity", 100);
            }
        }

        public static int MaxResultsCountOrders
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountOrders", 1000);
            }
        }

        public static int MaxResultsCountCategories
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountCategories", 1000);
            }
        }

        public static int MaxResultsCountWishList
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountWishList", 1000);
            }
        }

        /// <summary>
        /// Максимальное кол-во единиц товара в элементе WishList'а.
        /// </summary>
        public static int MaxWishListItemProductsQuantity
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxWishListItemProductsQuantity", 100);
            }
        }

        /// <summary>
        /// Максимальное кол-во элементов в WishList'е.
        /// </summary>
        public static int MaxWishListItemsCount
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxWishListProductsQuantity", 30);
            }
        }

        public static int MaxResultsCountProducts
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountProducts", 1000);
            }
        }

        public static int MaxResultsCountDeliveryAddresses
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountDeliveryAddresses", 100);
            }
        }

        public static int MaxResultsCountPopularProducts
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountPopularProducts", 50);
            }
        }

        public static int MaxResultsCountRecommendedProducts
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountRecommendedProducts", 10);
            }
        }

        public static int MaxResultsCountImportYmlTasks
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountImportYmlTasks", 10);
            }
        }

        public static int MaxResultsCountImportDeliveryRateTasks
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountImportDeliveryRateTasks", 10);
            }
        }

        public static int MaxResultsCountDeliveryBindings
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountDeliveryBindings", 100);
            }
        }

        public static int MaxResultsCountDeliveryBindingHistory
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountDeliveryBindingHistory", 100);
            }
        }

        public static int BatchSizeValidatingKladrByGeobase
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("BatchSizeValidatingKladrByGeobase", 100);
            }
        }

        public static string ImportDeliveryRatesEtlPackageIds
        {
            get
            {
                return ConfigurationManager.AppSettings["ImportDeliveryRatesEtlPackageIds"] ?? string.Empty;
            }
        }

        public static int MechanicsCasheSeconds
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MechanicsCasheSeconds", 20);
            }
        }

        public static string BonusGatewayPartnerId
        {
            get
            {
                var retVal = ConfigurationManager.AppSettings["BonusGatewayPartnerId"];
                if (retVal == null)
                {
                    throw new NullReferenceException("Необходимо задать значение BonusGatewayPartnerId в конфигурационном файле");
                }

                return retVal;
            }
        }

        public static string BonusGatewayPosId
        {
            get
            {
                var retVal = ConfigurationManager.AppSettings["BonusGatewayPosId"];
                if (retVal == null)
                {
                    throw new NullReferenceException("Необходимо задать значение BonusGatewayPosId в конфигурационном файле");
                }

                return retVal;
            }
        }

        public static string BonusGatewayTerminalId
        {
            get
            {
                var retVal = ConfigurationManager.AppSettings["BonusGatewayTerminalId"];
                if (retVal == null)
                {
                    throw new NullReferenceException("Необходимо задать значение BonusGatewayTerminalId в конфигурационном файле");
                }

                return retVal;
            }
        }
    }
}