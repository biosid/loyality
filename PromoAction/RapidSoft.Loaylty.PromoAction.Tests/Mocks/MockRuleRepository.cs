namespace RapidSoft.Loaylty.PromoAction.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;
    using RapidSoft.Loaylty.PromoAction.Api.InputParameters;
    using RapidSoft.Loaylty.PromoAction.Repositories;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    public class MockRuleRepository : IRuleRepository
    {
        private readonly IList<Rule> retRules;

        public MockRuleRepository(IList<Rule> retRules)
        {
            this.retRules = retRules;
        }

        public Rule Get(long id)
        {
            throw new NotSupportedException();
        }

        public Page<RuleHistory> GetHistory(GetRuleHistoryParameters parameters)
        {
            throw new NotSupportedException();
        }

        public IList<Rule> GetRules(long[] ids)
        {
            throw new NotSupportedException();
        }

        public Page<Rule> GetRules(GetRulesParameters parameters = null)
        {
            throw new NotSupportedException();
        }

        public Page<Rule> GetPromoAction(GetRulesParameters parameters = null)
        {
            throw new NotSupportedException();
        }

        public IList<Rule> GetAll()
        {
            throw new NotSupportedException();
        }

        public void Save(Rule entity)
        {
            throw new NotSupportedException();
        }

        public void Save(IList<Rule> rules)
        {
            throw new NotSupportedException();
        }

        public void SaveApprove(long ruleId, bool approve, string reason)
        {
            throw new NotSupportedException();
        }

        public void SaveApproves(IList<Approve> approves)
        {
            throw new NotSupportedException();
        }

        public void DeleteById(long id, string userId)
        {
            throw new NotSupportedException();
        }

        public void SetRulesExternalStatusIds(IDictionary<long, string> ruleIdStatusIdPairs)
        {
            throw new NotSupportedException();
        }

        public IList<Rule> GetActualRulesByRuleDomainName(string ruleDomainName, DateTime? dateTime = null)
        {
            return this.retRules;
        }
    }
}