using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime;

using RapidSoft.Etl.Monitor.Agents;
using RapidSoft.Etl.Monitor.Helpers;
using RapidSoft.Etl.Monitor.Models;
using RapidSoft.Etl.Monitor.Resources.Views.Shared;
using System.Collections.Generic;

namespace RapidSoft.Etl.Monitor.Controllers
{
    public class EtlPackageMonitorController : AsyncController
    {
        #region Constants

        private const string PACKAGE_CURRENTLY_RUNNING = "Пакет выполняется";
        private const string DEFAULT_ERROR_MSG = "Произошла ошибка при выполнении пакета";
        private const string CACHE_KEY_ETL_PACKAGES = "EtlPackages";
        private const string CACHE_KEY_MONITOR_MODEL = "EtlPackageMonitorModel";

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult EtlPackageMonitor()
        {
            var model = (EtlPackageMonitorModel)HttpContext.Cache.Get(CACHE_KEY_MONITOR_MODEL);

            model = RefreshModel(model);

            HttpContext.Cache.Add(
                CACHE_KEY_MONITOR_MODEL,
                model,
                null,
                DateTime.Now.AddSeconds(SiteConfiguration.CacheExpiresInSeconds),
                Cache.NoSlidingExpiration,
                CacheItemPriority.Normal,
                null);

            Response.AddHeader("Refresh", SiteConfiguration.RefreshMonitoringEverySeconds.ToString());
            return View(model);
        }

        [HttpGet]
        public ActionResult InvokePackage(string etlPackageId)
        {
            var agent = SiteConfiguration.GetEtlAgent();
            var result = agent.InvokeEtlPackage(etlPackageId, null, null);
            if (result == null)
            {
                throw new Exception("Package \"" + etlPackageId + "\" does not exist or package has no text");
            }

            var model = HttpContext.Cache.Get("EtlPackageMonitorModel");
            if (model != null)
            {
                var etlPackageMonitorModel = (EtlPackageMonitorModel)model;
                var item = etlPackageMonitorModel.Items.FirstOrDefault(i => String.Equals(i.EtlPackageId, etlPackageId, StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    item.ForceRefresh = true;
            }

            return RedirectToAction("EtlPackageMonitor");
        }

        #endregion

        #region Methods

        private IEnumerable<EtlPackage> GetEtlPackages()
        {
            var obj = HttpContext.Cache.Get(CACHE_KEY_ETL_PACKAGES);
            if (obj == null)
            {
                var provider = SiteConfiguration.GetEtlAgent();

                obj = provider.GetEtlPackages();

                HttpContext.Cache.Add(
                    CACHE_KEY_ETL_PACKAGES,
                    obj,
                    null,
                    DateTime.Now.AddSeconds(SiteConfiguration.CacheExpiresInSeconds),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal,
                    null);
            }
            return (IEnumerable<EtlPackage>)obj;
        }

        private EtlPackageMonitorModel RefreshModel(EtlPackageMonitorModel model)
        {
            // обновляем модель или создаем заново
            var refresh = true;

            if (model == null)
            {
                model = new EtlPackageMonitorModel();

                model.ItemsByColumns = new List<EtlPackageMonitorItem>[SiteConfiguration.MaxCoulmnsToShow];
                for (var columnIndex = 0; columnIndex < SiteConfiguration.MaxCoulmnsToShow; columnIndex++)
                {
                    model.ItemsByColumns[columnIndex] = new List<EtlPackageMonitorItem>();
                }

                refresh = false;
            }

            model.ColumnCount = SiteConfiguration.MaxCoulmnsToShow;

            IEnumerable<EtlPackage> packages = new EtlPackage[0];
            if (model.Items.Count() > 0)
            {
                // идентификаторы пакетов из модели которые необходимо обновить
                var packageIds = model.Items.Where(i => i.ForceRefresh || i.EtlPackageStatus != EtlPackageMonitorItemStatus.Succeeded).Select(i => i.EtlPackageId);
                // сами пакеты
                packages = GetEtlPackages().Where(p => packageIds.Contains(p.Id, StringComparer.OrdinalIgnoreCase));
            }
            else
            {
                packages = GetEtlPackages();
            }

            var agent = SiteConfiguration.GetEtlAgent();
            var logParser = agent.GetEtlLogParser();
            var sessions = logParser.GetLatestEtlSessions(packages.Select(p => p.Id).ToArray());

            model.HasErrors = false;

            var packageIndex = 0;
            foreach (var package in packages)
            {
                var item = new EtlPackageMonitorItem();
                item.EtlPackageId = package.Id;
                item.RunIntervalSeconds = package.RunIntervalSeconds;
                item.EtlPackageName = package.Name;
                item.CanInvoke = true;

                var session = sessions.Where(s => s.EtlPackageId == package.Id).FirstOrDefault();

                if (session != null)
                {
                    var messages = logParser.GetEtlMessages(package.Id, session.EtlSessionId);

                    item.EtlSessionId = session.EtlSessionId;
                    item.EtlSessionDateTime = session.EndDateTime;
                    item.EtlPackageStatus = EtlPackageMonitorItemStatuses.GetMonitorItemStatus(session, package.RunIntervalSeconds, messages);
                    item.Counters = logParser.GetEtlCounters(package.Id, session.EtlSessionId);                    

                    if (item.EtlPackageStatus == EtlPackageMonitorItemStatus.Failed)
                    {
                        var errorMessage = messages.OrderByDescending(m => m.SequentialId).FirstOrDefault(m => m.MessageType == EtlMessageType.Error);
                        item.StatusMessage = errorMessage == null ? DEFAULT_ERROR_MSG : errorMessage.Text;
                    }

                    if (item.EtlPackageStatus == EtlPackageMonitorItemStatus.TooFar && session.Status == EtlStatus.Started)
                    {
                        item.StatusMessage = PACKAGE_CURRENTLY_RUNNING;
                    }
                }
                else
                {
                    item.EtlPackageStatus = EtlPackageMonitorItemStatus.Never;
                }

                if (item.EtlPackageStatus != EtlPackageMonitorItemStatus.Succeeded)
                {
                    model.HasErrors = true;
                }

                if (refresh)
                {
                    var pack = model.Items.FirstOrDefault(i => String.Equals(i.EtlPackageId, package.Id, StringComparison.OrdinalIgnoreCase));
                    if (pack != null)
                    {
                        pack.UpdateFromItem(item);
                    }
                }
                else
                {
                    var columnIndex = packageIndex % SiteConfiguration.MaxCoulmnsToShow;
                    model.ItemsByColumns[columnIndex].Add(item);
                }

                packageIndex++;
            }

            for (var columnIndex = 0; columnIndex < model.ItemsByColumns.Length; columnIndex++)
            {
                if (model.MaxItemCountInColumn < model.ItemsByColumns[columnIndex].Count)
                {
                    model.MaxItemCountInColumn = model.ItemsByColumns[columnIndex].Count;
                }
            }

            model.LastUpdated = DateTime.Now;

            return model;
        }

        #endregion
    }
}