namespace RapidSoft.Loaylty.PartnersConnector.Interfaces
{
    public interface ICryptoService
    {
        bool VerifyData(string signature, string checkValue);

        string CreateSignature(string text);

        string Encrypt(string text);

        string Decrypt(string text);
    }
}