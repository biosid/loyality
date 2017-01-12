namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime.Agents;
    using RapidSoft.Etl.Runtime.Agents.Sql;
    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Import;
    using RapidSoft.Loaylty.ProductCatalog.Settings;

    using Services;

    internal class ImportTaskRepository : IImportTaskRepository
    {
        /// <summary>
        /// Строка подключения.
        /// </summary>
        private readonly string connectionString;

        public ImportTaskRepository()
        {
            this.connectionString = DataSourceConfig.ConnectionString;
        }

        public ImportTaskRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ProductImportTask SaveProductImportTask(ProductImportTask task)
        {
            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                if (task.Id == default(int))
                {
                    ctx.Entry(task).State = EntityState.Added;
                }
                else
                {
                    ctx.ProductImportTasks.Attach(task);
                    ctx.Entry(task).State = EntityState.Modified;
                }

                ctx.SaveChanges();
                return task;
            }
        }

        public ProductImportTask GetProductImportTask(int id)
        {
            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                var retVal = ctx.ProductImportTasks.SingleOrDefault(x => x.Id == id);
                return retVal;
            }
        }

        public Page<ProductImportTask> GetPageProductImportTask(int? partnerId, int? skipCount, int? takeCount, bool? calcTotalCount)
        {
            using (var ctx = new LoyaltyDBEntities(this.connectionString))
            {
                var calcTotal = calcTotalCount ?? false;

                int? total = null;

                IQueryable<ProductImportTask> query = ctx.ProductImportTasks;

                if (partnerId.HasValue)
                {
                    query = query.Where(x => x.PartnerId == partnerId.Value);
                }

                if (calcTotal)
                {
                    total = query.Count();
                }

                query = query.OrderByDescending(x => x.InsertedDate).ThenByDescending(x => x.StartDateTime);

                if (skipCount.HasValue)
                {
                    query = query.Skip(skipCount.Value);
                }

                if (takeCount.HasValue)
                {
                    query = query.Take(takeCount.Value);
                }

                var list = query.ToList();

                return new Page<ProductImportTask>(list, skipCount, takeCount, total);
            }
        }

        public Page<DeliveryRateImportTask> GetPageDeliveryRateImportTask(int? partnerId, int? skipCount, int? takeCount, bool? calcTotalCount)
        {
            var agentInfo = new EtlAgentInfo
                            {
                                ConnectionString = connectionString,
                                SchemaName = ApiSettings.EtlSchemaName,
                            };
            var agent = new SqlEtlAgent(agentInfo);
            var parser = agent.GetEtlLogParser();

            var query = this.BuildEtlSessionPagedQuery(partnerId, skipCount, takeCount, calcTotalCount);
            var etlPackageIds = query.EtlPackageIds.ToArray();

            int? totalCount = null;

            var sessions = parser.GetEtlSessions(query, out totalCount);
            var etlSessionIds = sessions.Select(x => x.EtlSessionId).ToArray();

            var variables = parser.GetEtlVariables(etlPackageIds, etlSessionIds);
            var counters = parser.GetEtlCounters(etlPackageIds, etlSessionIds);

            var deliveryRateImportTasks = this.BuildDeliveryRateImportTasks(sessions, variables, counters);

            return new Page<DeliveryRateImportTask>(deliveryRateImportTasks, skipCount, takeCount, totalCount);
        }

        private IEnumerable<DeliveryRateImportTask> BuildDeliveryRateImportTasks(EtlSession[] sessions, EtlVariable[] variables, EtlCounter[] counters)
        {
            var joinedSessionsAndVariables = sessions.GroupJoin(
                variables,
                session => session.EtlSessionId,
                variable => variable.EtlSessionId,
                (session, sessioinVariables) => new
                                                {
                                                    Session = session,
                                                    Variables = sessioinVariables.ToArray()
                                                });

            var joined = joinedSessionsAndVariables.GroupJoin(
                counters,
                arg => arg.Session.EtlSessionId,
                counter => counter.EtlSessionId,
                (sessionWithVariables, sessionCounters) => new
                                                           {
                                                               sessionWithVariables.Session,
                                                               sessionWithVariables.Variables,
                                                               Counters = sessionCounters.ToArray()
                                                           });

            var deliveryRateImportTasks = joined.Select(x => this.Build(x.Session, x.Variables, x.Counters));
            return deliveryRateImportTasks;
        }

        private DeliveryRateImportTask Build(EtlSession session, EtlVariable[] variables, EtlCounter[] counters)
        {
            var partnerId = variables.First(x => x.Name == "PartnerId").Value;
            var fileUrl = variables.First(x => x.Name == "FileUrl").Value;

            var importedCounterNames = new[] { "Импортировано без ошибок", "КЛАДР не указан или не корректен" };

            var successCount = counters.Where(x => importedCounterNames.Contains(x.CounterName)).Sum(x => x.CounterValue);
            var errorCount = counters.Where(x => x.CounterName == "Ошибочные (не импортировано)").Sum(x => x.CounterValue);

            var status = ConvertToTaskStatus(session.Status);

            var retVal = new DeliveryRateImportTask
                         {
                             Id = session.EtlSessionId,
                             CountFail = Convert.ToInt32(errorCount),
                             CountSuccess = Convert.ToInt32(successCount),
                             InsertDateTime = session.InsertDateTime,
                             StartDateTime = session.StartDateTime,
                             EndDateTime = session.EndDateTime,
                             FileUrl = fileUrl,
                             PartnerId = int.Parse(partnerId),
                             InsertedUserId = session.UserName,
                             Status = status
                         };

            return retVal;
        }

        private ImportTaskStatuses ConvertToTaskStatus(EtlStatus etlStatus)
        {
            ImportTaskStatuses status;
            switch (etlStatus)
            {
                case EtlStatus.Waiting:
                    {
                        status = ImportTaskStatuses.Waiting;
                        break;
                    }

                case EtlStatus.Started:
                    {
                        status = ImportTaskStatuses.Loading;
                        break;
                    }

                case EtlStatus.Succeeded:
                case EtlStatus.FinishedWithLosses:
                case EtlStatus.FinishedWithWarnings:
                case EtlStatus.FinishedWithErrors:
                    {
                        status = ImportTaskStatuses.Completed;
                        break;
                    }

                case EtlStatus.Failed:
                    {
                        status = ImportTaskStatuses.Error;
                        break;
                    }

                default:
                    {
                        var mess = string.Format("Тип {0} не поддерживается", etlStatus);
                        throw new NotSupportedException(mess);
                    }
            }

            return status;
        }

        private EtlSessionPagedQuery BuildEtlSessionPagedQuery(
            int? partnerId, int? skipCount, int? takeCount, bool? calcTotalCount)
        {
            var query = new EtlSessionPagedQuery
                        {
                            CountToSkip = skipCount ?? 0,
                            CountToTake = takeCount ?? ApiSettings.MaxResultsCountImportDeliveryRateTasks,
                            CalcCount = calcTotalCount ?? false
                        };

            if (partnerId.HasValue)
            {
                query.Variables.Add(new EtlVariableFilter("PartnerId", partnerId.Value.ToString(CultureInfo.InvariantCulture)));
            }

            var packageIds = this.GetImportDeliveryRatesEtlPackageIds();

            query.EtlPackageIds.AddRange(packageIds);
            return query;
        }

        private IEnumerable<string> GetImportDeliveryRatesEtlPackageIds()
        {
            var importDeliveryRatesEtlPackageIds = ApiSettings.ImportDeliveryRatesEtlPackageIds;

            var packageIds = importDeliveryRatesEtlPackageIds.Split(
                ','.MakeArray(), StringSplitOptions.RemoveEmptyEntries);

            var retVal =
                packageIds.Union(PartnerSettingsExtension.DefaultImportDeliveryRatesEtlPackageId.ToString().MakeArray()).ToArray();

            return retVal;
        }
    }
}