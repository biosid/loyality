using System;
using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models;

namespace RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Infrastructure
{
    internal static class MappingsFromService
    {
        public static AuthorizeResponseCode ToAuthorizeResponseCode(string original)
        {
            AuthorizeResponseCode code;
            return Enum.TryParse(original, out code) ? code : AuthorizeResponseCode.Unknown;
        }
    }
}
