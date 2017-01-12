namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.Online
{
    using System;
    using System.Text;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;
    using RapidSoft.Loaylty.PartnersConnector.Services;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService;

    public partial class NotifyOrder
    {
        public string SerializeWithSing(ICryptoService cryptoService)
        {
            var serialized = this.Serialize(Encoding.UTF8, true);
            var sing = cryptoService.CreateSignature(serialized);
            this.Signature = sing;
            var retVal = this.Serialize(Encoding.UTF8, true);
            return retVal;
        }

        public static NotifyOrder DeserializeNotifyOrder(string notifyOrderXml)
        {
            if (string.IsNullOrWhiteSpace(notifyOrderXml))
            {
                const string Mess = "Тело запроса не содержит данных";
                throw new OperationException((int)ResultCodes.InvalidArgument, Mess);
            }

            NotifyOrder notifyOrder;
            try
            {
                notifyOrder = notifyOrderXml.Deserialize<NotifyOrder>(Encoding.UTF8);
            }
            catch (Exception ex)
            {
                var mess = "Тело запроса содержит не корректный xml: " + ex.Message;

                throw new OperationException((int)ResultCodes.InvalidArgument, mess);
            }

            return notifyOrder;
        }
    }

    public partial class NotifyOrderItem
    {
        public CreateOrderFromOnlinePartnerItem ConvertToCreateOrderFromOnlinePartnerItem()
        {
            var retVal = new CreateOrderFromOnlinePartnerItem
                             {
                                 ArticleId = this.ArticleId,
                                 ArticleName = this.ArticleName,
                                 Weight = this.Weight,
                                 Price = this.Price,
                                 BonusPrice = this.BonusPrice,
                                 Amount = this.Amount,
                                 Comment = this.Comment,
                                 Id = this.Id
                             };
            return retVal;
        }
    }
}