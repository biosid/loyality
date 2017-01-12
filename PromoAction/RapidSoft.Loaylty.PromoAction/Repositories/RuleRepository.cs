namespace RapidSoft.Loaylty.PromoAction.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Objects.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;
    using RapidSoft.Loaylty.PromoAction.Api.InputParameters;
    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;
    using RapidSoft.Loaylty.PromoAction.Mechanics;
    using RapidSoft.Loaylty.PromoAction.Repositories.Core;
    using RapidSoft.Loaylty.PromoAction.Service;

    /// <summary>
    /// Репозиторий для сущности <see cref="Rule"/>.
    /// </summary>
    public class RuleRepository : TraceableEntityRepository<Rule>, IRuleRepository
    {
        /// <summary>
        /// Получение коллекции правил по названию домена правил
        /// </summary>
        /// <param name="ruleDomainName">
        /// Название домена правил
        /// </param>
        /// <returns>
        /// Коллекция правил домена
        /// </returns>
        public IList<Rule> GetRulesByRuleDomainName(string ruleDomainName)
        {
            var retVal = this.GetEntityDbSet().Where(x => x.RuleDomain.Name == ruleDomainName).ToList();
            return retVal;
        }

        public Page<RuleHistory> GetHistory(GetRuleHistoryParameters parameters)
        {
            parameters.ThrowIfNull("parameters");
            var inner = parameters;

            IQueryable<RuleHistory> query = this.Context.Set<RuleHistory>();

            query = query.Where(x => x.RuleId == inner.RuleId);

            int? totalCount = null;

            if (inner.CalcTotalCount.HasValue && inner.CalcTotalCount.Value)
            {
                totalCount = query.Count();
            }

            query = query.OrderByDescending(x => x.HistoryId);

            if (inner.CountSkip.HasValue)
            {
                var skip = inner.CountSkip.Value;
                query = query.Skip(skip);
            }

            if (inner.CountTake.HasValue)
            {
                var take = inner.CountTake.Value;
                query = query.Take(take);
            }

            var ruleHistories = query.ToList();

            return new Page<RuleHistory>(ruleHistories, inner.CountSkip, inner.CountTake, totalCount);
        }

        /// <summary>
        /// Получение набора правил по уникальным идентификаторам.
        /// </summary>
        /// <param name="ids">
        /// Уникальные идентификаторы.
        /// </param>
        /// <returns>
        /// Коллекция правил.
        /// </returns>
        public IList<Rule> GetRules(long[] ids)
        {
            var retVal = this.GetEntityDbSet().Where(x => ids.Contains(x.Id)).ToList();
            return retVal;
        }

        /// <summary>
        /// Получение коллекции правил по условиям <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters">
        /// Условия отбора правил.
        /// </param>
        /// <returns>
        /// Коллекция правил.
        /// </returns>
        public Page<Rule> GetRules(GetRulesParameters parameters = null)
        {
            var inner = parameters ?? new GetRulesParameters();

            IQueryable<Rule> query = this.GetEntityDbSet();

            query = this.Where(query, inner);

            var totalCount = GetTotalCount(inner, query);

            query = SetOrder(inner, query);

            query = SetPaging(inner, query);

            var rules = query.ToList();

            return new Page<Rule>(rules, inner.CountSkip, inner.CountTake, totalCount);
        }

        public Page<Rule> GetPromoAction(GetRulesParameters parameters = null)
        {
            var inner = parameters ?? new GetRulesParameters();

            IQueryable<Rule> query = this.GetEntityDbSet();

            query = this.Where(query, inner);

            query = query.Where(x => x.Type != RuleTypes.BaseAddition && x.Type != RuleTypes.BaseMultiplication);

            var totalCount = GetTotalCount(inner, query);

            query = SetOrder(inner, query);

            query = SetPaging(inner, query);

            var rules = query.ToList();

            return new Page<Rule>(rules, inner.CountSkip, inner.CountTake, totalCount);
        }

        private static IQueryable<Rule> SetPaging(GetRulesParameters inner, IQueryable<Rule> query)
        {
            if (inner.CountSkip.HasValue)
            {
                var skip = inner.CountSkip.Value;
                query = query.Skip(skip);
            }

            if (inner.CountTake.HasValue)
            {
                var take = inner.CountTake.Value;
                query = query.Take(take);
            }
            return query;
        }

        private static int? GetTotalCount(GetRulesParameters inner, IQueryable<Rule> query)
        {
            int? totalCount = null;

            if (inner.CalcTotalCount.HasValue && inner.CalcTotalCount.Value)
            {
                totalCount = query.Count();
            }

            return totalCount;
        }

        private static IQueryable<Rule> SetOrder(GetRulesParameters inner, IQueryable<Rule> query)
        {
            var sortProperty = inner.SortProperty ?? SortProperty.Priority;
            var sortOrder = inner.SortDirect ?? SortDirections.Asc;

            switch (sortProperty)
            {
                case SortProperty.DateTimeTo:
                    query = OrderBy(query, sortOrder, rule => rule.DateTimeTo);
                    break;
                default:
                    query = OrderBy(query, sortOrder, rule => rule.Priority);
                    break;
            }

            return query;
        }

        /// <summary>
        /// Получение коллекции актуальных по дате правил по названию домена правил
        /// </summary>
        /// <param name="ruleDomainName">
        /// Название домена правил
        /// </param>
        /// <param name="dateTime">
        /// Дата и время для поиска актульных правил. По умолчанию текущие дата и время.
        /// </param>
        /// <returns>
        /// Коллекция правил домена
        /// </returns>
        public IList<Rule> GetActualRulesByRuleDomainName(string ruleDomainName, DateTime? dateTime = null)
        {
            var innerDate = dateTime.HasValue ? dateTime.Value : DateTime.Now;
            return
                this.GetEntityDbSet()
                    .Where(x => x.RuleDomain.Name == ruleDomainName)
                    .Where(x => x.Status == RuleStatuses.Active && x.Approved == ApproveStatus.Approved)
                    .Where(x => x.DateTimeFrom == null || (x.DateTimeFrom != null && x.DateTimeFrom < innerDate))
                    .Where(x => x.DateTimeTo == null || (x.DateTimeTo != null && x.DateTimeTo > innerDate))
                    .ToList();
        }

        /// <summary>
        /// Сохранение правила в хранилище.
        /// </summary>
        /// <param name="entity">
        /// Сохраняемое правило.
        /// </param>
        public override void Save(Rule entity)
        {
            entity.ThrowIfNull("entity");

            this.BeforeSave(entity);

            var fromDb = this.Get(entity.Id);

            if (fromDb == null)
            {
                if (!entity.IsBase())
                {
                    // NOTE: Для не базовых классов принудительно сбрасываем Approved
                    entity.Approved = ApproveStatus.NotApproved;
                    entity.ApproveDescription = null;
                }

                entity.InsertedDate = DateTime.Now;
                this.ExecuteSave(entity);
            }
            else
            {
                fromDb.ResetFrom(entity);

                fromDb.UpdatedDate = DateTime.Now;
                this.ExecuteSave(fromDb);
            }
        }

        public void Save(IList<Rule> rules)
        {
            foreach (var rule in rules)
            {
                this.Save(rule);
            }
        }

        public void SaveApprove(long ruleId, bool approve, string reason)
        {
            var approved = new Approve { RuleId = ruleId, IsApproved = approve, Reason = reason };
            var approves = new[] { approved };
            this.SaveApproves(approves);
        }

        public void SaveApproves(IList<Approve> approves)
        {
            var ids = approves.Select(x => x.RuleId);
            var rules = this.GetEntityDbSet().Where(x => ids.Contains(x.Id)).ToList();

            foreach (var source in rules.Join(
                approves,
                rule => rule.Id,
                approve => approve.RuleId,
                (rule, approve) => new { rule, approve }))
            {
                source.rule.Approved = source.approve.IsApproved ? ApproveStatus.Approved : ApproveStatus.Correction;
                source.rule.ApproveDescription = source.approve.Reason;
            }

            this.Commit();
        }

        /// <summary>
        /// Удаление правила из хранилища по уникальному идентификатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <param name="userId">
        /// Идентификатор пользователя выполняющего удаление.
        /// </param>
        public override void DeleteById(long id, string userId)
        {
            var entity = this.GetEntityDbSet().Find(id);

            if (entity == null)
            {
                return;
            }

            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedUserId = userId;

            this.ExecuteDelete(entity);
        }

        //public void SetRulesExternalStatusIds(IDictionary<long, string> ruleIdStatusIdPairs)
        //{
        //    var ids = ruleIdStatusIdPairs.Keys;
        //    var rules = this.GetEntityDbSet().Where(x => ids.Contains(x.Id)).ToList();

        //    var updated = rules.Join(
        //        ruleIdStatusIdPairs,
        //        rule => rule.Id,
        //        pair => pair.Key,
        //        (rule, pair) =>
        //        {
        //            rule.ExternalStatusId = pair.Value;
        //            return rule;
        //        }).ToList();

        //    this.Save(updated);
        //}

        /// <summary>
        /// Выполняет проверку сущности перед сохранением:
        /// 1. Проверка уникальности приоритета базового правила в рамках домена
        /// 2. Проверка уникальности приоритета исключающего не базового правила
        /// </summary>
        /// <param name="entity">
        /// Сохраняемая сущность.
        /// </param>
        protected override void BeforeSave(Rule entity)
        {
            if (entity == null)
            {
                return;
            }

            if (entity.IsBase())
            {
                var rule =
                    this.GetEntityDbSet()
                        .FirstOrDefault(
                            x =>
                            x.RuleDomainId == entity.RuleDomainId && x.Type == entity.Type && x.Priority == entity.Priority
                            && x.Id != entity.Id);
                if (rule != null)
                {
                    var mess =
                        string.Format(
                            "Нельзя выполнить сохранение, так как уже существует в домене правило с типом {0} и приоритетом {1}: id правила = {2}",
                            rule.Type,
                            rule.Priority,
                            rule.Id);
                    throw new OperationException(ResultCodes.INVALID_PRIORITY, mess);
                }
            }

            if (entity.IsExclusive)
            {
                var rule =
                    this.GetEntityDbSet()
                        .FirstOrDefault(
                            x =>
                            x.RuleDomainId == entity.RuleDomainId && x.Type == entity.Type && x.IsExclusive
                            && x.Priority == entity.Priority && x.Id != entity.Id);
                if (rule != null)
                {
                    var mess =
                        string.Format(
                            "Нельзя выполнить сохранение, так как уже существует в домене исключающее правило с типом {0} и приоритетом {1}: id правила = {2}",
                            rule.Type,
                            rule.Priority,
                            rule.Id);
                    throw new OperationException(ResultCodes.INVALID_PRIORITY, mess);
                }
            }
        }

        private IQueryable<Rule> Where(IQueryable<Rule> query, GetRulesParameters parameters)
        {
            parameters.ThrowIfNull("parameters");

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var term = parameters.SearchTerm;
                query = query.Where(x => x.Name.Contains(term) || SqlFunctions.StringConvert((decimal)x.Id).Contains(term));
            }

            if (parameters.DateTimeFrom.HasValue)
            {
                var from = parameters.DateTimeFrom.Value;

                // NOTE: Берем которые на дату from не кончились
                query = query.Where(x => (x.DateTimeTo == null || x.DateTimeTo >= @from));
            }

            if (parameters.DateTimeTo.HasValue)
            {
                var to = parameters.DateTimeTo.Value;

                // NOTE: Берем которые на дату to уже начались
                query = query.Where(x => x.DateTimeFrom == null || x.DateTimeFrom <= to);
            }

            if (parameters.DateTimeUntil.HasValue)
            {
                var until = parameters.DateTimeUntil.Value;

                // NOTE: Берем которые на дату уже завершились.
                query = query.Where(x => (x.DateTimeTo != null && x.DateTimeTo <= until));
            }

            if (parameters.RuleDomainId.HasValue)
            {
                var ruleDomainId = parameters.RuleDomainId.Value;
                query = query.Where(x => x.RuleDomainId == ruleDomainId);
            }

            if (parameters.Status.HasValue)
            {
                var status = parameters.Status.Value;
                query = query.Where(x => x.Status == status);
            }

            if (parameters.Type.HasValue)
            {
                var type = parameters.Type.Value;
                query = query.Where(x => x.Type == type);
            }

            if (parameters.ApproveStatuses != null && parameters.ApproveStatuses.Length > 0)
            {
                var approveStatuses = parameters.ApproveStatuses;
                query = query.Where(x => approveStatuses.Contains(x.Approved));
            }

            return query;
        }

        private static IQueryable<Rule> OrderBy<T>(IQueryable<Rule> query, SortDirections sortOrder, Expression<Func<Rule, T>> expression)
        {
            return sortOrder == SortDirections.Desc ? query.OrderByDescending(expression) : query.OrderBy(expression);
        }

        private static IEnumerable<Rule> OrderBy<T>(IEnumerable<Rule> query, SortDirections sortOrder, Func<Rule, T> expression)
        {
            return sortOrder == SortDirections.Desc ? query.OrderByDescending(expression) : query.OrderBy(expression);
        }
    }
}