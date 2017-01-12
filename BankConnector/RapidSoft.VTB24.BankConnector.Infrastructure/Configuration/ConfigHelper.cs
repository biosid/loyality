using System;
using System.Configuration;

namespace RapidSoft.VTB24.BankConnector.Infrastructure.Configuration
{
    public static class ConfigHelper
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["BankConnectorDB"].ConnectionString;
            }
        }

		public static string UnitellerPaymentLogin
		{
			get
			{
				return ConfigurationManager.AppSettings["UnitellerLogin"];
			}
		}

		public static string UnitellerPaymentPassword
        {
            get
            {
				return ConfigurationManager.AppSettings["UnitellerPassword"];
            }
        }

		public static string UnitellerPaymentUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["UnitellerUrl"];
			}
		}

		public static string UnitellerPaymentUrlConfirm
		{
			get
			{
				return ConfigurationManager.AppSettings["UnitellerUrlConfirm"];
			}
		}

		public static string UnitellerPaymentUrlCheque
		{
			get
			{
				return ConfigurationManager.AppSettings["UnitellerUrlCheque"];
			}
		}

		public static string UnitellerPaymentUrlResult
		{
			get
			{
				return ConfigurationManager.AppSettings["UnitellerUrlResult"];
			}
		}

		public static int UnitellerResultFormat
		{
			get
			{
				return Convert.ToInt32(ConfigurationManager.AppSettings["UnitellerResultFormat"]);
			}
		}

		public static string UnitellerResultFields
		{
			get
			{
				return ConfigurationManager.AppSettings["UnitellerResultFields"];
			}
		}

		public static string UnitellerPaymentReturnUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["UnitellerReturnUrl"];
            }
        }

        public static string UnitellerRegisterShopId
        {
            get
            {
                return ConfigurationManager.AppSettings["UnitellerRegisterShopId"];
            }
        }

        public static string UnitellerRegisterLogin
        {
            get
            {
                return ConfigurationManager.AppSettings["UnitellerRegisterLogin"];
            }
        }

        public static string UnitellerRegisterPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["UnitellerRegisterPassword"];
            }
        }

        public static int Cv2
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["CV2"]);
            }
        }

        public static string LoyaltyPosId
        {
            get
            {
                return ConfigurationManager.AppSettings["LoyaltyPOSId"];
            }
        }

        public static string LoyaltyTerminalId
        {
            get
            {
                return ConfigurationManager.AppSettings["LoyaltyTerminalId"];
            }
        }

        public static string CardReaderId
        {
            get
            {
                return ConfigurationManager.AppSettings["CardReaderId"];
            }
        }

        public static string ChannelId
        {
            get
            {
                return ConfigurationManager.AppSettings["ChannelId"];
            }
        }

        public static int LoyaltyProgramId
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["LoyaltyProgramId"]);
            }
        }
	
		public static int BatchSize
		{
			get
			{
				return Convert.ToInt32(ConfigurationManager.AppSettings["BatchSize"]);
			}
		}

		public static string VtbSystemUser
		{
			get
			{
				return ConfigurationManager.AppSettings["VtbSystemUser"];
			}
		}

        public static int DefaultCountryCode
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCountryCode"]);
            }
        }

        public static string LoyaltyPartnerId
        {
            get
            {
                return ConfigurationManager.AppSettings["LoyaltyPartnerId"];
            }
        }

        public static string ValidPhoneRegExp
        {
            get
            {
                return ConfigurationManager.AppSettings["ValidPhoneRegExp"];
            }
        }

        public static string SchemaName
        {
            get
            {
                return "etl";
            }
        }

        public static string PaymentSupportPartnerSetting
        {
            get
            {
                return ConfigurationManager.AppSettings["payment_support_partner_setting"];
            }
        }

        public static string PaymentUnitellerShopIdPartnerSetting
        {
            get
            {
                return ConfigurationManager.AppSettings["payment_uniteller_shop_id_partner_setting"];
            }
        }

        public static bool PaymentUnitellerUsePreauth
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["payment_uniteller_use_preauth"]);
            }
        }

        public static int PaymentUnitellerLifetime
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["payment_uniteller_lifetime"]);
            }
        }

        public static string AcquiringUnitellerLogin
        {
            get
            {
                return ConfigurationManager.AppSettings["acquiring_uniteller_login"];
            }
        }

        public static string AcquiringUnitellerPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["acquiring_uniteller_password"];
            }
        }

        public static string AcquiringUnitellerPayFormUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["acquiring_uniteller_pay_form_url"];
            }
        }

        public static string AcquiringUnitellerResultsUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["acquiring_uniteller_results_url"];
            }
        }

        public static string AcquiringUnitellerConfirmUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["acquiring_uniteller_confirm_url"];
            }
        }

        public static string AcquiringUnitellerCancelUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["acquiring_uniteller_cancel_url"];
            }
        }

        public static string ClientSiteBasketUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["client_site_basket_url"];
            }
        }

        public static string ClientSiteOrderUrlTemplate
        {
            get
            {
                return ConfigurationManager.AppSettings["client_site_order_url_template"];
            }
        }

        public static int BankProductsPartnerId
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["BankProductsPartnerId"]);
            }
        }
    }
}
