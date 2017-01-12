namespace RapidSoft.Loaylty.PromoAction.Repositories
{
    using System;
    using System.Collections.Generic;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;
    using RapidSoft.Loaylty.PromoAction.Api.InputParameters;

    /// <summary>
    /// Интерфейс репозитория для сущности <see cref="Rule"/>.
    /// </summary>
    public interface IRuleRepository
    {
        /// <summary>
        /// Получение правила по уникальному идентифкатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <returns>
        /// Искомое правило, <c>null</c>, если не найдено.
        /// </returns>
        Rule Get(long id);

        Page<RuleHistory> GetHistory(GetRuleHistoryParameters parameters);
        
        /// <summary>
        /// Получение набора правил по уникальным идентификаторам.
        /// </summary>
        /// <param name="ids">
        /// Уникальные идентификаторы.
        /// </param>
        /// <returns>
        /// Коллекция правил.
        /// </returns>
        IList<Rule> GetRules(long[] ids);

        /// <summary>
        /// Получение коллекции правил по условиям <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters">
        /// Условия отбора правил.
        /// </param>
        /// <returns>
        /// Коллекция правил.
        /// </returns>
        Page<Rule> GetRules(GetRulesParameters parameters = null);

        Page<Rule> GetPromoAction(GetRulesParameters parameters = null);

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
        IList<Rule> GetActualRulesByRuleDomainName(string ruleDomainName, DateTime? dateTime = null);

        /// <summary>
        /// Сохранение правила в хранилище.
        /// </summary>
        /// <param name="entity">
        /// Сохраняемое правило.
        /// </param>
        void Save(Rule entity);

        void Save(IList<Rule> rules);

        void SaveApprove(long ruleId, bool approve, string reason);

        void SaveApproves(IList<Approve> approves);

        /// <summary>
        /// Удаление правила из хранилища по уникальному идентификатору.
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <param name="userId">
        /// Идентификатор пользователя выполняющего удаление
        /// </param>
        void DeleteById(long id, string userId);

        //void SetRulesExternalStatusIds(IDictionary<long, string> ruleIdStatusIdPairs);
    }
}