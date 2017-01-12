namespace RapidSoft.Loaylty.PartnersConnector.HttpHandlers.Online
{
    using System;
    using System.Web;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.Logging.Interaction;
    using RapidSoft.Loaylty.PartnersConnector.Services;

    public class ValidateUserHttpHandler : PartnerInteractionHttpHandler
    {
        private readonly ILog log = LogManager.GetLogger(typeof (ValidateUserHttpHandler));

        public ValidateUserHttpHandler() : base("ValidateUser") {}

        protected override void ProcessRequest(HttpContext context, IInteractionLogEntry logEntry)
        {
            var logId = Guid.NewGuid().ToString();
            var request = context.Request;
            log.InfoFormat("Запрос ({0}) {1}", logId, request.RawUrl);

            logEntry.Info["Request"] = request.Url.Query;

            var shopId = request["ShopId"];
            var userTicket = request["UserTicket"];
            var signature = request["Signature"];

            var service = new BonusPaymentGatewayService();

            var result = service.ValidateUser(shopId, userTicket, signature, logEntry);

            logEntry.Info["Response"] = result;

            context.Response.OkXmlResponse(result);

            log.InfoFormat("Ответ ({0}): {1}", logId, result);
        }
    }
}