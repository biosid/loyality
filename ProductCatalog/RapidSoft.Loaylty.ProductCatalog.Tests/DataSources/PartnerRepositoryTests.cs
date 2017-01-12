namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API.Entities;

    using Common;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ProductCatalog.DataSources.Repositories;

    using RapidSoft.Loaylty.ProductCatalog.Services;

    [TestClass]
    public class PartnerRepositoryTests
    {
        private string userId = TestDataStore.TestUserId;

        [TestMethod]
        [Description("Integration")]
        public void Insert()
        {
            var repo = new PartnerRepository();
            Partner p = new Partner();
            p.InsertedUserId = RandomHelper.RandomString(64);
            p.Name = RandomHelper.RandomString(256);
            p.Type = 0;
            p.Status = 0;
            p.ThrustLevel = 0;
            p.Description = RandomHelper.RandomString(256);
            p.InsertedDate = DateTime.Now;
            var settings = new Dictionary<string, string>
                               {
                                   {
                                       PartnerSettingsExtension.ReportRecipientsKey,
                                       "test@rapidsoft.ru"
                                   }
                               };
            var partner = repo.CreateOrUpdate(TestDataStore.TestUserId, p, settings: settings);
            Assert.IsNotNull(partner);

            repo.Delete(partner.Id);
        }

        [TestMethod]
        [Description("Integration")]
        public void InsertWithSettings()
        {
            var repo = new PartnerRepository();
            Partner p = new Partner();
            p.InsertedUserId = RandomHelper.RandomString(64);
            p.Name = RandomHelper.RandomString(256);
            p.Type = 0;
            p.Status = 0;
            p.ThrustLevel = 0;
            p.Description = RandomHelper.RandomString(256);
            p.InsertedDate = DateTime.Now;

            var settings = new Dictionary<string, string>
                               {
                                   { "p1", "v1" },
                                   { "p2", "v2" },
                                   {
                                       PartnerSettingsExtension.ReportRecipientsKey,
                                       "test@rapidsoft.ru"
                                   }
                               };

            var partner = repo.CreateOrUpdate(TestDataStore.TestUserId, p, settings: settings);
            Assert.IsNotNull(partner);

            var settingsFromDB = repo.GetSettings(partner.Id);

            Assert.IsTrue(settingsFromDB.Count == 3);
            Assert.IsTrue(settingsFromDB.Any(x => x.Key == "p1" && x.Value == "v1"));
            Assert.IsTrue(settingsFromDB.Any(x => x.Key == "p2" && x.Value == "v2"));
            Assert.IsTrue(settingsFromDB.Any(x => x.Key == PartnerSettingsExtension.ReportRecipientsKey && x.Value == "test@rapidsoft.ru"));

            repo.Delete(partner.Id);
        }

        [TestMethod]
        [Description("Integration")]
        public void UpdateWithSettings()
        {
            var repo = new PartnerRepository();
            Partner p = new Partner();
            p.InsertedUserId = RandomHelper.RandomString(64);
            p.Name = RandomHelper.RandomString(256);
            p.Type = 0;
            p.Status = 0;
            p.ThrustLevel = 0;
            p.Description = RandomHelper.RandomString(256);
            p.InsertedDate = DateTime.Now;

            var settings = new Dictionary<string, string>
                           {
                               { "p1", "created" }
                           };

            var partner = repo.CreateOrUpdate(TestDataStore.TestUserId, p, settings: settings);

            partner.Description = RandomHelper.RandomString(256);
            partner.UpdatedUserId = RandomHelper.RandomString(64);
            partner.UpdatedDate = DateTime.Now;

            var partnerSettings = new Dictionary<string, string>
                           {
                               { "p1", "v1" },
                               { "p2", "v2" },
                               {
                                       PartnerSettingsExtension.ReportRecipientsKey,
                                       "test@rapidsoft.ru"
                                   }
                           };

            var partnerUpdated = repo.CreateOrUpdate(TestDataStore.TestUserId, partner, settings: partnerSettings);
            Assert.IsNotNull(partnerUpdated.UpdatedDate);

            var settingsFromDB = repo.GetSettings(partner.Id);

            Assert.IsTrue(settingsFromDB.Count == 3);
            Assert.IsNotNull(settingsFromDB.GetReportRecipients(string.Empty));
            Assert.IsTrue(settingsFromDB.Any(x => x.Key == "p1" && x.Value == "v1"));
            Assert.IsTrue(settingsFromDB.Any(x => x.Key == "p2" && x.Value == "v2"));

            repo.Delete(partnerUpdated.Id);
        }

        [TestMethod]
        [Description("Integration")]
        public void Update()
        {
            var repo = new PartnerRepository();
            Partner p = new Partner();
            p.InsertedUserId = RandomHelper.RandomString(64);
            p.Name = RandomHelper.RandomString(256);
            p.Type = 0;
            p.Status = 0;
            p.ThrustLevel = 0;
            p.Description = RandomHelper.RandomString(256);
            p.InsertedDate = DateTime.Now;
            var partner = repo.CreateOrUpdate(TestDataStore.TestUserId, p, settings: null);

            partner.Description = RandomHelper.RandomString(256);
            partner.UpdatedUserId = RandomHelper.RandomString(64);
            partner.UpdatedDate = DateTime.Now;

            var partnerUpdated = repo.CreateOrUpdate(TestDataStore.TestUserId, p, settings: null);
            Assert.IsNotNull(partnerUpdated.UpdatedDate);

            repo.Delete(partnerUpdated.Id);
        }

        [TestMethod]
        [Description("Integration")]
        public void GetAllPartners()
        {
            var repo = new PartnerRepository();
            var list = repo.GetAllPartners();
            Assert.IsTrue(list.Any());
        }

        [TestMethod]
        [Description("Integration")]
        public void GetById()
        {
            var repo = new PartnerRepository();

            Partner p = new Partner();
            p.InsertedUserId = RandomHelper.RandomString(64);
            p.Name = RandomHelper.RandomString(256);
            p.Type = 0;
            p.Status = 0;
            p.ThrustLevel = 0;
            p.Description = RandomHelper.RandomString(256);
            p.InsertedDate = DateTime.Now;
            var settings = new Dictionary<string, string>
                               {
                                   {
                                       PartnerSettingsExtension.ReportRecipientsKey,
                                       "test@rapidsoft.ru"
                                   }
                               };

            var partner = repo.CreateOrUpdate(TestDataStore.TestUserId, p, settings: settings);

            var partnerById = repo.GetById(partner.Id);

            Assert.AreEqual(partner.Id, partnerById.Id);
            Assert.AreEqual("test@rapidsoft.ru", partnerById.Settings.GetReportRecipients(string.Empty));

            repo.Delete(partnerById.Id);
        }

        [TestMethod]
        [Description("Integration")]
        public void ShouldDeleteSettingsByKeys()
        {
            var repo = new PartnerRepository();
            Partner p = new Partner();
            p.InsertedUserId = RandomHelper.RandomString(64);
            p.Name = RandomHelper.RandomString(256);
            p.Type = 0;
            p.Status = 0;
            p.ThrustLevel = 0;
            p.Description = RandomHelper.RandomString(256);
            p.InsertedDate = DateTime.Now;

            var settings = new Dictionary<string, string>
                           {
                               { "p1", "v1" },
                               { "p2", "v2" }
                           };

            var partner = repo.CreateOrUpdate(TestDataStore.TestUserId, p, settings: settings);
            Assert.IsNotNull(partner);

            repo.DeleteSettings(userId, partner.Id, new[] { "p2", "p3" });

            var settingsFromDB = repo.GetSettings(partner.Id);

            Assert.IsTrue(settingsFromDB.Count == 1);
            Assert.IsTrue(settingsFromDB.Any(x => x.Key == "p1" && x.Value == "v1"));
            Assert.IsTrue(!settingsFromDB.Any(x => x.Key == "p2" && x.Value == "v2"));

            repo.Delete(partner.Id);
        }
    }
}
