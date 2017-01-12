namespace RapidSoft.Loaylty.PartnersConnector.Interfaces
{
    public interface ICryptoServiceFactory
    {
        ICryptoService GetBankCryptoService();

        ICryptoService GetParnterCryptoService(int partnerId);
    }
}