namespace RapidSoft.Loaylty.PromoAction.Tests.Services
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Linq;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Moq;

	using RapidSoft.Loaylty.PromoAction.Api.DTO;
	using RapidSoft.Loaylty.PromoAction.Api.InputParameters;
	using RapidSoft.Loaylty.PromoAction.Api.OutputResults;
	using RapidSoft.Loaylty.PromoAction.Repositories;
	using RapidSoft.Loaylty.PromoAction.Service;
	using RapidSoft.Loaylty.PromoAction.Settings;
	using RapidSoft.VTB24.ArmSecurity;
	using RapidSoft.VTB24.ArmSecurity.Interfaces;

	using TargetAudience = RapidSoft.Loaylty.PromoAction.Api.Entities.TargetAudience;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class TargetAudienceServiceTests
	{
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

		[TestMethod]
		public void ShouldReturnTargetAudience()
		{
			var clientId = (-33).ToString(CultureInfo.InvariantCulture);
			var link1 = TestHelper.InsertTargetAudienceClientLink(clientId: clientId);
			var link2 = TestHelper.InsertTargetAudienceClientLink(clientId: clientId + 1);
			
			var service = new TargetAudienceService();

			var result = service.GetClientTargetAudiences(clientId);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.ClientTargetAudiences);
			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.ClientTargetAudiences.Any(x => x.Id == link1.TargetAudienceId));
			Assert.IsFalse(result.ClientTargetAudiences.Any(x => x.Id == link2.TargetAudienceId));
		}

		[TestMethod]
		public void ShouldAssignClientTargetAudience()
		{
			var ids = new List<long>();

			var ruleRepo = new RuleRepository();
			var ruleDomain1 = TestHelper.InsertTestRuleDomain();
			ids.Add(ruleDomain1.Id);

			var rule = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
			rule.SetDeserializedPredicate(TestHelper.BuildRulePredicate());
			ruleRepo.Save(rule);

			var promoaction = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
			promoaction.SetDeserializedPredicate(TestHelper.BuildPromoActionPredicate());
			ruleRepo.Save(promoaction);

			var promoactionLinkedToTargetAudience = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
			promoactionLinkedToTargetAudience.SetDeserializedPredicate(TestHelper.BuildPromoActionLinkedToTargetAudiencePredicate());
			ruleRepo.Save(promoactionLinkedToTargetAudience);

			var service = new TargetAudienceService();

			var request =
				new AssignClientTargetAudienceParameters()
				{
					UserId = TestDataStore.TestUserId
				};
			request.ClientAudienceRelations =
				new[]
					{
						new ClientTargetAudienceRelation
							{
								ClientId = TestDataStore.TestClientId,
								PromoActionId = promoaction.Id.ToString(),
							},
						new ClientTargetAudienceRelation
							{
								ClientId = TestDataStore.TestClientId,
								PromoActionId = promoactionLinkedToTargetAudience.Id.ToString(),
							},
						new ClientTargetAudienceRelation
							{
								ClientId = TestDataStore.TestClientId,
								PromoActionId = rule.Id.ToString(),
							},
                            new ClientTargetAudienceRelation
							{
								ClientId = TestDataStore.TestClientId,
								PromoActionId = promoactionLinkedToTargetAudience.Id.ToString(),
							},
					};

			var result = service.AssignClientTargetAudience(request);

			Assert.IsTrue(result.Success);
			Assert.AreEqual(4, result.ClientTargetAudienceRelations.Length);
			Assert.AreEqual((int)AssignClientTargetAudienceCodes.Success,
							result.ClientTargetAudienceRelations.Single(r => r.PromoActionId == promoaction.Id.ToString())
								  .AssignResultCode);
			Assert.AreEqual((int)AssignClientTargetAudienceCodes.Success,
							result.ClientTargetAudienceRelations.First(r => r.PromoActionId == promoactionLinkedToTargetAudience.Id.ToString())
								  .AssignResultCode);
			Assert.AreEqual((int)AssignClientTargetAudienceCodes.TargetAudienceNotFound,
							result.ClientTargetAudienceRelations.Single(r => r.PromoActionId == rule.Id.ToString())
								  .AssignResultCode);

			var repo = new TargetAudienceRepository();

			Assert.IsNotNull(repo.Get(ApiSettings.TargetAudienceLiteralPrefix + promoaction.Id));
			Assert.IsNotNull(repo.Get(ApiSettings.TargetAudienceLiteralPrefix + promoactionLinkedToTargetAudience.Id));
			Assert.IsNull(repo.Get(ApiSettings.TargetAudienceLiteralPrefix + rule.Id));

			var repo2 = new TargetAudienceClientLinkRepository();

			Assert.IsNotNull(repo2.Get(ApiSettings.TargetAudienceLiteralPrefix + promoaction.Id, TestDataStore.TestClientId));
			Assert.IsNotNull(repo2.Get(ApiSettings.TargetAudienceLiteralPrefix + promoactionLinkedToTargetAudience.Id, TestDataStore.TestClientId));
		}

		[TestMethod]
		public void ShouldAssignClientSegment()
		{
			var client1 = Guid.NewGuid().ToString();
			var client2 = Guid.NewGuid().ToString();
			var client3 = Guid.NewGuid().ToString();

			var segmentId1 = "Vip " + Guid.NewGuid();
			var segmentId2 = "Standant " + Guid.NewGuid();

			var segment1 = new Segment { Id = segmentId1, ClientIds = new[] { client1, client2 } };
			var segment2 = new Segment { Id = segmentId2, ClientIds = new[] { client3 } };

			var param = new AssignClientSegmentParameters
							{
								Segments = new[] { segment1, segment2 },
								UserId = TestDataStore.TestUserId
							};

			var service = new TargetAudienceService();

			var result = service.AssignClientSegment(param);

			Assert.IsNotNull(result);
			Assert.AreEqual(true, result.Success);

			using (var ctx = MechanicsDbContext.Get())
			{
				Assert.IsTrue(ctx.TargetAudienceClientLinks.Count(x => x.TargetAudienceId == segmentId1) == 2);
				Assert.IsTrue(ctx.TargetAudienceClientLinks.Count(x => x.TargetAudienceId == segmentId2) == 1);
			}

			var segment2A = new Segment { Id = segmentId2, ClientIds = new[] { client1, client2, client3 } };

			param = new AssignClientSegmentParameters
						{
							Segments = new[] { segment2A },
							UserId = TestDataStore.TestUserId
						};
			result = service.AssignClientSegment(param);

			Assert.IsTrue(result.Success);

            using (var ctx = MechanicsDbContext.Get())
			{
				var res = ctx.TargetAudienceClientLinks.Join(ctx.TargetAudiences, tal => tal.TargetAudienceId, ta => ta.Id,
			                                                 (tal, ta) => new
			                                                     {
			                                                         tal,
			                                                         ta
			                                                     })
			                 .Where(t => t.ta.IsSegment && t.tal.ClientId == client1).Select(t => t.tal).ToList();
                
                Assert.AreEqual(1, res.Count);
			}
		}

        [TestMethod]
        public void ShouldReturn()
        {
            var mock = new Mock<ITargetAudienceRepository>();
            mock.Setup(x => x.GetBySegment(It.IsAny<bool?>()))
                .Returns(new List<TargetAudience> { new TargetAudience { Id = "1", Name = "111", IsSegment = true } });

            var service = new TargetAudienceService(mock.Object);

            var result = service.GetTargetAudiences(null);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TargetAudiences);
            Assert.IsTrue(result.TargetAudiences.Any());
        }

		#region IServiceInfo
		[TestMethod]
		public void ShouldPing()
		{
			var service = new TargetAudienceService();

			service.Ping();
		}

		[TestMethod]
		public void ShouldReturnServiceVersion()
		{
			var service = new TargetAudienceService();

			var version = service.GetServiceVersion();

			Assert.IsNotNull(version);
		}
		#endregion
	}
}
