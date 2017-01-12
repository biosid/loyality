namespace RapidSoft.Loaylty.PromoAction.Tests.Mocks
{
    using System.Diagnostics.CodeAnalysis;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Repositories;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    public class MockRuleDomainRepository : IRuleDomainRepository
    {
        private readonly RuleDomain _ruleDomain;

        public MockRuleDomainRepository(RuleDomain ruleDomain)
        {
            this._ruleDomain = ruleDomain;
        }

        public RuleDomain GetByName(string name)
        {
            return this._ruleDomain;
        }
    }
}