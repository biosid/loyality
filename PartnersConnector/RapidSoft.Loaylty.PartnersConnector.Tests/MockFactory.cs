namespace RapidSoft.Loaylty.PartnersConnector.Tests
{
    using System.Collections.Generic;

    using Interfaces;
    using Interfaces.Settings;

    using Moq;

    using ProductCatalog.WsClients.OrderManagementService;

    public static class MockFactory
    {
         public static Mock<IProductCatalogProvider> GetProductCatalogProvider()
         {
             var mock2 = new Mock<IProductCatalogProvider>();

             mock2.Setup(m => m.ChangeOrdersStatuses(It.IsAny<ExternalOrdersStatus[]>()));

             return mock2;
         }

         public static Mock<ICatalogAdminServiceProvider> GetCatalogAdminServiceProvider()
         {
             var mock2 = new Mock<ICatalogAdminServiceProvider>();

             mock2.Setup(m => m.GetPartnerSettings(It.IsAny<int>(), It.IsAny<string>())).Returns<int, string>(GetSettings);

             return mock2;
         }

        private static PartnerSettings GetSettings(int partnerId, string userId)
        {
            return new PartnerSettings(partnerId, TestHelper.GetSettings(false));
        }
    }
}