using System.Diagnostics;
using System.Runtime.Caching;
using System.Web.Mvc;
using System.Linq;
using Vtb24.Site.Infrastructure.Caching;

namespace Vtb24.Site.Controllers
{
    [Authorize]
    public class CacheController : Controller
    {
        // TODO: мир пока не готов к этому
        /*
        [HttpGet]
        public ActionResult Purge(string what)
        {
            var cache = Caching.Current;

            if (cache == null)
            {
                return Content("Кэш не проинициализировал. Скорее всего, кэш отключен в конфигурации");
            }

            var timer = new Stopwatch();

            timer.Start();

            var before = cache.Store.GetCount();
            cache.Purge();
            var after = cache.Store.GetCount();

            timer.Stop();

            return Content(
                string.Format("Кэш очищен. Кол-во записей до очистки: {0}, после очистки: {1}, время выполнения: {2}", before, after, timer.Elapsed)
            );
        }
       */
    }
}
