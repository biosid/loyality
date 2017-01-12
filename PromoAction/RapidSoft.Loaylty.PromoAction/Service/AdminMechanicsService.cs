namespace RapidSoft.Loaylty.PromoAction.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Monitoring;

    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.Logging.Wcf;
    using RapidSoft.Loaylty.PromoAction.Api;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.InputParameters;
    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;
    using RapidSoft.Loaylty.PromoAction.Repositories;
    using RapidSoft.Loaylty.PromoAction.Repositories.Core;
    using RapidSoft.Loaylty.PromoAction.Settings;
    using RapidSoft.Loaylty.PromoAction.Wcf;
    using RapidSoft.VTB24.ArmSecurity;

    /// <summary>
    /// Реализация сервиса управления данными расчета механик, <see cref="IAdminMechanicsService"/>.
    /// </summary>
    [DbContextBehavior(true), LoggingBehavior]
    public class AdminMechanicsService : SupportService, IAdminMechanicsService
    {
        private readonly ILog log = LogManager.GetLogger(typeof(AdminMechanicsService));

        /// <summary>
        /// The rule repository.
        /// </summary>
        private readonly IRuleRepository ruleRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminMechanicsService"/> class.
        /// </summary>
        public AdminMechanicsService()
            : this(new RuleRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminMechanicsService"/> class.
        /// </summary>
        /// <param name="ruleRepository">
        /// The rule repository.
        /// </param>
        public AdminMechanicsService(IRuleRepository ruleRepository = null)
        {
            this.ruleRepository = ruleRepository ?? new RuleRepository();
        }

        /// <summary>
        /// Получение метаданных домена.
        /// </summary>
        /// <param name="id">
        /// Идентификатор домена.
        /// </param>
        /// <returns>
        /// The <see cref="GetMetadataByDomainIdResult"/>.
        /// </returns>
        public GetMetadataByDomainIdResult GetMetadataByDomainId(long id, string userId)
        {
            try
            {
                ArmSecurityHelper.CheckPermissions(userId);

                var retRuleDomain = this.Get<RuleDomain, long, RuleDomainRepository>(id);

                if (retRuleDomain == null)
                {
                    var mess = string.Format("Домен правил с идентификатором {0} не найден", id);
                    log.Warn(mess);

                    return GetMetadataByDomainIdResult.BuildFail(ResultCodes.RULE_DOMAIN_NOT_FOUND, mess);
                }

                var entitiesMetadata = retRuleDomain.GetDeserializedMetadata();

                return GetMetadataByDomainIdResult.BuidSuccess(entitiesMetadata);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения метаданных домена правил", ex);
                return ServiceOperationResult.BuildErrorResult<GetMetadataByDomainIdResult>(ex);
            }
        }

        /// <summary>
        /// Сохранение домена правил.
        /// </summary>
        /// <param name="ruleDomain">
        /// Сохраняемый домен правил.
        /// </param>
        /// <returns>
        /// Сохраненый домен правил, с присвоенным уникальным идентификатором при необходимости.
        /// </returns>
        public RuleDomainResult SaveRuleDomain(RuleDomain ruleDomain, string userId)
        {
            try
            {
                ArmSecurityHelper.CheckPermissions(userId, ArmPermissions.PromoCreateUpdateDelete);

                ruleDomain.UpdatedUserId = userId;

                var retRuleDomain = this.Save<RuleDomain, long, RuleDomainRepository>(ruleDomain);
                return RuleDomainResult.BuildSuccess(retRuleDomain);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка сохранения домена правил", ex);
                return ServiceOperationResult.BuildErrorResult<RuleDomainResult>(ex);
            }
        }

        /// <summary>
        /// Получение домена правил по уникальному идентификатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <returns>
        /// Найденный домен правил или <c>null</c>.
        /// </returns>
        public RuleDomainResult GetRuleDomain(long id, string userId)
        {
            try
            {
                ArmSecurityHelper.CheckPermissions(userId);

                var retRuleDomain = this.Get<RuleDomain, long, RuleDomainRepository>(id);
                return RuleDomainResult.BuildSuccess(retRuleDomain);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения домена правил по уникальному идентификатору", ex);
                return ServiceOperationResult.BuildErrorResult<RuleDomainResult>(ex);
            }
        }

        /// <summary>
        /// Получение коллекции всех доменов правил.
        /// </summary>
        /// <returns>
        /// Коллекция всех доменов правил.
        /// </returns>
        public RuleDomainsResult GetAllRuleDomains(string userId)
        {
            try
            {
                ArmSecurityHelper.CheckPermissions(userId);

                var retRuleDomains = this.GetAll<RuleDomain, long, RuleDomainRepository>();
                return RuleDomainsResult.BuildSuccess(retRuleDomains);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения коллекции всех доменов правил", ex);
                return ServiceOperationResult.BuildErrorResult<RuleDomainsResult>(ex);
            }
        }

        /// <summary>
        /// Удаление домена правил по уникальному идентификатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <param name="userId">
        /// Идентификатор пользователя выполняющего удаление.
        /// </param>
        /// <returns>
        /// The <see cref="ResultBase"/>.
        /// </returns>
        public ResultBase DeleteRuleDomainById(long id, string userId)
        {
            try
            {
                ArmSecurityHelper.CheckPermissions(userId, ArmPermissions.PromoCreateUpdateDelete);

                this.DeleteById<RuleDomain, long, RuleDomainRepository>(id, userId);
                return ResultBase.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.Error("Ошибка удаления домена правил", ex);
                return ServiceOperationResult.BuildErrorResult<RuleDomainsResult>(ex);
            }
        }

        public RuleResult CreateRule(Rule rule, string userId)
        {
            try
            {
                rule.ThrowIfNull("rule");
                userId.ThrowIfNull("userId");

                ArmSecurityHelper.CheckPermissions(userId, ArmPermissions.PromoCreateUpdateDelete);

                var ruleDomain = this.Get<RuleDomain, long, RuleDomainRepository>(rule.RuleDomainId);
                if (ruleDomain == null)
                {
                    var message = string.Format("Домен правил с идентификатором {0} не найден", rule.RuleDomainId);
                    return RuleResult.BuildFail(ResultCodes.RULE_DOMAIN_NOT_FOUND, message);
                }
                
                rule.Id = 0;
                rule.UpdatedUserId = userId;
                this.ruleRepository.Save(rule);
                return RuleResult.BuildSuccess(rule);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка сохранения правила", ex);
                return ServiceOperationResult.BuildErrorResult<RuleResult>(ex);
            }
        }

        public RuleResult UpdateRule(Rule rule, string userId)
        {
            try
            {
                rule.ThrowIfNull("rule");
                userId.ThrowIfNull("userId");

                ArmSecurityHelper.CheckPermissions(userId, ArmPermissions.PromoCreateUpdateDelete);

                var exists = this.ruleRepository.Get(rule.Id);

                if (exists == null)
                {
                    var mess = string.Format("Правило с идентификатором {0} не найдено", rule.Id);
                    return RuleResult.BuildFail(ResultCodes.RULE_NOT_FOUND, mess);
                }

                var ruleDomain = this.Get<RuleDomain, long, RuleDomainRepository>(rule.RuleDomainId);
                if (ruleDomain == null)
                {
                    var message = string.Format("Домен правил с идентификатором {0} не найден", rule.RuleDomainId);
                    return RuleResult.BuildFail(ResultCodes.RULE_DOMAIN_NOT_FOUND, message);
                }

                rule.UpdatedUserId = userId;
                this.ruleRepository.Save(rule);
                return RuleResult.BuildSuccess(rule);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка сохранения правила", ex);
                return ServiceOperationResult.BuildErrorResult<RuleResult>(ex);
            }
        }

        /// <summary>
        /// Сохранение правила.
        /// </summary>
        /// <param name="rule">
        /// Сохранямое правило.
        /// </param>
        /// <param name="userId"></param>
        /// <returns>
        /// Сохраненое правило, с присвоенным уникальным идентификатором при необходимости.
        /// </returns>
        public RuleResult SaveRule(Rule rule, bool approve, string userId)
        {
            try
            {
                ArmSecurityHelper.CheckPermissions(userId, ArmPermissions.PromoCreateUpdateDelete);

                rule.UpdatedUserId = userId;
                this.ruleRepository.Save(rule);
                if (approve)
                {
                    this.ruleRepository.SaveApprove(rule.Id, true, "Принудительное подтверждение при создании, для тестов");
                }

                return RuleResult.BuildSuccess(rule);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка сохранения правила", ex);
                return ServiceOperationResult.BuildErrorResult<RuleResult>(ex);
            }
        }

        /// <summary>
        /// Получение правила по уникальному идентификатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <returns>
        /// Найденное правило или <c>null</c>.
        /// </returns>
        public RuleResult GetRule(long id, string userId)
        {
            try
            {
                ArmSecurityHelper.CheckPermissions(userId);

                var retRule = this.ruleRepository.Get(id);

                if (retRule == null)
                {
                    var mess = string.Format("Правило с идентификатором {0} не найдено", id);
                    return RuleResult.BuildFail(ResultCodes.RULE_NOT_FOUND, mess);
                }

                return RuleResult.BuildSuccess(retRule);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения правила по уникальному идентификатору", ex);
                return ServiceOperationResult.BuildErrorResult<RuleResult>(ex);
            }
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
        public RulesResult GetRules(GetRulesParameters parameters)
        {
            try
            {
                var inner = parameters ?? new GetRulesParameters();

                ArmSecurityHelper.CheckPermissions(inner.UserId);

                if ((!inner.CountTake.HasValue) || (inner.CountTake.Value > ApiSettings.MaxResultsCountRules))
                {
                    inner.CountTake = ApiSettings.MaxResultsCountRules;
                }

                var retRule = this.ruleRepository.GetRules(parameters);

                return RulesResult.BuildSuccess(retRule, retRule.TotalCount, (int)inner.CountTake);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения правил", ex);
                return ServiceOperationResult.BuildErrorResult<RulesResult>(ex);
            }
        }

        public RulesResult GetPromoActions(GetRulesParameters parameters)
        {
            try
            {
                var inner = parameters ?? new GetRulesParameters();

                ArmSecurityHelper.CheckPermissions(inner.UserId);

                if ((!inner.CountTake.HasValue) || (inner.CountTake.Value > ApiSettings.MaxResultsCountRules))
                {
                    inner.CountTake = ApiSettings.MaxResultsCountRules;
                }

                var retRule = this.ruleRepository.GetPromoAction(parameters);

                return RulesResult.BuildSuccess(retRule, retRule.TotalCount, (int)inner.CountTake);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения промоакций", ex);
                return ServiceOperationResult.BuildErrorResult<RulesResult>(ex);
            }
        }

        /// <summary>
        /// Удаление правила по уникальному идентификатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <param name="userId">
        /// Идентификатор пользователя выполняющего удаление.
        /// </param>
        /// <returns>
        /// The <see cref="ResultBase"/>.
        /// </returns>
        public ResultBase DeleteRuleById(long id, string userId)
        {
            try
            {
                ArmSecurityHelper.CheckPermissions(userId, ArmPermissions.PromoCreateUpdateDelete);

                this.ruleRepository.DeleteById(id, userId);
                return ResultBase.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.Error("Ошибка удаления правила", ex);
                return ServiceOperationResult.BuildErrorResult<RuleDomainsResult>(ex);
            }
        }

        public ResultBase DeactivateRule(long id, string userId)
        {
            return this.DeactivateRules(new[] { id }, userId);
        }

        public ResultBase DeactivateRules(long[] ids, string userId)
        {
            try
            {
                ids.ThrowIfNull("ids");
                userId.ThrowIfNull("userId");

                ArmSecurityHelper.CheckPermissions(userId, ArmPermissions.PromoCreateUpdateDelete);

                var rules = this.ruleRepository.GetRules(ids);

                var notFound = ids.Except(rules.Select(x => x.Id)).ToArray();
                if (notFound.Length > 0)
                {
                    var message = string.Format("Правила с идентификаторами {0} не найдены", string.Join(", ", notFound));
                    return ResultBase.BuildFail(ResultCodes.RULE_NOT_FOUND, message);
                }

                foreach (var rule in rules)
                {
                    rule.Status = RuleStatuses.NotActive;
                    rule.UpdatedUserId = userId;
                }

                this.ruleRepository.Save(rules);
                return ResultBase.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.Error("Ошибка деактивации правила", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        public GetRuleHistoryResult GetRuleHistory(GetRuleHistoryParameters parameters)
        {
            try
            {
                if (parameters == null)
                {
                    return GetRuleHistoryResult.BuildFail(
                        ResultCodes.INVALID_PARAMETER_VALUE, "Не указан идентификатор правила.");
                }

                ArmSecurityHelper.CheckPermissions(parameters.UserId);

                var ruleId = parameters.RuleId;

                var rule = this.ruleRepository.Get(ruleId);
                 
                if (rule == null)
                {
                    var mess = string.Format("Правило с идентификатором {0} не найдено", ruleId);
                    return GetRuleHistoryResult.BuildFail(ResultCodes.RULE_NOT_FOUND, mess);
                }

                if ((!parameters.CountTake.HasValue) || (parameters.CountTake.Value > ApiSettings.MaxResultsCountRuleHistories))
                {
                    parameters.CountTake = ApiSettings.MaxResultsCountRules;
                }

                var retHistory = this.ruleRepository.GetHistory(parameters);

                return GetRuleHistoryResult.BuildSuccess(retHistory, retHistory.TotalCount, parameters.CountTake.Value);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения истории правила по уникальному идентификатору", ex);
                return ServiceOperationResult.BuildErrorResult<GetRuleHistoryResult>(ex);
            }
        }

        public ResultBase SetRuleApproved(Approve[] approves, string userId)
        {
            try
            {
                ArmSecurityHelper.CheckPermissions(userId, ArmPermissions.PromoCreateUpdateDelete);

                this.ruleRepository.SaveApproves(approves);
                return ResultBase.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.Error("Ошибка установки статуса подтверждения акций(правил)", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        /// <summary>
        /// Сохранение сущности.
        /// </summary>
        /// <param name="entity">
        /// Сохранаемая сущность.
        /// </param>
        /// <typeparam name="TEntity">
        /// Тип сущности, реализующее <see cref="IEntity{TKey}"/>
        /// </typeparam>
        /// <typeparam name="TKey">
        /// Тип уникального идентификатора сущности.
        /// </typeparam>
        /// <typeparam name="TRepository">
        /// Тип репозитария сущности, реализующего <see cref="ITraceableEntityRepository{TEntity,TKey}"/>
        /// </typeparam>
        /// <returns>
        /// Сохраненая сущность, с присвоенным уникальным идентификатором при необходимости.
        /// </returns>
        private TEntity Save<TEntity, TKey, TRepository>(TEntity entity) 
            where TEntity : class, IEntity<TKey> 
            where TRepository : class, ITraceableEntityRepository<TEntity, TKey>
        {
            var service = new AdminEntityService<TEntity, TKey, TRepository>();
            return service.Save(entity);
        }

        /// <summary>
        /// Получение сущности по уникальному идентификатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <typeparam name="TEntity">
        /// Тип сущности, реализующее <see cref="IEntity{TKey}"/>
        /// </typeparam>
        /// <typeparam name="TKey">
        /// Тип уникального идентификатора сущности.
        /// </typeparam>
        /// <typeparam name="TRepository">
        /// Тип репозитария сущности, реализующего <see cref="ITraceableEntityRepository{TEntity,TKey}"/>
        /// </typeparam>
        /// <returns>
        /// Экземпляр искомой сущности или <c>null</c>.
        /// </returns>
        private TEntity Get<TEntity, TKey, TRepository>(TKey id)
            where TEntity : class, IEntity<TKey>
            where TRepository : class, ITraceableEntityRepository<TEntity, TKey>
        {
            var service = new AdminEntityService<TEntity, TKey, TRepository>();
            return service.Get(id);
        }

        /// <summary>
        /// Получение коллекции всех сущностей типа <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">
        /// Тип сущности, реализующее <see cref="IEntity{TKey}"/>
        /// </typeparam>
        /// <typeparam name="TKey">
        /// Тип уникального идентификатора сущности.
        /// </typeparam>
        /// <typeparam name="TRepository">
        /// Тип репозитария сущности, реализующего <see cref="ITraceableEntityRepository{TEntity,TKey}"/>
        /// </typeparam>
        /// <returns>
        /// Коллекция всех сущностей типа <typeparamref name="TEntity"/>.
        /// </returns>
        private IList<TEntity> GetAll<TEntity, TKey, TRepository>()
            where TEntity : class, IEntity<TKey>
            where TRepository : class, ITraceableEntityRepository<TEntity, TKey>
        {
            var service = new AdminEntityService<TEntity, TKey, TRepository>();
            return service.GetAll();
        }

        /// <summary>
        /// Удаление сущности по уникальному идентификатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <param name="userId">
        /// Идентификатор пользователя выполняющего удаление.
        /// </param>
        /// <typeparam name="TEntity">
        /// Тип сущности, реализующее <see cref="IEntity{TKey}"/>
        /// </typeparam>
        /// <typeparam name="TKey">
        /// Тип уникального идентификатора сущности.
        /// </typeparam>
        /// <typeparam name="TRepository">
        /// Тип репозитария сущности, реализующего <see cref="ITraceableEntityRepository{TEntity,TKey}"/>
        /// </typeparam>
        private void DeleteById<TEntity, TKey, TRepository>(TKey id, string userId)
            where TEntity : class, IEntity<TKey>
            where TRepository : class, ITraceableEntityRepository<TEntity, TKey>
        {
            var service = new AdminEntityService<TEntity, TKey, TRepository>();
            service.DeleteById(id, userId);
        }
    }
}