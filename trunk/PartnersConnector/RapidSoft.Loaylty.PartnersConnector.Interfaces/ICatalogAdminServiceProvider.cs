namespace RapidSoft.Loaylty.PartnersConnector.Interfaces
{
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

    using ImportProductsFromYmlResult = RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities.ImportProductsFromYmlResult;

    public interface ICatalogAdminServiceProvider
    {
        ImportProductsFromYmlResult ImportProductsFromYmlHttp(int partnerId, string fullFilePath, string userId = null);

        ImportDeliveryRatesResult ImportDeliveryRatesHttp(int partnerId, string fullFilePath, string userId = null);

        PartnerCommitOrdersResult PartnerCommitOrder(int partnerId, PartnerOrderCommitment[] partnerOrderCommitments, string userId = null);

        Settings.PartnerSettings GetPartnerSettings(int partnerId, string userId = null);

        Settings.PartnerSettings GetPartnerSettingsByCertificateThumbprint(string thumbprint, string userId = null);

        SearchProductsResult SearchAllProducts(string userId = null, string[] productsIds = null);
    }
}