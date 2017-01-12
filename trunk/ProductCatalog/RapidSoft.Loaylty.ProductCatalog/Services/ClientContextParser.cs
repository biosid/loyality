namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System.Collections.Generic;
    using System.Linq;

    public class ClientContextParser
    {
        public static string ClientIdKey
        {
            get
            {
                return "ClientProfile.UserId";
            }
        }

        public static string AudiencesKey
        {
            get
            {
                return "ClientProfile.Audiences";
            }
        }

        public static string LocationKladrCodeKey
        {
            get
            {
                return "ClientProfile.KLADR";
            }
        }

        public string GetLocationKladrCode(Dictionary<string, string> clientContext)
        {
            return GetValue(LocationKladrCodeKey, clientContext);
        }

        public string GetAudienceIds(Dictionary<string, string> clientContext)
        {
            return GetValue(AudiencesKey, clientContext);
        }

        public string GetClientId(Dictionary<string, string> parameters)
        {
            return GetValue(ClientIdKey, parameters);
        }

        private string GetValue(string key, Dictionary<string, string> context)
        {
            if (context == null || string.IsNullOrEmpty(key))
            {
                return null;
            }

            var res =
                context.SingleOrDefault(
                    entry => entry.Key.Equals(key) || entry.Key.Trim().Equals(key.Trim()));

            return res.Value;
        }
    }
}