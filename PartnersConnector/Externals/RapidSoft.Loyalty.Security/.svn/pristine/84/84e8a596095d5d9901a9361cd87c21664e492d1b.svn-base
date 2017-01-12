using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace RapidSoft.Loyalty.Security
{
    public class StoreCertificateProvider : ICertificateProvider
    {
        public X509Certificate2 GetByThumbprint(string thumbprint)
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            thumbprint = Regex.Replace(thumbprint, @"[^\u0000-\u007F]", string.Empty); 

            var certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, true);
            store.Close();
            
            if (certs.Count == 0)
            {
                throw new InvalidOperationException(string.Format("Certificate with thumbprint {0} in LocalMachine cert store not found", thumbprint));
            }
            return certs[0];
        }
    }
}
