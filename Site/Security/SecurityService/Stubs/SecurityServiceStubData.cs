using System.Collections.Generic;
using System.Text.RegularExpressions;
using Vtb24.Site.Security.Models;

namespace Vtb24.Site.Security.SecurityService.Stubs
{
    internal class SecurityServiceStubData
    {
        static SecurityServiceStubData()
        {
            Accounts = GenerateAccounts();
        }

        public static Dictionary<string, ClientPrincipal> Accounts { get; set; }

        private static Dictionary<string, ClientPrincipal> GenerateAccounts()
        {
            var accounts = new Dictionary<string, ClientPrincipal>
            {
                {
                    NormalizePhone("+7 (111) 111-1111"),
                    new ClientPrincipal
                        {
                            ClientId = "vtb_1",
                            ClientIp = "127.0.0.1"
                        }
                },
                {
                    NormalizePhone("+7 (222) 222-2222"),
                    new ClientPrincipal
                        {
                            ClientId = "vtb_2",
                            ClientIp = "127.0.0.1"
                        }
                }
            };

            return accounts;
        }

        private static string NormalizePhone(string phone)
        {
            return Regex.Replace(phone, @"[^\d]", "");
        }
    }
}