using System.Security.Cryptography.X509Certificates;

namespace RapidSoft.Loyalty.Security
{
    public interface ICertificateProvider
    {
        X509Certificate2 GetByThumbprint(string thumbprint);
    }
}
