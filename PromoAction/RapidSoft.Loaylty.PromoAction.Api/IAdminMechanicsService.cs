namespace RapidSoft.Loaylty.PromoAction.Api
{
    using System.ServiceModel;

    using Monitoring;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.InputParameters;
    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;

    /// <summary>
    /// Интерфейс админмистративного сервиса для управления данными расчета механик.
    /// </summary>
    [ServiceContract]
    public interface IAdminMechanicsService : ISupportService
    {
        /// <summary>
        /// Получение метаданных домена.
        /// </summary>
        /// <param name="id">
        /// Идентификатор домена.
        /// </param>
        /// <returns>
        /// The <see cref="GetMetadataByDomainIdResult"/>.
        /// </returns>
        [OperationContract]
        GetMetadataByDomainIdResult GetMetadataByDomainId(long id, string userId);

        /// <summary>
        /// Сохранение домена правил.
        /// </summary>
        /// <param name="ruleDomain">
        /// Сохраняемый домен правил.
        /// </param>
        /// <returns>
        /// Сохраненый домен правил, с присвоенным уникальным идентификатором при необходимости.
        /// </returns>
        [OperationContract]
        RuleDomainResult SaveRuleDomain(RuleDomain ruleDomain, string userId);

        /// <summary>
        /// Получение домена правил по уникальному идентификатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <returns>
        /// Найденный домен правил или <c>null</c>.
        /// </returns>
        [OperationContract]
        RuleDomainResult GetRuleDomain(long id, string userId);

        /// <summary>
        /// Получение коллекции всех доменов правил.
        /// </summary>
        /// <returns>
        /// Коллекция всех доменов правил.
        /// </returns>
        [OperationContract]
        RuleDomainsResult GetAllRuleDomains(string userId);

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
        [OperationContract]
        ResultBase DeleteRuleDomainById(long id, string userId);

        [OperationContract]
        RuleResult CreateRule(Rule rule, string userId);

        [OperationContract]
        RuleResult UpdateRule(Rule rule, string userId);

        /// <summary>
        /// Получение правила/промоакции по уникальному идентификатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <returns>
        /// Найденное правило или <c>null</c>.
        /// </returns>
        [OperationContract]
        RuleResult GetRule(long id, string userId);

        /// <summary>
        /// Получение коллекции правил удовлетворяющих запросу.
        /// </summary>
        /// <param name="parameters">
        /// Параметр фильтрации правил.
        /// </param>
        /// <returns>
        /// Коллекция правил.
        /// </returns>
        [OperationContract]
        RulesResult GetRules(GetRulesParameters parameters);

        /// <summary>
        /// Получение коллекции промоакций привязанных к целевой аудитории удовлетворяющих запросу.
        /// Промоакциями является правило которые в своем предикате использует переменную ClientProfile.Audiences (object="ClientProfile" name="Audiences"). 
        /// Промоакциями привязанными к целевой аудитории являются промоакций где переменная ClientProfile.Audiences сопоставляется с литералом начинающимся на "Audience_".
        /// PS. ClientProfile и Audiences - задаются настройками ClientProfileObjectName и PromoActionPropertyName в конфиг файле.
        /// </summary>
        /// <param name="parameters">Параметр фильтрации промоакции.</param>
        /// <returns>Коллекция правил.</returns>
        [OperationContract]
        RulesResult GetPromoActions(GetRulesParameters parameters);

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
        [OperationContract]
        ResultBase DeleteRuleById(long id, string userId);

        [OperationContract]
        ResultBase DeactivateRule(long id, string userId);

        [OperationContract]
        ResultBase DeactivateRules(long[] ids, string userId);

        [OperationContract]
        GetRuleHistoryResult GetRuleHistory(GetRuleHistoryParameters parameters);

        [OperationContract]
        ResultBase SetRuleApproved(Approve[] approves, string userId);
    }
}
