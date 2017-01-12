namespace RapidSoft.VTB24.BankConnector.Tests.StubServices
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using RapidSoft.Loaylty.PromoAction.WsClients.AdminMechanicsService;

	public class AdminMechanicsServiceStub
		: StubBase, IAdminMechanicsService
	{
		private static List<Rule> rules = new List<Rule>();

		public static void InitRules()
		{
			rules = new List<Rule>
				        {
					        new Rule
						        {
							        Id = -10,
							        Name = "Акция-заглушка 1",
							        DateTimeFrom = null,
							        DateTimeTo = null,
							        Approved = ApproveStatus.NotApproved,
							        ApproveDescription = string.Empty
						        },
					        new Rule
						        {
							        Id = -11,
							        Name = "Акция-заглушка 2",
							        DateTimeFrom = DateTime.Now.AddYears(-5),
							        DateTimeTo = DateTime.Now.AddYears(5),
							        Approved = ApproveStatus.NotApproved,
							        ApproveDescription = string.Empty
						        }
				        };
		}

		public static void CleanUpRules()
		{
			rules = new List<Rule>();
		}

	    public string Echo(string message)
	    {
	        throw new NotImplementedException();
	    }

	    public Task<string> EchoAsync(string message)
	    {
	        throw new NotImplementedException();
	    }

	    public GetMetadataByDomainIdResult GetMetadataByDomainId(long id, string userId)
		{
			throw new NotImplementedException();
		}

		public Task<GetMetadataByDomainIdResult> GetMetadataByDomainIdAsync(long id, string userId)
		{
			throw new NotImplementedException();
		}

		public RuleDomainResult SaveRuleDomain(RuleDomain ruleDomain, string userId)
		{
			throw new NotImplementedException();
		}

		public Task<RuleDomainResult> SaveRuleDomainAsync(RuleDomain ruleDomain, string userId)
		{
			throw new NotImplementedException();
		}

		public RuleDomainResult GetRuleDomain(long id, string userId)
		{
			throw new NotImplementedException();
		}

		public Task<RuleDomainResult> GetRuleDomainAsync(long id, string userId)
		{
			throw new NotImplementedException();
		}

		public RuleDomainsResult GetAllRuleDomains(string userId)
		{
			throw new NotImplementedException();
		}

		public Task<RuleDomainsResult> GetAllRuleDomainsAsync(string userId)
		{
			throw new NotImplementedException();
		}

		public ResultBase DeleteRuleDomainById(long id, string userId)
		{
			throw new NotImplementedException();
		}

		public Task<ResultBase> DeleteRuleDomainByIdAsync(long id, string userId)
		{
			throw new NotImplementedException();
		}

		public RuleResult CreateRule(Rule rule, string userId)
		{
			throw new NotImplementedException();
		}

		public Task<RuleResult> CreateRuleAsync(Rule rule, string userId)
		{
			throw new NotImplementedException();
		}

		public RuleResult UpdateRule(Rule rule, string userId)
		{
			throw new NotImplementedException();
		}

		public Task<RuleResult> UpdateRuleAsync(Rule rule, string userId)
		{
			throw new NotImplementedException();
		}

		public RuleResult GetRule(long id, string userId)
		{
			throw new NotImplementedException();
		}

		public Task<RuleResult> GetRuleAsync(long id, string userId)
		{
			throw new NotImplementedException();
		}

		public RulesResult GetRules(GetRulesParameters parameters)
		{
			var result = new RulesResult();
			result.Rules = rules.OrderBy(x => x.Id).Skip(parameters.CountSkip ?? 0).Take(parameters.CountTake ?? 0).ToArray();
			result.ResultCode = 0;
			result.Success = true;
			result.ResultDescription = this.GetStubDescription();
			result.TotalCount = result.Rules.Count();
			return result;
		}

		public Task<RulesResult> GetRulesAsync(GetRulesParameters parameters)
		{
			throw new NotImplementedException();
		}

		public RulesResult GetPromoActions(GetRulesParameters parameters)
		{
			var result = new RulesResult();
			var selected = rules.AsEnumerable();
			if (parameters.CountSkip.HasValue)
			{
				selected = selected.Skip(parameters.CountSkip.Value);
			}

			if (parameters.CountTake.HasValue)
			{
				selected = selected.Take(parameters.CountTake.Value);
			}

			result.Rules = selected.ToArray();
			result.ResultCode = 0;
			result.Success = true;
			result.ResultDescription = this.GetStubDescription();
			result.TotalCount = result.Rules.Count();
			return result;
		}

		public Task<RulesResult> GetPromoActionsAsync(GetRulesParameters parameters)
		{
			throw new NotImplementedException();
		}

		public Task<RulesResult> GetPromoActionsLinkedToTargetAudienceAsync(GetRulesParameters parameters)
		{
			throw new NotImplementedException();
		}

		public ResultBase DeleteRuleById(long id, string userId)
		{
			throw new NotImplementedException();
		}

		public Task<ResultBase> DeleteRuleByIdAsync(long id, string userId)
		{
			throw new NotImplementedException();
		}

		public ResultBase DeactivateRule(long id, string userId)
		{
			throw new NotImplementedException();
		}

		public Task<ResultBase> DeactivateRuleAsync(long id, string userId)
		{
			throw new NotImplementedException();
		}

		public ResultBase DeactivateRules(long[] ids, string userId)
		{
			throw new NotImplementedException();
		}

		public Task<ResultBase> DeactivateRulesAsync(long[] ids, string userId)
		{
			throw new NotImplementedException();
		}

		public GetRuleHistoryResult GetRuleHistory(GetRuleHistoryParameters parameters)
		{
			throw new NotImplementedException();
		}

		public Task<GetRuleHistoryResult> GetRuleHistoryAsync(GetRuleHistoryParameters parameters)
		{
			throw new NotImplementedException();
		}

		public ResultBase SetRuleApproved(Approve[] approves, string userId)
		{
			var result = new ResultBase { ResultCode = 0, ResultDescription = this.GetStubDescription(), Success = true };
			return result;
		}

		public Task<ResultBase> SetRuleApprovedAsync(Approve[] approves, string userId)
		{
			throw new NotImplementedException();
		}
	}
}
