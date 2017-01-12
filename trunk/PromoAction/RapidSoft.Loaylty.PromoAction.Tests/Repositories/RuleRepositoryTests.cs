namespace RapidSoft.Loaylty.PromoAction.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Data.SqlTypes;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.InputParameters;
    using RapidSoft.Loaylty.PromoAction.Repositories;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class RuleRepositoryTests
    {
        private readonly ILog log = LogManager.GetLogger(typeof(RuleRepositoryTests));
        private readonly List<long> ids = new List<long>();

        #region Additional test attributes
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            // NOTE: Используется только если БД нету
            // Database.SetInitializer(new CreateDatabaseIfNotExists<MechanicsDbContext>());
            // var ctx = MechanicsDbContext.Get();
            // ctx.Database.Delete();
            // ctx.Database.CreateIfNotExists();
        }

        [TestInitialize]
        public void MyTestInitialize()
        {
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            TestHelper.DeleteTestRuleDomain(this.ids);
        }
        #endregion

        [TestMethod]
        public void Create()
        {
            var repo = new RuleRepository();
            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain1.Id);
            var rule = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule.Name = "Create Rule";
            rule.Approved = ApproveStatus.Approved;
            rule.ApproveDescription = "ApproveDescription";
            repo.Save(rule);
            var temp = repo.Get(rule.Id);
            Assert.IsNotNull(temp);
        }

        [TestMethod]
        public void ShouldCreateBaseRuleWithAutoApproved()
        {
            var repo = new RuleRepository();
            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain1.Id);
            var rule = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule.Approved = ApproveStatus.NotApproved;
            rule.ApproveDescription = "ApproveDescription";
            rule.Type = RuleTypes.BaseAddition;

            repo.Save(rule);
            var temp = repo.Get(rule.Id);
            Assert.IsNotNull(temp);
            Assert.AreEqual(ApproveStatus.Approved, temp.Approved, "Нельзя сохранить базовое правило с Approved = false");
        }

        [TestMethod]
        public void ShouldCreateNotBaseRuleAsNotApproved()
        {
            var repo = new RuleRepository();
            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain1.Id);
            
            var rule2 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule2.Approved = ApproveStatus.Approved;
            rule2.ApproveDescription = "ApproveDescription";
            rule2.Type = RuleTypes.Addition;

            repo.Save(rule2);
            var temp2 = repo.Get(rule2.Id);
            Assert.IsNotNull(temp2);
            Assert.AreEqual(ApproveStatus.NotApproved, temp2.Approved);
        }

        [TestMethod]
        public void ShouldChangeUpdatedDate()
        {
            var repo = new RuleRepository();

            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            var rule1 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);

            repo.Save(rule1);

            var rule1FromDb = repo.Get(rule1.Id);
            Assert.IsNotNull(rule1FromDb.InsertedDate);
            Assert.IsNull(rule1FromDb.UpdatedDate);

            var rule2 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule2.Id = rule1.Id;

            repo.Save(rule2);

            var rule2FromDb = repo.Get(rule1.Id);

            Assert.IsNotNull(rule2FromDb.UpdatedDate);

            this.ids.Add(ruleDomain1.Id);
        }

        [TestMethod]
        public void ShouldUpdateRuleWithApprovedReset()
        {
            var repo = new RuleRepository();
            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain1.Id);
            var rule = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule.Approved = ApproveStatus.NotApproved;
            rule.ApproveDescription = "ApproveDescription";
            rule.Type = RuleTypes.BaseAddition;

            repo.Save(rule);
            var temp = repo.Get(rule.Id);
            Assert.IsNotNull(temp);
            Assert.AreEqual(ApproveStatus.Approved, temp.Approved);

            temp.Type = RuleTypes.Addition;
            repo.Save(temp);
            var temp2 = repo.Get(rule.Id);
            Assert.IsNotNull(temp2);
            Assert.AreEqual(ApproveStatus.NotApproved, temp2.Approved);
        }

        [TestMethod]
        public void ShouldUpdateFactorWithApprovedReset()
        {
            var repo = new RuleRepository();
            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain1.Id);
            var rule = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule.Approved = ApproveStatus.NotApproved;
            rule.ApproveDescription = "ApproveDescription";
            rule.Type = RuleTypes.Addition;
            repo.Save(rule);
            repo.SaveApprove(rule.Id, true, "Test");

            var temp = repo.Get(rule.Id);
            Assert.IsNotNull(temp);
            Assert.AreEqual(ApproveStatus.Approved, temp.Approved);

            // temp.Type = RuleTypes.Addition;
            temp.Factor = temp.Factor + 10;
            repo.Save(temp);
            var temp2 = repo.Get(rule.Id);
            Assert.IsNotNull(temp2);
            Assert.AreEqual(ApproveStatus.NotApproved, temp2.Approved);
        }

        [TestMethod]
        public void ShouldReturnRulesByDomainName()
        {
            var repo = new RuleRepository();

            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            var ruleDomain2 = TestHelper.InsertTestRuleDomain();
            var rule1 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);

            var rule2 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain2.Id);

            repo.Save(rule1);
            repo.Save(rule2);

            var rules = repo.GetRulesByRuleDomainName(ruleDomain1.Name);

            Assert.IsTrue(rules.Any(r => r.Id == rule1.Id));
            Assert.IsFalse(rules.Any(r => r.Id == rule2.Id));

            this.ids.Add(ruleDomain1.Id);
            this.ids.Add(ruleDomain2.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void ShouldNotSaveRule()
        {
            var repo = new RuleRepository();

            var ruleDomain = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain.Id);
            var rule = TestHelper.BuildTestRule(ruleDomainId: ruleDomain.Id);
            rule.DateTimeFrom = DateTime.Now.AddDays(10);
            rule.DateTimeTo = DateTime.Now.AddDays(-10);

            repo.Save(rule);

            Assert.Fail("Должен быть DbEntityValidationException");
        }

        [TestMethod]
        public void ShouldNotModify()
        {
            var repo = new RuleRepository();

            var ruleDomain = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain.Id);
            var rule = TestHelper.BuildTestRule(ruleDomainId: ruleDomain.Id);
            var date1 = DateTime.Now.AddDays(10);
            rule.DateTimeFrom = date1;
            rule.DateTimeTo = DateTime.Now.AddDays(20);

            repo.Save(rule);

            var ruleFrom = repo.Get(rule.Id);

            ruleFrom.DateTimeFrom = DateTime.Now.AddDays(30);
            Exception exception = null;
            try
            {
                repo.Save(rule);
            }
            catch (Exception ex)
            {
                // NOTE: Игнорим.
                exception = ex;
            }

            var ruleFrom2 = repo.Get(rule.Id);

            Assert.IsNotNull(exception);
            Assert.IsNotNull(ruleFrom2.DateTimeFrom);

            log.Debug("Сохранили в БД " + date1.ToString("dd/MM/yyyy hh:mm:ss.fff") + ", а получили " + ruleFrom2.DateTimeFrom.Value.ToString("dd/MM/yyyy hh:mm:ss.fff"));
            Assert.AreEqual(ruleFrom2.DateTimeFrom.Value.ToString("dd/MM/yyyy hh:mm:ss"), date1.ToString("dd/MM/yyyy hh:mm:ss"));
        }

        [TestMethod]
        public void ShouldReturnRules()
        {
            var repo = new RuleRepository();

            var ruleDomain = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain.Id);
            var rule = TestHelper.BuildTestRule(ruleDomainId: ruleDomain.Id);
            repo.Save(rule);

            var all = repo.GetRules();

            Assert.IsNotNull(all);
            Assert.IsNull(all.TotalCount);
            Assert.IsTrue(all.Any(x => x.Id == rule.Id));

            var withTotalCount = repo.GetRules(new GetRulesParameters { CalcTotalCount = true });
            Assert.IsNotNull(withTotalCount);
            Assert.IsNotNull(withTotalCount.TotalCount, "Должен вычислен кол-во записей");
            Assert.IsTrue(all.Any(x => x.Id == rule.Id));

            var now = DateTime.Now;

            var param = new GetRulesParameters
                            {
                                DateTimeFrom = now.AddDays(-5),
                                DateTimeTo = now.AddDays(5),
                                RuleDomainId = ruleDomain.Id,
                                SearchTerm = "tt",
                                SortDirect = SortDirections.Asc,
                                Status = RuleStatuses.NotActive,
                                Type = RuleTypes.BaseAddition,
                                CalcTotalCount = true,
                                CountSkip = 1,
                                CountTake = 5
                            };

            var filtered = repo.GetRules(param);
            Assert.IsNotNull(filtered);
        }

        [TestMethod]
        public void ShouldReturnRuleFromDate()
        {
            var repo = new RuleRepository();

            var ruleDomain = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain.Id);
            var rule1 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain.Id);
            rule1.DateTimeFrom = null;
            rule1.DateTimeTo = new DateTime(2013, 06, 01);

            var rule2 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain.Id);
            rule2.DateTimeFrom = new DateTime(2013, 05, 31, 23, 59, 59);
            rule2.DateTimeTo = null;

            repo.Save(rule1);
            repo.Save(rule2);

            var param = new GetRulesParameters
                             {
                                 RuleDomainId = ruleDomain.Id,
                                 DateTimeFrom = new DateTime(2013, 05, 01),
                                 DateTimeTo = new DateTime(2013, 06, 30)
                             };

            var page = repo.GetRules(param);

            Assert.IsNotNull(page);
            Assert.AreEqual(2, page.Count);
            Assert.IsTrue(page.Any(x => x.Id == rule1.Id));
            Assert.IsTrue(page.Any(x => x.Id == rule2.Id));
        }

        [TestMethod]
        public void ShouldReturnRuleHistory()
        {
            var repo = new RuleRepository();

            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain1.Id);
            var rule1 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);

            repo.Save(rule1);

            rule1.Name = "Other Name " + Guid.NewGuid();

            repo.Save(rule1);

            repo.DeleteById(rule1.Id, "UserId");

            var param = new GetRuleHistoryParameters { RuleId = rule1.Id, CalcTotalCount = true };
            var history = repo.GetHistory(param);

            Assert.IsNotNull(history);
            Assert.IsTrue(history.Count == 3);
            Assert.AreEqual(3, history.TotalCount);
        }

        [TestMethod]
        public void ShouldReturnRuleWithDateFromAndWithoutDateTo()
        {
            var repo = new RuleRepository();

            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain1.Id);
            var rule1 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);

            rule1.DateTimeFrom = new DateTime(2013, 05, 15);
            rule1.DateTimeTo = null;
            repo.Save(rule1);

            var ruleId = rule1.Id;
            Assert.AreNotSame(0, ruleId);

            var param = new GetRulesParameters { DateTimeFrom = new DateTime(2013, 05, 16) };

            var rules = repo.GetRules(param);

            Assert.IsNotNull(rules);

            Assert.IsTrue(rules.Any(x => x.Id == ruleId));
        }

        [TestMethod]
        public void ShouldReturnRulesByDates()
        {
            var repo = new RuleRepository();
            var dateFrom = new DateTime(2013, 05, 05);
            var dateTo = new DateTime(2013, 05, 10);
            var dateUntil = new DateTime(2013, 05, 11);

            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain1.Id);

            // Вечное
            var rule1 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule1.DateTimeFrom = null;
            rule1.DateTimeTo = null;
            repo.Save(rule1);

            // прошлое и закончилось до dateFrom
            var rule2 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule2.DateTimeFrom = null;
            rule2.DateTimeTo = new DateTime(2013, 05, 2);
            repo.Save(rule2);

            // прошлое и действует на dateFrom, но закончится до dateTo
            var rule3 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule3.DateTimeFrom = null;
            rule3.DateTimeTo = new DateTime(2013, 05, 7);
            repo.Save(rule3);

            // прошлое и действует на dateFrom и закончится после dateTo
            var rule4 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule4.DateTimeFrom = null;
            rule4.DateTimeTo = new DateTime(2013, 05, 13);
            repo.Save(rule4);

            // началось и закончилось до dateFrom
            var rule5 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule5.DateTimeFrom = new DateTime(2013, 05, 1);
            rule5.DateTimeTo = new DateTime(2013, 05, 2);
            repo.Save(rule5);

            // началось до dateFrom, закончился после dateFrom, но до dateTo
            var rule6 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule6.DateTimeFrom = new DateTime(2013, 05, 1);
            rule6.DateTimeTo = new DateTime(2013, 05, 7);
            repo.Save(rule6);

            // началось до dateFrom, закончился после dateTo
            var rule7 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule7.DateTimeFrom = new DateTime(2013, 05, 1);
            rule7.DateTimeTo = new DateTime(2013, 05, 13);
            repo.Save(rule7);

            // началось до dateFrom, закончился никогда
            var rule8 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule8.DateTimeFrom = new DateTime(2013, 05, 1);
            rule8.DateTimeTo = null;
            repo.Save(rule8);

            // началось после dateFrom и закончился до dateTo
            var rule9 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule9.DateTimeFrom = new DateTime(2013, 05, 7);
            rule9.DateTimeTo = new DateTime(2013, 05, 8);
            repo.Save(rule9);

            // началось после dateFrom и закончился после dateTo
            var rule10 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule10.DateTimeFrom = new DateTime(2013, 05, 7);
            rule10.DateTimeTo = new DateTime(2013, 05, 13);
            repo.Save(rule10);

            // началось после dateFrom и закончился никогда
            var rule11 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule11.DateTimeFrom = new DateTime(2013, 05, 7);
            rule11.DateTimeTo = null;
            repo.Save(rule11);

            // началось после dateTo и закончился dateTo
            var rule12 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule12.DateTimeFrom = new DateTime(2013, 05, 13);
            rule12.DateTimeTo = new DateTime(2013, 05, 14);
            repo.Save(rule12);

            // началось после dateTo и закончился никогда
            var rule13 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule13.DateTimeFrom = new DateTime(2013, 05, 13);
            rule13.DateTimeTo = null;
            repo.Save(rule13);
            
            // Все
            var paramForever = new GetRulesParameters
                                   {
                                       RuleDomainId = ruleDomain1.Id,
                                       DateTimeFrom = null,
                                       DateTimeTo = null
                                   };
            var rules = repo.GetRules(paramForever);

            Assert.IsNotNull(rules);
            Assert.AreEqual(13, rules.Count, "Должны получить все правила домена");

            // С даты dateFrom
            var paramFrom = new GetRulesParameters
                                {
                                    RuleDomainId = ruleDomain1.Id,
                                    DateTimeFrom = dateFrom,
                                    DateTimeTo = null
                                };
            rules = repo.GetRules(paramFrom);
            Assert.IsNotNull(rules);

            var notAssertRulesIdsFrom = new[] { rule2.Id, rule5.Id };

            var arrFrom = rules.Where(x => !notAssertRulesIdsFrom.Contains(x.Id)).ToList();
            Assert.AreEqual(11, arrFrom.Count, "Получен известный список правил, кроме правила 2 и 5, они завершились до dateFrom");
            Assert.AreEqual(11, rules.Count, "Лишних быть не должно");

            // По дату dateTo
            var paramTo = new GetRulesParameters
                              {
                                  RuleDomainId = ruleDomain1.Id,
                                  DateTimeFrom = null,
                                  DateTimeTo = dateTo
                              };
            rules = repo.GetRules(paramTo);
            Assert.IsNotNull(rules);

            var notAssertRulesIdsTo = new[] { rule12.Id, rule13.Id };

            var arrTo = rules.Where(x => !notAssertRulesIdsTo.Contains(x.Id)).ToList();
            Assert.AreEqual(11, arrTo.Count, "Получен известный список правил, кроме правила 12 и 13, они начались после dateTo");
            Assert.AreEqual(11, rules.Count, "Лишних быть не должно");

            // С даты dateFrom по дату dateTo
            var paramFromTo = new GetRulesParameters
            {
                RuleDomainId = ruleDomain1.Id,
                DateTimeFrom = dateFrom,
                DateTimeTo = dateTo
            };
            rules = repo.GetRules(paramFromTo);
            Assert.IsNotNull(rules);

            var notAssertRulesIdsFromTo = notAssertRulesIdsFrom.Concat(notAssertRulesIdsTo).ToArray();

            var arrFromTo = rules.Where(x => !notAssertRulesIdsFromTo.Contains(x.Id)).ToList();
            Assert.AreEqual(
                9,
                arrFromTo.Count,
                "Получен известный список правил, кроме правила 2 и 5, они завершились до dateFrom и кроме правила 12 и 13, они начались после dateTo");
            Assert.AreEqual(9, rules.Count, "Лишних быть не должно");

            // Завершившиеся до даты
            var paramUntil = new GetRulesParameters { RuleDomainId = ruleDomain1.Id, DateTimeUntil = dateUntil };
            rules = repo.GetRules(paramUntil);
            Assert.IsNotNull(rules);

            var notAssertRulesIdsUntil = new[]
                                             {
                                                 rule1.Id, rule4.Id, rule7.Id, rule8.Id, rule10.Id, rule11.Id, rule12.Id,
                                                 rule13.Id
                                             };

            var arrUntil = rules.Where(x => !notAssertRulesIdsUntil.Contains(x.Id)).ToList();
            Assert.AreEqual(
                5,
                arrUntil.Count,
                "Получен известный список правил, кроме правила 1, 8, 11, 13 - они бессрочные; 4, 7, 10, 12 - они завершатся после dateUntil");
            Assert.AreEqual(5, rules.Count, "Лишних быть не должно");
        }

        [TestMethod]
        public void ShouldReturnSorted()
        {
            var repo = new RuleRepository();
            var maxDateTime = (DateTime)SqlDateTime.MaxValue;
            var minDateTime = (DateTime)SqlDateTime.MinValue;

            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain1.Id);

            var rule1 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule1.Priority = int.MinValue;
            rule1.DateTimeTo = maxDateTime;
            repo.Save(rule1);

            var rule2 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule2.Priority = int.MaxValue;
            rule2.DateTimeTo = minDateTime;
            repo.Save(rule2);

            var parameters = new GetRulesParameters
                             {
                                 RuleDomainId = ruleDomain1.Id,
                                 SortDirect = SortDirections.Asc,
                                 SortProperty = SortProperty.Priority
                             };

            var rules = repo.GetRules(parameters);

            Assert.AreEqual(2, rules.Count);
            Assert.IsTrue(rules[0].Priority == int.MinValue && rules[0].DateTimeTo == maxDateTime);
            Assert.IsTrue(rules[1].Priority == int.MaxValue && rules[1].DateTimeTo == minDateTime);

            parameters = new GetRulesParameters
                             {
                                 RuleDomainId = ruleDomain1.Id,
                                 SortDirect = SortDirections.Asc,
                                 SortProperty = SortProperty.DateTimeTo
                             };

            rules = repo.GetRules(parameters);

            Assert.AreEqual(2, rules.Count);
            Assert.IsTrue(rules[0].DateTimeTo == minDateTime && rules[0].Priority == int.MaxValue);
            Assert.IsTrue(rules[1].DateTimeTo == maxDateTime && rules[1].Priority == int.MinValue);

            parameters = new GetRulesParameters
                             {
                                 RuleDomainId = ruleDomain1.Id,
                                 SortDirect = SortDirections.Desc,
                                 SortProperty = SortProperty.Priority
                             };

            rules = repo.GetRules(parameters);

            Assert.AreEqual(2, rules.Count);
            Assert.IsTrue(rules[0].Priority == int.MaxValue && rules[0].DateTimeTo == minDateTime);
            Assert.IsTrue(rules[1].Priority == int.MinValue && rules[1].DateTimeTo == maxDateTime);

            parameters = new GetRulesParameters
                             {
                                 RuleDomainId = ruleDomain1.Id,
                                 SortDirect = SortDirections.Desc,
                                 SortProperty = SortProperty.DateTimeTo
                             };

            rules = repo.GetRules(parameters);

            Assert.AreEqual(2, rules.Count);
            Assert.IsTrue(rules[0].DateTimeTo == maxDateTime && rules[0].Priority == int.MinValue);
            Assert.IsTrue(rules[1].DateTimeTo == minDateTime && rules[1].Priority == int.MaxValue);
        }

        [TestMethod]
        public void ShouldReturnPromoActions()
        {
            var repo = new RuleRepository();
            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain1.Id);

            var rule = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
            rule.SetDeserializedPredicate(TestHelper.BuildRulePredicate());
            repo.Save(rule);

            // NOTE: ApproveStatus.NotApproved
            var promoaction1 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id, type: RuleTypes.Addition);
            promoaction1.SetDeserializedPredicate(TestHelper.BuildPromoActionPredicate());
            repo.Save(promoaction1);

            // NOTE: ApproveStatus.Approved
            var promoaction2 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id, type: RuleTypes.Multiplication);
            promoaction2.SetDeserializedPredicate(TestHelper.BuildPromoActionPredicate());
            repo.Save(promoaction2);
            repo.SaveApprove(promoaction2.Id, true, "Test");

            // NOTE: ApproveStatus.Correction
            var promoaction3 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id, type: RuleTypes.Multiplication);
            promoaction3.SetDeserializedPredicate(TestHelper.BuildPromoActionPredicate());
            repo.Save(promoaction3);
            repo.SaveApprove(promoaction3.Id, false, "Test");

            var parameters = new GetRulesParameters { RuleDomainId = ruleDomain1.Id };

            var rules = repo.GetRules(parameters);
            var promoactions = repo.GetPromoAction(parameters);
            Assert.AreEqual(4, rules.Count());
            Assert.AreEqual(3, promoactions.Count());

            var parametersNotApproved = new GetRulesParameters
                                            {
                                                RuleDomainId = ruleDomain1.Id,
                                                CalcTotalCount = true,
                                                ApproveStatuses = new[] { ApproveStatus.NotApproved, }
                                            };

            var promoactionsNotApproved = repo.GetPromoAction(parametersNotApproved);
            Assert.AreEqual(1, promoactionsNotApproved.Count());

            var parametersAllApprove = new GetRulesParameters
                                           {
                                               RuleDomainId = ruleDomain1.Id,
                                               CalcTotalCount = true,
                                               ApproveStatuses =
                                                   new[]
                                                       {
                                                           ApproveStatus.NotApproved,
                                                           ApproveStatus.Correction,
                                                           ApproveStatus.Approved
                                                       }
                                           };
            var promoactionsAllApprove = repo.GetPromoAction(parametersAllApprove);
            Assert.AreEqual(3, promoactionsAllApprove.Count());
        }

        [TestMethod]
        public void ShouldSaveApproves()
        {
            var repo = new RuleRepository();
            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain1.Id);
            
            var rule1 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id, type: RuleTypes.Addition);
            rule1.Approved = ApproveStatus.NotApproved;
            rule1.ApproveDescription = null;

            var rule2 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id, type: RuleTypes.Multiplication);
            rule2.Approved = ApproveStatus.NotApproved;
            rule2.ApproveDescription = null;

            repo.Save(rule1);
            repo.Save(rule2);

            var app1 = new Approve { RuleId = rule1.Id, IsApproved = true, Reason = "ОК" };
            var app2 = new Approve { RuleId = rule2.Id, IsApproved = false, Reason = "Не корректное" };

            repo.SaveApproves(new List<Approve> { app1, app2 });

            var fromDb1 = repo.Get(rule1.Id);

            Assert.IsNotNull(fromDb1);
            Assert.AreEqual(ApproveStatus.Approved, fromDb1.Approved);
            Assert.AreEqual("ОК", fromDb1.ApproveDescription);

            var fromDb2 = repo.Get(rule2.Id);

            Assert.IsNotNull(fromDb2);
            Assert.AreEqual(ApproveStatus.Correction, fromDb2.Approved);
            Assert.AreEqual("Не корректное", fromDb2.ApproveDescription);
        }

        [TestMethod]
        public void ShouldResetApprove()
        {
            var repo = new RuleRepository();
            var ruleDomain1 = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain1.Id);

            var rule = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id, type: RuleTypes.Addition);
            repo.Save(rule);

            var app1 = new Approve { RuleId = rule.Id, IsApproved = true, Reason = "Jr" };
            repo.SaveApproves(new List<Approve> { app1 });

            rule = repo.Get(rule.Id);

            Assert.AreEqual(ApproveStatus.Approved, rule.Approved);

            var updateRule = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id, type: RuleTypes.Addition);
            updateRule.Id = rule.Id;
            updateRule.DateTimeFrom = DateTime.Now.AddYears(-1);
            repo.Save(updateRule);

            var fromDb = repo.Get(rule.Id);

            Assert.IsNotNull(fromDb);
            Assert.AreEqual(ApproveStatus.NotApproved, fromDb.Approved);
            Assert.IsNull(fromDb.ApproveDescription);
        }

        //[TestMethod]
        //public void ShouldSetRulesExternalStatusIds()
        //{
        //    var repo = new RuleRepository();
        //    var ruleDomain1 = TestHelper.InsertTestRuleDomain();
        //    this.ids.Add(ruleDomain1.Id);

        //    var rule1 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
        //    repo.Save(rule1);

        //    var rule2 = TestHelper.BuildTestRule(ruleDomainId: ruleDomain1.Id);
        //    repo.Save(rule2);

        //    var rule1FromDbBeforeSet = repo.Get(rule1.Id);
        //    var rule2FromDbBeforeSet = repo.Get(rule2.Id);

        //    Assert.IsNotNull(rule1FromDbBeforeSet);
        //    Assert.IsNotNull(rule2FromDbBeforeSet);
        //    Assert.IsNull(rule1FromDbBeforeSet.ExternalStatusId);
        //    Assert.IsNull(rule2FromDbBeforeSet.ExternalStatusId);

        //    var pairs = new Dictionary<long, string> { { rule2.Id, "Status2" }, { rule1.Id, "Status1" } };

        //    repo.SetRulesExternalStatusIds(pairs);

        //    var rule1FromDbAfterSet = repo.Get(rule1.Id);
        //    var rule2FromDbAfterSet = repo.Get(rule2.Id);

        //    Assert.IsNotNull(rule1FromDbBeforeSet);
        //    Assert.IsNotNull(rule2FromDbBeforeSet);
        //    Assert.AreEqual("Status1", rule1FromDbAfterSet.ExternalStatusId);
        //    Assert.AreEqual("Status2", rule2FromDbAfterSet.ExternalStatusId);
        //}
    }
}
