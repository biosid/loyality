using System.Web.Mvc;
using Serilog;
using Vtb24.Logging;
using Vtb24.Logging.Interaction;

namespace Vtb24.Site.Filters
{
    public class InteractionAttribute : ActionFilterAttribute
    {
        private readonly ILogger _log = SerilogLoggers.MainLogger.ForContext<InteractionAttribute>();
        private readonly string _subject;
        private readonly string _name;

        private IInteractionLogEntry _logEntry;

        public InteractionAttribute(string subject, string name)
        {
            _subject = subject;
            _name = name;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _logEntry = StartInteraction.With(_subject).For(_name);

            filterContext.RouteData.Values["logEntry"] = _logEntry;

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                _logEntry.Failed("ошибка взаимодействия", filterContext.Exception);
            }

            _logEntry.FinishAndWrite(_log);

            base.OnActionExecuted(filterContext);
        }
    }
}
