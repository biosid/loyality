using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Partners
{
    internal interface IPartners
    {
        #region получение краткой информации о партнерах

        PartnerInfo[] GetPartnersInfo();

        SupplierInfo[] GetSuppliersInfo();

        CarrierInfo[] GetCarriersInfo();

        PartnerInfo[] GetUserPartnersInfo();

        SupplierInfo[] GetUserSuppliersInfo();

        CarrierInfo[] GetUserCarriersInfo();

        PartnerInfo GetPartnerInfoById(int id);

        SupplierInfo GetSupplierInfoById(int id);

        CarrierInfo GetCarrierInfoById(int id);

        PartnerInfo GetUserPartnerInfoById(int id);

        SupplierInfo GetUserSupplierInfoById(int id);

        CarrierInfo GetUserCarrierInfoById(int id);

        #endregion

        #region получение полной информации о партнерах

        Partner[] GetPartners();

        Supplier[] GetSuppliers();

        Carrier[] GetCarriers();

        Partner GetPartnerById(int id);

        Supplier GetSupplierById(int id);

        Carrier GetCarrierById(int id);

        #endregion

        #region создание/обновление партнеров

        int CreateSupplier(Supplier supplier);

        int CreateCarrier(Carrier carrier);

        void UpdateSupplier(Supplier supplier);

        void UpdateCarrier(Carrier carrier);

        #endregion

        #region загрузка матрицы стоимости доставки

        void ImportDeliveryRatesFromHttp(int partnerId, string fileUrl);

        #endregion
    }
}
