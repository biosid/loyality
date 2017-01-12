namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System.Collections.Generic;

    using API.Entities;

    using Entities;

    public interface IDeliveryRatesRepository
    {
        bool HasDelivery(int partnerId, string kladr);

        int[] GetDeliveringPartnerIds(string kladr);

        Page<DeliveryLocation> GetDeliveryLocations(
            int partnerId, DeliveryLocationStatus[] statuses, int? countToSkip, int? countToTake, bool? calcTotalCount, bool? hasRates, string searchTerm);

        DeliveryLocation GetDeliveryLocation(int id);

        List<DeliveryLocation> GetDeliveryLocationsByPartnerAndKladr(int partnerId, string kladrCode, int? countToSkip, int? countToTake);

        DeliveryLocation GetDeliveryLocationByPartnerAndKladr(int partnerId, string kladrCode, DeliveryLocationStatus[] statuses, int[] excludeIds, bool? hasRates);

        DeliveryLocation SaveDeliveryLocation(DeliveryLocation deliveryLocation);

        Page<DeliveryLocationHistory> GetDeliveryLocationHistory(
            int? locationId, int partnerId, int? countToSkip, int? countToTake, bool? calcTotalCount);

        Page<string> GetKladrCodesFromBuffer(string etlSessionId, int countToSkip, int countToTake);

        void SetDeliveryBufferStatusByKladr(string etlSessionId, IList<string> kladrCodes, int status);

        PartnerDeliveryRate GetMinPriceRate(int partnerId, string kladr, int weight);
    }
}