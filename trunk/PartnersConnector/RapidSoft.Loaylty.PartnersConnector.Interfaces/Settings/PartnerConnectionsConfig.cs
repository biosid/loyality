namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Settings
{
	using System;
	using System.Configuration;
	using System.IO;
	using System.Reflection;
	using System.Web;

    public static class PartnerConnectionsConfig
	{
		private const int DefaultMaxTaskRefire = 2;
		private const int DefaultRefireCountToMilisecFactore = 10000;

		private static string bankPrivateKey = null;

        public static string GiftFilesPath
        {
            get
            {
                var retVal = ConfigurationManager.AppSettings["GiftFilesPath"];
                if (retVal == null)
                {
                    throw new NullReferenceException("Необходимо задать значение GiftFilesPath в конфигурационном файле");
                }

                return retVal;
            }
        }

		public static string PosId
		{
			get
			{
				var retVal = ConfigurationManager.AppSettings["PosId"];
				if (retVal == null)
				{
					throw new NullReferenceException("Необходимо задать значение PosId в конфигурационном файле");
				}

				return retVal;
			}
		}

		public static string TerminalId
		{
			get
			{
				var retVal = ConfigurationManager.AppSettings["TerminalId"];
				if (retVal == null)
				{
					throw new NullReferenceException("Необходимо задать значение TerminalId в конфигурационном файле");
				}

				return retVal;
			}
		}

		public static int MaxTaskRefire
		{
			get
			{
				var retVal = ConfigurationManager.AppSettings["MaxTaskRefire"];
				if (retVal == null)
				{
					return DefaultMaxTaskRefire;
				}

				return Convert.ToInt32(ConfigurationManager.AppSettings["MaxTaskRefire"]);
			}
		}

		public static int RefireCountToMilisecFactor
		{
			get
			{
				var retVal = ConfigurationManager.AppSettings["RefireCountToMilisecFactor"];
				if (retVal == null)
				{
					return DefaultRefireCountToMilisecFactore;
				}

				return Convert.ToInt32(ConfigurationManager.AppSettings["RefireCountToMilisecFactor"]);
			}
		}

		public static string BankPrivateKey
		{
			get
			{
				if (bankPrivateKey == null)
				{
                    var file = ConfigurationManager.AppSettings["BankPrivateKeyFile"];
                    if (file == null)
                    {
                        throw new NullReferenceException("Необходимо задать значение BankPrivateKeyFile в конфигурационном файле");
                    }

                    if (HttpContext.Current != null)
                    {
                        if (!Path.IsPathRooted(file))
                        {
                            file = HttpContext.Current.Server.MapPath(file);
                        }
                    }

					if (!File.Exists(file))
					{
                        throw new Exception(string.Format("Файл \"{0}\" не найден", file));
					}

					using (var sr = new StreamReader(file))
					{
						bankPrivateKey = sr.ReadToEnd();
					}
				}

				return bankPrivateKey;
			}
		}
	}
}