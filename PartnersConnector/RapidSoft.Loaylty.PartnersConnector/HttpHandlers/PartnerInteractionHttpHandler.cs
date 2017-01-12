using System;
using System.Web;
using RapidSoft.Loaylty.Logging;
using RapidSoft.Loaylty.Logging.Interaction;

namespace RapidSoft.Loaylty.PartnersConnector.HttpHandlers
{
    public abstract class PartnerInteractionHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var logEntry = StartInteraction.With("Partner").For(_interactionName);

            try
            {
                ProcessRequest(context, logEntry);
            }
            catch (Exception e)
            {
                logEntry.Failed("необработанная ошибка", e);
                throw;
            }
            finally
            {
                logEntry.FinishAndWrite(_log);
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        protected PartnerInteractionHttpHandler(string interactionName)
        {
            _interactionName = interactionName;
        }

        private readonly ILog _log = LogManager.GetLogger(typeof(PartnerInteractionHttpHandler));
        private readonly string _interactionName;

        protected abstract void ProcessRequest(HttpContext context, IInteractionLogEntry logEntry);
    }
}
