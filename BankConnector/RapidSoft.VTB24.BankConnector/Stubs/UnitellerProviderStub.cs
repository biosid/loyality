namespace RapidSoft.VTB24.BankConnector.Stubs
{
    using System;
    using System.Collections.Generic;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.VTB24.BankConnector.Acquiring;

    public class UnitellerProviderStub : IUnitellerProvider
    {
        private static Dictionary<string, decimal> partnerSum = new Dictionary<string, decimal>();

        private readonly ILog logger = LogManager.GetLogger(typeof(UnitellerProviderStub));

        public static Dictionary<string, decimal> PartnerSum
        {
            get
            {
                return partnerSum;
            }
        }

        public static void ResetSum()
        {
            partnerSum = new Dictionary<string, decimal>();
        }

        public void Pay(string shopId, decimal sum, string client, string orderId)
        {
            if (string.IsNullOrEmpty(shopId))
            {
                throw new ArgumentException("shopId is null or empty");
            }

            if (string.IsNullOrEmpty(client))
            {
                throw new ArgumentException("client is null or empty");
            }

            if (string.IsNullOrEmpty(orderId))
            {
                throw new ArgumentException("orderId is null or empty");
            }

            logger.InfoFormat("Вызван метод UnitellerProviderStub.Pay: shopId = {0}, sum = {1}, client = {2}, orderId = {3}", shopId, sum, client, orderId);

            if (partnerSum.ContainsKey(shopId))
            {
                partnerSum[shopId] += sum;
            }
            else
            {
                partnerSum[shopId] = sum;
            }
        }

        public void RegisterCard(string client)
        {
            if (string.IsNullOrEmpty(client))
            {
                throw new ArgumentException("client is null or empty");
            }
        }
    }
}
