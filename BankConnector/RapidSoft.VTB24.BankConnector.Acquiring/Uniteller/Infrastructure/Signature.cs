using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models.Inputs;

namespace RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Infrastructure
{
    internal static class Signature
    {
        public static string Sign(this UnitellerPayParameters parameters, string password)
        {
            using (var md5 = MD5.Create())
            {
                var parts = parameters.GetParts(password)
                                      .Select(p => p.SignPart(md5))
                                      .ToArray();

                var signature = string.Join("&", parts).Sign(md5).ToUpper();

                return signature;
            }
        }

        private static IEnumerable<string> GetParts(this UnitellerPayParameters parameters, string password)
        {
            // Shop_IDP
            yield return parameters.ShopId;
            // Order_IDP
            yield return parameters.OrderId;
            // Subtotal_P
            yield return parameters.Subtotal.ToString("F2", CultureInfo.InvariantCulture);
            // MeanType
            yield return null;
            // EMoneyType
            yield return null;
            // Lifetime
            yield return parameters.Lifetime.HasValue ? parameters.Lifetime.Value.ToString("D") : null;
            // Customer_IDP
            yield return parameters.CustomerId;
            // Card_IDP
            yield return null;
            // IData
            yield return null;
            // PT_Code
            yield return null;
            // password
            yield return password;
        }

        private static string SignPart(this string data, MD5 md5)
        {
            return string.IsNullOrEmpty(data)
                       ? GetEmptyPart(md5)
                       : data.Sign(md5);
        }

        private static string Sign(this string data, MD5 md5)
        {
            var signatureBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(data));

            var signature = string.Join(string.Empty, signatureBytes.Select(b => b.ToString("x2")));

            return signature;
        }

        private static string _emptyPart;

        private static string GetEmptyPart(MD5 md5)
        {
            return _emptyPart ?? (_emptyPart = string.Empty.Sign(md5));
        }
    }
}
