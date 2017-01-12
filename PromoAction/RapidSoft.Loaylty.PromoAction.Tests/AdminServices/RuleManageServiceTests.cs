namespace RapidSoft.Loaylty.PromoAction.Tests.AdminServices
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Moq;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.PromoAction.Api.Entities;
	using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
	using RapidSoft.Loaylty.PromoAction.Api.InputParameters;
	using RapidSoft.Loaylty.PromoAction.Api.OutputResults;
	using RapidSoft.Loaylty.PromoAction.Repositories;
	using RapidSoft.Loaylty.PromoAction.Service;
	using RapidSoft.VTB24.ArmSecurity;
	using RapidSoft.VTB24.ArmSecurity.Interfaces;

	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class RuleManageServiceTests
	{
		private readonly RuleDomain _rd = CreateRuleDomain();

		private readonly filter _filter1 = new filter
											  {
												  Item =
													  new equation
														  {
															  @operator = equationOperator.like,
															  value =
																  new[]
																	  {
																		  new value
																			  {
																				  type = valueType.attr,
																				  attr =
																					  new[]
																						  {
																							  new valueAttr
																								  {
																									  @object = "Prodicts",
																									  name = "Vendor",
																									  type =
																										  valueType.@string
																								  }
																						  }
																			  },
																		  new value
																			  {
																				  type = valueType.@string,
																				  Text = new[] { "1C" }
																			  }
																	  }
														  }
											  };

		private readonly filter _filter2 = new filter
											  {
												  Item =
													  new equation
														  {
															  @operator = equationOperator.gt,
															  value =
																  new[]
																	  {
																		  new value
																			  {
																				  type = valueType.attr,
																				  attr =
																					  new[]
																						  {
																							  new valueAttr
																								  {
																									  @object = "Prodicts",
																									  name = "PriceRUR",
																									  type =
																										  valueType.numeric
																								  }
																						  }
																			  },
																		  new value
																			  {
																				  type = valueType.numeric,
																				  Text = new[] { "1500" }
																			  }
																	  }
														  }
											  };

		[ClassInitialize]
		public static void ClassInit(TestContext context)
		{
			var mock = new Mock<IUserService>();
			var superUser = TestHelper.BuildSuperUser();
			mock.Setup(x => x.GetUserPrincipalByName(TestDataStore.TestUserId)).Returns(superUser);
			var service = mock.Object;
			ArmSecurity.UserServiceCreator = () => service;
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			ArmSecurity.UserServiceCreator = null;
		}

		[TestCleanup]
		public void MyTestCleanup()
		{
			TestHelper.DeleteTestRuleDomain(new long[] { _rd.Id });
		}

		[TestMethod]
		public void ShouldCreate()
		{
			var client = new AdminMechanicsService();

			var rule = new Rule
						   {
							   Name = "Test",
							   RuleDomainId = this._rd.Id,
							   Type = RuleTypes.BaseAddition,
							   Predicate = this._filter1.Serialize(),
							   Factor = 5.555m,
							   ConditionalFactors = null,
							   DateTimeFrom = null,
							   DateTimeTo = DateTime.Now.AddMonths(5),
							   UpdatedUserId = "We",
							   IsExclusive = true,
							   IsNotExcludedBy = true,
							   Priority = 500,
						   };

			var saved = client.SaveRule(rule, true, TestDataStore.TestUserId);

			Assert.AreNotEqual(saved.Rule.Id, default(long));
			Assert.IsNotNull(new RuleRepository().Get(saved.Rule.Id));
		}

		[TestMethod]
		public void ShouldCreateWithNullPredicate()
		{
			var client = new AdminMechanicsService();

			var rule = new Rule
			{
				Name = "Test",
				RuleDomainId = this._rd.Id,
				Type = RuleTypes.BaseAddition,
				Predicate = null,
				Factor = 5.555m,
				ConditionalFactors = null,
				DateTimeFrom = null,
				DateTimeTo = DateTime.Now.AddMonths(5),
				UpdatedUserId = "We",
				IsExclusive = true,
				IsNotExcludedBy = true,
				Priority = 500,
			};

			var saved = client.SaveRule(rule, true, TestDataStore.TestUserId);

			Assert.AreNotEqual(saved.Rule.Id, default(long));
			Assert.IsNotNull(new RuleRepository().Get(saved.Rule.Id));
		}

        [TestMethod]
        public void ShouldNotCreateWithExistsPriority()
        {
            var client = new AdminMechanicsService();

            var rule1 = new Rule
                            {
                                Name = "Test",
                                RuleDomainId = this._rd.Id,
                                Type = RuleTypes.BaseAddition,
                                Predicate = null,
                                Factor = 5.555m,
                                ConditionalFactors = null,
                                DateTimeFrom = null,
                                DateTimeTo = DateTime.Now.AddMonths(5),
                                UpdatedUserId = "We",
                                IsExclusive = true,
                                IsNotExcludedBy = true,
                                Priority = 600,
                            };

            var ruleResult1 = client.SaveRule(rule1, true, TestDataStore.TestUserId);

            Assert.AreEqual(true, ruleResult1.Success);
            Assert.AreNotEqual(ruleResult1.Rule.Id, default(long));

            var rule2 = new Rule
                            {
                                Name = "Test",
                                RuleDomainId = this._rd.Id,
                                Type = RuleTypes.BaseAddition,
                                Predicate = null,
                                Factor = 5.555m,
                                ConditionalFactors = null,
                                DateTimeFrom = null,
                                DateTimeTo = DateTime.Now.AddMonths(5),
                                UpdatedUserId = "We",
                                IsExclusive = true,
                                IsNotExcludedBy = true,
                                Priority = 600,
                            };
            var ruleResult2 = client.SaveRule(rule2, true, TestDataStore.TestUserId);

            Assert.AreEqual(false, ruleResult2.Success);
            Assert.AreEqual(ResultCodes.INVALID_PRIORITY, ruleResult2.ResultCode);
            Assert.IsNotNull(ruleResult2.ResultDescription);
        }

		[TestMethod]
		public void ShouldReturnById()
		{
			var client = new AdminMechanicsService();

			var rule = new Rule
							   {
								   Name = "Test",
								   RuleDomainId = this._rd.Id,
								   Type = RuleTypes.BaseAddition,
								   Predicate = this._filter1.Serialize(),
								   Factor = 5.555m,
								   ConditionalFactors = null,
								   DateTimeFrom = null,
								   DateTimeTo = DateTime.Now.AddMonths(5),
								   UpdatedUserId = "We",
								   IsExclusive = true,
								   IsNotExcludedBy = true,
								   Priority = 500,
							   };

			new RuleRepository().Save(rule);

			var getted = client.GetRule(rule.Id, TestDataStore.TestUserId);

			Assert.IsNotNull(getted);
			Assert.AreEqual(getted.Rule.RuleDomainId, rule.RuleDomainId);
			Assert.AreEqual(getted.Rule.Factor, rule.Factor);
			Assert.AreEqual(getted.Rule.Type, RuleTypes.BaseAddition);

			var filter = getted.Rule.Predicate.Deserialize<filter>();
			Assert.AreEqual(filter.name, this._filter1.name);

			var eq = filter.Item as equation;
			var test = this._filter1.Item as equation;
			Assert.IsNotNull(eq);
			Assert.IsNotNull(test);
			Assert.AreEqual(eq.@operator, test.@operator);
			Assert.AreEqual(eq.value.First().type, test.value.First().type);
		}

		[TestMethod]
		public void ShouldReturn()
		{
			var client = new AdminMechanicsService();

			var rule = new Rule
							   {
								   Name = "Test",
								   RuleDomainId = this._rd.Id,
								   Type = RuleTypes.BaseAddition,
								   Predicate = this._filter1.Serialize(),
								   Factor = 5.555m,
								   ConditionalFactors = null,
								   DateTimeFrom = null,
								   DateTimeTo = DateTime.Now.AddMonths(5),
								   UpdatedUserId = "We",
								   IsExclusive = true,
								   IsNotExcludedBy = true,
								   Priority = 500,
							   };

			new RuleRepository().Save(rule);

			var all = client.GetRules(
				new GetRulesParameters { RuleDomainId = this._rd.Id, UserId = TestDataStore.TestUserId });

			var getted = all.Rules.FirstOrDefault(x => x.Id == rule.Id);

			Assert.IsNotNull(getted);
			Assert.AreEqual(getted.RuleDomainId, rule.RuleDomainId);
			Assert.AreEqual(getted.Factor, rule.Factor);
			Assert.AreEqual(getted.Type, RuleTypes.BaseAddition);

			var filter = getted.Predicate.Deserialize<filter>();
			Assert.AreEqual(filter.name, this._filter1.name);

			var eq = filter.Item as equation;
			var test = this._filter1.Item as equation;
			Assert.IsNotNull(eq);
			Assert.IsNotNull(test);
			Assert.AreEqual(eq.@operator, test.@operator);
			Assert.AreEqual(eq.value.First().type, test.value.First().type);
		}

		[TestMethod]
		public void ShouldUpdate()
		{
			var client = new AdminMechanicsService();

			var rule = new Rule
						   {
							   Name = "Test",
							   RuleDomainId = this._rd.Id,
							   Type = RuleTypes.BaseAddition,
							   Predicate = this._filter1.Serialize(),
							   Factor = 5.555m,
							   ConditionalFactors = null,
							   DateTimeFrom = null,
							   DateTimeTo = DateTime.Now.AddMonths(5),
							   UpdatedUserId = "We",
							   IsExclusive = true,
							   IsNotExcludedBy = true,
							   Priority = 500,
						   };
			new RuleRepository().Save(rule);

			var updateRule = new Rule
								 {
									 Name = "Test",
									 Id = rule.Id,
									 RuleDomainId = this._rd.Id,
									 Type = RuleTypes.BaseAddition,
									 Predicate = this._filter2.Serialize(),
									 Factor = 5.566m,
									 ConditionalFactors = null,
									 DateTimeFrom = null,
									 DateTimeTo = DateTime.Now.AddMonths(2),
									 UpdatedUserId = "We",
									 IsExclusive = true,
									 IsNotExcludedBy = true,
									 Priority = 520,
								 };

			client.SaveRule(updateRule, true, TestDataStore.TestUserId);

			var getted = client.GetRule(rule.Id, TestDataStore.TestUserId);

			Assert.IsNotNull(getted);
			Assert.AreEqual(getted.Rule.RuleDomainId, updateRule.RuleDomainId);
			Assert.AreEqual(getted.Rule.Factor, updateRule.Factor);
			Assert.AreEqual(getted.Rule.Type, RuleTypes.BaseAddition);

			var filter = getted.Rule.Predicate.Deserialize<filter>();
			Assert.AreEqual(filter.name, this._filter2.name);

			var eq = filter.Item as equation;
			var test = this._filter2.Item as equation;
			Assert.IsNotNull(eq);
			Assert.IsNotNull(test);
			Assert.AreEqual(eq.@operator, test.@operator);
			Assert.AreEqual(eq.value.First().type, test.value.First().type);
		}

		[TestMethod]
		public void ShouldDeactivate()
		{
			var service = new AdminMechanicsService();

			var rule = new Rule
			{
				Name = "Test",
				RuleDomainId = this._rd.Id,
				Type = RuleTypes.BaseAddition,
				Predicate = this._filter1.Serialize(),
				Factor = 5.555m,
				ConditionalFactors = null,
				DateTimeFrom = null,
				DateTimeTo = DateTime.Now.AddMonths(5),
				UpdatedUserId = "We",
				IsExclusive = true,
				IsNotExcludedBy = true,
				Priority = 500,
			};
			new RuleRepository().Save(rule);

			var getRule = service.GetRule(rule.Id, TestDataStore.TestUserId);
			Assert.IsNotNull(getRule);
			Assert.IsNotNull(getRule.Rule);
			Assert.AreEqual(RuleStatuses.Active, getRule.Rule.Status);

			var result = service.DeactivateRule(rule.Id, TestDataStore.TestUserId);
			Assert.IsNotNull(result);
			Assert.AreEqual(true, result.Success);

			var getRule2 = service.GetRule(rule.Id, TestDataStore.TestUserId);
			Assert.IsNotNull(getRule2);
			Assert.IsNotNull(getRule2.Rule);
			Assert.AreEqual(RuleStatuses.NotActive, getRule2.Rule.Status);
		}

		[TestMethod]
		public void ShouldDelete()
		{
			var repo = new RuleRepository();
			var client = new AdminMechanicsService();

			var rule = new Rule
						   {
							   Name = "Test",
							   RuleDomainId = this._rd.Id,
							   Type = RuleTypes.BaseAddition,
							   Predicate = this._filter1.Serialize(),
							   Factor = 5.555m,
							   ConditionalFactors = null,
							   DateTimeFrom = null,
							   DateTimeTo = DateTime.Now.AddMonths(5),
							   UpdatedUserId = "We",
							   IsExclusive = true,
							   IsNotExcludedBy = true,
							   Priority = 500,
						   };
			repo.Save(rule);

			client.DeleteRuleById(rule.Id, TestDataStore.TestUserId);

			var fromDB = repo.Get(rule.Id);
			Assert.IsNull(fromDB);
		}

		private static RuleDomain CreateRuleDomain()
		{
			var repo = new RuleDomainRepository();

			var domain = new RuleDomain
							 {
								 Name = "Name " + Guid.NewGuid(), 
								 Description = "Описалово",
								 LimitFactor = 1,
								 LimitType = LimitTypes.Fixed,
								 UpdatedUserId = "I"
							 };

			repo.Save(domain);

			return domain;
		}
	}
}
