namespace RapidSoft.Loaylty.PartnersConnector.HttpHandlers.Online
{
    using System;
    using System.Web;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.Logging.Interaction;
    using RapidSoft.Loaylty.PartnersConnector.Services;

    /// <summary>
    /// Summary description for NotifyOrderStatusChange
    /// </summary>
    public class NotifyOrderHttpHandler : PartnerInteractionHttpHandler
    {
        private readonly ILog log = LogManager.GetLogger(typeof (NotifyOrderHttpHandler));

        public NotifyOrderHttpHandler() : base("NotifyOrder") {}

        protected override void ProcessRequest(HttpContext context, IInteractionLogEntry logEntry)
        {
            var logId = Guid.NewGuid().ToString();
            var request = context.Request;

            var body = request.ReadBody();

            logEntry.Info["Request"] = body;

            log.InfoFormat("NotifyOrder Запрос ({0}) {1}, body: {2}", logId, request.RawUrl, body);

            var service = new BonusPaymentGatewayService();

            var result = service.NotifyOrder(body, logEntry);

            logEntry.Info["Response"] = result;
            
            context.Response.OkXmlResponse(result);

            log.InfoFormat("Ответ ({0}): {1}", logId, result);
        }
    }
}
