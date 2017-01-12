namespace RapidSoft.Loaylty.IntegrationTests.PromoAction.Admins
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Loaylty.PromoAction.WsClients.AdminMechanicsService;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class RuleDomainManageServiceTests
    {
        private const string MetadataXml = @"<EntitiesMetadata xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Entities>
    <EntityName>Prodicts</EntityName>
    <DisplayName>Prodicts</DisplayName>
    <Attributes>
      <Name>Available</Name>
      <DisplayName>Available</DisplayName>
      <Id>c0b03775-dcdc-4ad3-bd35-e856279396d4</Id>
      <Type>Boolean</Type>
    </Attributes>
    <Attributes>
      <Name>IsKids</Name>
      <DisplayName>IsKids</DisplayName>
      <Id>a6529cf9-a3f9-46ae-88b1-0f5b21ad4371</Id>
      <Type>Boolean</Type>
    </Attributes>
    <Attributes>
      <Name>Vendor</Name>
      <DisplayName>Vendor</DisplayName>
      <Id>aaff6f72-e406-48b1-965c-d39e4077866a</Id>
      <Type>Text</Type>
    </Attributes>
    <Attributes>
      <Name>PriceRUR</Name>
      <DisplayName>PriceRUR</DisplayName>
      <Id>d59aea70-0b0b-4b11-b921-d377348bd1be</Id>
      <Type>Number</Type>
    </Attributes>
  </Entities>
</EntitiesMetadata>";

        private static readonly List<long> Ids = new List<long>();
        private string userId = TestHelper.UserId;

        [ClassCleanup]
        public static void ClassCleanup()
        {
            foreach (var id in Ids)
            {
                var id1 = id;
                WebClientCaller.CallService<AdminMechanicsServiceClient, ResultBase>(c => c.DeleteRuleDomainById(id1, TestHelper.VtbSystemUserId));
            }
        }

        [TestMethod]
        public void ShouldCreate()
        {
            var client = new AdminMechanicsServiceClient();

            var ruleDomain = new RuleDomain
                                 {
                                     Name = "Правило" + Guid.NewGuid(),
                                     Description = "Jgbcfkjfj",
                                     Metadata = MetadataXml,
                                     LimitFactor = 56,
                                     LimitType = LimitTypes.Percent,
                                     UpdatedUserId = "We"
                                 };

            var saved = client.SaveRuleDomain(ruleDomain, userId);

            Assert.AreNotEqual(saved.RuleDomain.Id, default(long));
            Assert.AreNotEqual(saved.RuleDomain.Id, ruleDomain.Id);

            Ids.Add(saved.RuleDomain.Id);
        }

        [TestMethod]
        public void ShouldReturnById()
        {
            var client = new AdminMechanicsServiceClient();

            var ruleDomain = new RuleDomain
                               {
                                   Name = "Правило" + Guid.NewGuid(),
                                   Metadata = MetadataXml,
                                   LimitFactor = 56,
                                   LimitType = LimitTypes.Percent,
                                   Description = "Jgbcfkjfj",
                                   UpdatedUserId = "We"
                               };

            var saved = client.SaveRuleDomain(ruleDomain, userId);

            var getted = client.GetRuleDomain(saved.RuleDomain.Id, userId);

            Assert.IsNotNull(getted);
            Assert.AreEqual(getted.RuleDomain.Name, ruleDomain.Name);
            Assert.AreEqual(getted.RuleDomain.Description, ruleDomain.Description);

            Ids.Add(saved.RuleDomain.Id);
        }

        [TestMethod]
        public void ShouldReturnAll()
        {
            var client = new AdminMechanicsServiceClient();

            var ruleDomain = new RuleDomain
                               {
                                   Name = "Правило" + Guid.NewGuid(),
                                   Metadata = MetadataXml,
                                   LimitFactor = 56,
                                   LimitType = LimitTypes.Percent,
                                   Description = "Jgbcfkjfj",
                                   UpdatedUserId = "We"
                               };

            var saved = client.SaveRuleDomain(ruleDomain, userId);

            var all = client.GetAllRuleDomains(userId);

            var getted = all.RuleDomains.FirstOrDefault(x => x.Id == saved.RuleDomain.Id);

            Assert.IsNotNull(getted);
            Assert.AreEqual(getted.Name, ruleDomain.Name);
            Assert.AreEqual(getted.Description, ruleDomain.Description);
            Assert.AreEqual(getted.LimitFactor, 56);
            Assert.AreEqual(getted.LimitType, LimitTypes.Percent);

            Ids.Add(saved.RuleDomain.Id);
        }

        [TestMethod]
        public void ShouldUpdate()
        {
            var client = new AdminMechanicsServiceClient();

            var ruleDomain = new RuleDomain
                               {
                                   Name = "Правило" + Guid.NewGuid(),
                                   Metadata = MetadataXml,
                                   LimitFactor = 56,
                                   LimitType = LimitTypes.Percent,
                                   Description = "Jgbcfkjfj",
                                   UpdatedUserId = "We"
                               };
            var saved = client.SaveRuleDomain(ruleDomain, userId);

            var updateRuleDomain = new RuleDomain
                                       {
                                           Id = saved.RuleDomain.Id,
                                           Name = "Новое название " + Guid.NewGuid(),
                                           Metadata = MetadataXml,
                                           LimitFactor = 1,
                                           LimitType = LimitTypes.Fixed,
                                           Description = "Описалово",
                                           UpdatedUserId = "I"
                                       };

            client.SaveRuleDomain(updateRuleDomain, userId);

            var updated = client.GetRuleDomain(saved.RuleDomain.Id, userId);

            Assert.IsNotNull(updated);
            Assert.AreEqual(updated.RuleDomain.Name, updateRuleDomain.Name);
            Assert.AreEqual(updated.RuleDomain.Description, updateRuleDomain.Description);
            Assert.AreEqual(updated.RuleDomain.LimitFactor, 1);
            Assert.AreEqual(updated.RuleDomain.LimitType, LimitTypes.Fixed);

            Ids.Add(saved.RuleDomain.Id);
        }

        [TestMethod]
        public void ShouldDelete()
        {
            var client = new AdminMechanicsServiceClient();

            var ruleDomain = new RuleDomain
                               {
                                   Name = "Правило" + Guid.NewGuid(),
                                   Metadata = MetadataXml,
                                   Description = "Jgbcfkjfj",
                                   LimitFactor = 1,
                                   LimitType = LimitTypes.Fixed,
                                   UpdatedUserId = "We"
                               };
            var saved = client.SaveRuleDomain(ruleDomain, userId);

            client.DeleteRuleDomainById(saved.RuleDomain.Id, TestHelper.VtbSystemUserId);

            var fromDB = client.GetRuleDomain(saved.RuleDomain.Id, userId);
            Assert.IsNotNull(fromDB);
            Assert.IsNull(fromDB.RuleDomain);

            Ids.Add(saved.RuleDomain.Id);
        }

        [TestMethod]
        public void ShouldReturnMetadata()
        {
            var client = new AdminMechanicsServiceClient();

            var ruleDomain = new RuleDomain
            {
                Name = "Правило" + Guid.NewGuid(),
                Metadata = MetadataXml,
                Description = "Jgbcfkjfj",
                LimitFactor = 1,
                LimitType = LimitTypes.Fixed,
                UpdatedUserId = "We"
            };
            var saved = client.SaveRuleDomain(ruleDomain, userId);

            var metadataResult = client.GetMetadataByDomainId(saved.RuleDomain.Id, userId);

            Assert.IsNotNull(metadataResult);
            Assert.AreEqual(true, metadataResult.Success);
            Assert.IsNotNull(metadataResult.Entities);

            Assert.IsTrue(metadataResult.Entities.Length == 1);
            Assert.IsTrue(metadataResult.Entities.Any(x => x.EntityName == "Prodicts"));
            Assert.IsTrue(metadataResult.Entities.Single().Attributes.Length == 4);

            Ids.Add(saved.RuleDomain.Id);
        }
    }
}
