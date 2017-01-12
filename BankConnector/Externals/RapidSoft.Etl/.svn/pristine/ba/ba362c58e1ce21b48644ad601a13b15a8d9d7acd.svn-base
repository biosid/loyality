using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Logging.Dumps;
using RapidSoft.Etl.Monitor.Helpers;
using RapidSoft.Etl.Monitor.Models;
using RapidSoft.Etl.Monitor.Resources.Views.Shared;

namespace RapidSoft.Etl.Monitor.Controllers
{
    using RapidSoft.Etl.Runtime;

    public class EtlSessionListController : AsyncController
    {
        #region Actions

        [HttpGet]
        public ActionResult EtlSessionList(EtlSessionListModel model)
        {
            model = SetDefaultModel(model);

            if (ValidateModelExt(model))
            {
                model.Sessions.AddRange(GetEtlSessions(model));
            }

            return View(model);
        }

        #endregion

        #region Methods

        private static EtlSessionListModel SetDefaultModel(EtlSessionListModel model)
        {
            if (model == null)
            {
                model = new EtlSessionListModel();
            }

            var dateFrom = StringToDateTime(model.DateFrom);
            var dateTo = StringToDateTime(model.DateTo);

            if (!dateTo.HasValue)
            {
                dateTo = DateTime.Now;
            }

            if (!dateFrom.HasValue)
            {
                dateFrom = dateTo.Value.Subtract(new TimeSpan(model.DefaultTimespanDays, 0, 0, 0));
            }

            model.DateFrom = dateFrom.Value.ToString(SharedStrings.DateFormat);
            model.DateTo = dateTo.Value.ToString(SharedStrings.DateFormat);
            model.Sessions = new List<EtlSession>();

            if (model.EtlPackagesId == "0")
            {
                model.EtlPackagesId = null;
            }

            model.EtlPackages = GetPackages();

            return model;
        }

        private static IEnumerable<EtlPackage> GetPackages()
        {
            var agent = SiteConfiguration.GetEtlAgent();
            var retVal = agent.GetEtlPackages();
            return retVal;
        }

        private bool ValidateModelExt(EtlSessionListModel model)
        {
            var wasErrors = false;

            var dateFrom = StringToDateTime(model.DateFrom);
            var dateTo = StringToDateTime(model.DateTo);

            if (!dateTo.HasValue)
            {
                dateTo = DateTime.Now;
            }

            if (!dateFrom.HasValue)
            {
                dateFrom = dateTo.Value.Subtract(new TimeSpan(model.DefaultTimespanDays, 0, 0, 0));
            }

            if (dateFrom.Value > dateTo.Value)
            {
                this.AddErrorMessageFor<EtlSessionListModel, string>(m => m.DateFrom, "Начальная дата должна быть меньше конечной");
                wasErrors = true;
            }

            return !wasErrors;
        }

        private EtlSession[] GetEtlSessions(EtlSessionListModel model)
        {
            // TODO: Поиск сессий по пакетам.
            var dateTo = DateTime.ParseExact(model.DateTo, SharedStrings.DateFormat, null);
            var dateFrom = DateTime.ParseExact(model.DateFrom, SharedStrings.DateFormat, null);
            var oneDay = dateTo == dateFrom;
            dateTo = dateTo.AddDays(1);

            var query = new EtlSessionQuery
            {
                FromDateTime = dateFrom,
                ToDateTime = dateTo,
                MaxSessionCount = model.DefaultMaxSessionCount,
            };

            if (model.Status > 0)
            {
                query.EtlStatuses.Add(model.Status);
            }

            if (!string.IsNullOrWhiteSpace(model.EtlPackagesId))
            {
                query.EtlPackageIds.Add(model.EtlPackagesId);
            }

            var agent = SiteConfiguration.GetEtlAgent();
            var logParser = agent.GetEtlLogParser();
            var sessions = logParser.GetEtlSessions(query);

            return sessions;
        }

        //private EtlDump GetEtlDump(EtlSessionListModel model)
        //{
        //    var dateTo = DateTime.ParseExact(model.DateTo, SharedStrings.DateFormat, null);
        //    var dateFrom = DateTime.ParseExact(model.DateFrom, SharedStrings.DateFormat, null);
        //    var oneDay = dateTo == dateFrom;
        //    dateTo = dateTo.AddDays(1);

        //    var query = new EtlSessionQuery
        //    {
        //        FromDateTime = dateFrom,
        //        ToDateTime = dateTo,
        //        MaxSessionCount = model.DefaultMaxSessionCount,
        //    };
            
        //    var agent = SiteConfiguration.GetEtlAgent();

        //    var writer = new EtlDumpWriter(new EtlDumpSettings
        //    {
        //        MaxContentSizeBytes = 0
        //    });
        //    writer.Write(query, agent);

        //    var dump = writer.GetDump();
        //    return dump;
        //}

        private static DateTime? StringToDateTime(string str)
        {
            DateTime dt;

            if (DateTime.TryParse(str, out dt))
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

    	#endregion
    }
}