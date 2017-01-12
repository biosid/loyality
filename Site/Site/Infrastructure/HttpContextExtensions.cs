using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vtb24.Site.Infrastructure
{
    public static class HttpContextExtensions
    {
        public static void ProcessNotFoundRequest(this HttpContext context)
        {
            var routeData = new RouteData
            {
                Values =
                {
                    { "controller", "Error" },
                    { "action", "NotFound" }
                }
            };

            ProcessRequest(context, routeData);
        }

        public static void ProcessErrorRequest(this HttpContext context)
        {
            var routeData = new RouteData
            {
                Values =
                {
                    { "controller", "Error" },
                    { "action", "Error" }
                }
            };

            ProcessRequest(context, routeData);
        }

        private static void ProcessRequest(HttpContext context, RouteData routeData)
        {
            // начнём с чистого листа
            context.Response.Clear();

            // подготовим контекст запроса
            var requestContext = new RequestContext(new HttpContextWrapper(context), routeData);

            // удалим текущий Unity-контейнер (он уже Disposed)
            requestContext.HttpContext.Items.Remove("perRequestContainer");

            // построим контроллер через стандартную фабрику контроллеров
            var controller = ControllerBuilder.Current
                                              .GetControllerFactory()
                                              .CreateController(
                                                  requestContext,
                                                  routeData.Values["controller"].ToString());

            // обработаем запрос по стандартной схеме MVC
            controller.Execute(new RequestContext(new HttpContextWrapper(context), routeData));
        }
    }
}
