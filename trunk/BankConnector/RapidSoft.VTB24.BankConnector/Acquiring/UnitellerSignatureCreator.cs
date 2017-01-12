namespace RapidSoft.VTB24.BankConnector.Acquiring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class UnitellerSignatureCreator
    {
        public string GetSignatureHash(UnitellerAuthInfo authInfo)
        {
            authInfo.Validate();

            Func<MD5, string, string> md5 = (func, input) =>
            {
                var data = func.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder();

                // Convert to hexadecimal string.
                data.ToList().ForEach(x => sb.Append(x.ToString("x2")));

                return sb.ToString();
            };

            using (var hashFunction = MD5.Create())
            {
                var codes = new List<string>
                            {
                                md5(hashFunction, authInfo.ShopId), 
                                md5(hashFunction, authInfo.OrderId), 
                                md5(hashFunction, authInfo.SubtotalString), 
                                md5(hashFunction, string.Empty), 
                                md5(hashFunction, string.Empty), 
                                md5(hashFunction, string.Empty), 
                                md5(hashFunction, authInfo.CustomerId), 
                                md5(hashFunction, string.Empty), 
                                md5(hashFunction, string.Empty), 
                                md5(hashFunction, string.Empty), 
                                md5(hashFunction, authInfo.Password)
                            };

                return md5(hashFunction, string.Join("&", codes)).ToUpper();
            }
        }
    }
}
