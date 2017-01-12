using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Vtb24.Site.Content.Pages;
using Vtb24.Site.Controllers;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // задаем constraint на имя контроллера, чтобы была возможность
            // передать контекст на следующий маршрут CustomPageRoute
            // (если не задать такой constraint, то запросы будут мапиться
            // на несуществующий контроллер и возвращать 404)

            var controllerConstraint = string.Join("|", GetControllersNames(typeof(ErrorController)));

            routes.MapRoute(
                "PleaseRegister",
                "PleaseRegister",
                new { controller = "Main", action = "PleaseRegister" },
                new { controller = controllerConstraint }
            );

            routes.MapRoute(
                "PersonalVideo",
                "pvideo",
                new { controller = "PersonalVideo", action = "Index" },
                new { controller = controllerConstraint }
            );

            routes.MapRoute(
                "AnonymousFeedback",
                "Feedback/Conversation/{id}",
                new { controller = "Feedback", action = "AnonymousThread" },
                new { controller = controllerConstraint }
            );

            routes.MapRoute(
                "ClientFeedback",
                "MyMessages/Conversation/{id}",
                new { controller = "Feedback", action = "ClientThread" },
                new { controller = controllerConstraint }
            );

            routes.MapRoute(
                "BecomeAPartner",
                "become-a-partner",
                new { controller = "Feedback", action = "BecomeAPartner" },
                new { controller = controllerConstraint }
            );

            routes.MapRoute(
                "Uploads",
                "uploads/u/{id}/{fileName}",
                new { controller = "Files", action = "Uploads" },
                new { controller = controllerConstraint }
            );

            routes.MapRoute(
                "Unsubscribe",
                "Unsubscribe/{emailHash}",
                new { controller = "Unsubscribe", action = "Index" },
                new { controller = controllerConstraint }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Main", action = "Index", id = UrlParameter.Optional },
                new { controller = controllerConstraint }
            );

            routes.Add(new CustomPageRoute(DependencyResolver.Current.GetService<IPagesManagement>()));

            routes.MapRoute("Error", "{*url}", new { controller = "Error", action = "NotFound" });
        }

        private const string CONTROLLER_NAME_ENDS_WITH = "Controller";

        private static IEnumerable<string> GetControllersNames(params Type[] exclude)
        {
            // ищем все классы контроллеров:
            //   - унаследованые от Controller
            //   - публичные
            //   - неабстрактные
            //   - имеют имя, оканчивающееся на "Controller"
            var allControllers = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(IsController);

            // возвращаем их имена без "Controller",
            // если нужно исключаем контроллеры из списка exclude
            return (exclude != null
                        ? allControllers.Except(exclude)
                        : allControllers)
                .Select(t => t.Name.Substring(0, t.Name.Length - CONTROLLER_NAME_ENDS_WITH.Length));
        }

        private static bool IsController(Type type)
        {
            if (type.IsAbstract || !type.IsPublic)
                return false;

            if (!type.Name.EndsWith(CONTROLLER_NAME_ENDS_WITH))
                return false;

            var baseType = type.BaseType;
            while (baseType != null)
            {
                if (baseType == typeof (Controller))
                    return true;
                baseType = baseType.BaseType;
            }

            return false;
        }
    }
}