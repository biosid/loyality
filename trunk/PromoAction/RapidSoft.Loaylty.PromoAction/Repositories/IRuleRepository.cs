namespace RapidSoft.Loaylty.PromoAction.Repositories
{
    using System;
    using System.Collections.Generic;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;
    using RapidSoft.Loaylty.PromoAction.Api.InputParameters;

    /// <summary>
    /// ��������� ����������� ��� �������� <see cref="Rule"/>.
    /// </summary>
    public interface IRuleRepository
    {
        /// <summary>
        /// ��������� ������� �� ����������� �������������.
        /// </summary>
        /// <param name="id">
        /// ���������� �������������.
        /// </param>
        /// <returns>
        /// ������� �������, <c>null</c>, ���� �� �������.
        /// </returns>
        Rule Get(long id);

        Page<RuleHistory> GetHistory(GetRuleHistoryParameters parameters);
        
        /// <summary>
        /// ��������� ������ ������ �� ���������� ���������������.
        /// </summary>
        /// <param name="ids">
        /// ���������� ��������������.
        /// </param>
        /// <returns>
        /// ��������� ������.
        /// </returns>
        IList<Rule> GetRules(long[] ids);

        /// <summary>
        /// ��������� ��������� ������ �� �������� <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters">
        /// ������� ������ ������.
        /// </param>
        /// <returns>
        /// ��������� ������.
        /// </returns>
        Page<Rule> GetRules(GetRulesParameters parameters = null);

        Page<Rule> GetPromoAction(GetRulesParameters parameters = null);

        /// <summary>
        /// ��������� ��������� ���������� �� ���� ������ �� �������� ������ ������
        /// </summary>
        /// <param name="ruleDomainName">
        /// �������� ������ ������
        /// </param>
        /// <param name="dateTime">
        /// ���� � ����� ��� ������ ��������� ������. �� ��������� ������� ���� � �����.
        /// </param>
        /// <returns>
        /// ��������� ������ ������
        /// </returns>
        IList<Rule> GetActualRulesByRuleDomainName(string ruleDomainName, DateTime? dateTime = null);

        /// <summary>
        /// ���������� ������� � ���������.
        /// </summary>
        /// <param name="entity">
        /// ����������� �������.
        /// </param>
        void Save(Rule entity);

        void Save(IList<Rule> rules);

        void SaveApprove(long ruleId, bool approve, string reason);

        void SaveApproves(IList<Approve> approves);

        /// <summary>
        /// �������� ������� �� ��������� �� ����������� ��������������.
        /// </summary>
        /// <param name="id">
        /// ���������� �������������.
        /// </param>
        /// <param name="userId">
        /// ������������� ������������ ������������ ��������
        /// </param>
        void DeleteById(long id, string userId);

        //void SetRulesExternalStatusIds(IDictionary<long, string> ruleIdStatusIdPairs);
    }
}