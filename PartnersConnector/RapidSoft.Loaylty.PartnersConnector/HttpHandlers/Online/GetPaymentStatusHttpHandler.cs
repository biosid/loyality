using System;
using System.Web;
using RapidSoft.Loaylty.Logging;
using RapidSoft.Loaylty.Logging.Interaction;
using RapidSoft.Loaylty.PartnersConnector.Services;

namespace RapidSoft.Loaylty.PartnersConnector.HttpHandlers.Online
{
    public class GetPaymentStatusHttpHandler : PartnerInteractionHttpHandler
    {
        private readonly ILog log = LogManager.GetLogger(typeof(GetPaymentStatusHttpHandler));

        public GetPaymentStatusHttpHandler() : base("GetPaymentStatus")
        {
        }

        protected override void ProcessRequest(HttpContext context, IInteractionLogEntry logEntry)
        {
            var logId = Guid.NewGuid().ToString();
            var request = context.Request;
            log.InfoFormat("Запрос ({0}) {1}", logId, request.RawUrl);

            logEntry.Info["Request"] = request.Url.Query;

            var shopId = request["ShopId"];
            var orderId = request["OrderId"];
            var signature = request["Signature"];

            var service = new BonusPaymentGatewayService();

            var result = service.GetPaymentStatus(shopId, orderId, signature, logEntry);

            logEntry.Info["Response"] = result;

            context.Response.OkXmlResponse(result);

            log.InfoFormat("Ответ ({0}): {1}", logId, result);
        }
    }
}