namespace RapidSoft.Loaylty.PromoAction.Tests.Entities
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.PromoAction.Api.Entities;

	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass, System.Runtime.InteropServices.GuidAttribute("377A23B1-BCE8-4A14-A04D-C0232670C3D6")]
	public class RuleTests
	{
		private const string Predicate =
			"<?xml version=\"1.0\" encoding=\"utf-16\"?><filter xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><equation operator=\"eq\"><value type=\"attr\"><attr object=\"Product\" name=\"City\" type=\"numeric\" /></value><value type=\"numeric\">5</value></equation></filter>";

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ShouldSetEmptyPredicate()
		{
#pragma warning disable 168
			var rule = new Rule { Predicate = string.Empty };
#pragma warning restore 168

			Assert.Fail("Должен быть Exception");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ShouldNotSetSpacedPredicate()
		{
#pragma warning disable 168
			var rule = new Rule { Predicate = "    " };
#pragma warning restore 168

			Assert.Fail("Должен быть Exception");
		}

		[TestMethod]
		public void ShouldReturnDeserializedPredicate()
		{
			var rule = new Rule { Predicate = Predicate };

			var desPredicate = rule.GetDeserializedPredicate();

			Assert.IsNotNull(desPredicate);
		}

		[TestMethod]
		public void ShouldNotReturnInvalidBaseRule()
		{
			var rule = new Rule { Predicate = Predicate, Type = RuleTypes.BaseAddition };

			Assert.IsTrue(rule.Validate(null).Count(x => x.ErrorMessage == "Если правило базовое, то оно должно быть не исключаемое.") == 0);
		}

		//[TestMethod]
		//public void ShouldReturnPredicat()
		//{
		//	var rule = new Rule();

		//	Assert.IsNull(rule.Predicate);
		//	Assert.IsTrue(rule.Validate(null).Any(x => x.ErrorMessage == "Предикат правила не может пустой строкой или null."));
		//}

		[TestMethod]
		public void ShouldNotReturnInvalidDateStart()
		{
			var rule = new Rule { DateTimeFrom = DateTime.Now.AddDays(-50) };

			Assert.IsTrue(rule.Validate(null).All(x => x.ErrorMessage != "Дата начала действия правила не может быть меньше текущей даты."));
		}

		[TestMethod]
		public void ShouldReturnInvalidDatesRange()
		{
			var rule = new Rule { DateTimeFrom = DateTime.Now.AddDays(-50), DateTimeTo = DateTime.Now.AddDays(-150) };

			Assert.IsTrue(rule.Validate(null).Any(x => x.ErrorMessage == "Дата начала действия правила не может быть больше даты окончания."));
		}

		[TestMethod]
		public void ShouldSerializeAndDeserialize()
		{
			var conCof = new ConditionalFactor { Factor = 5, Predicate = TestHelper.ReadPredicate("EquationEqNumeric.xml"), Priority = 5 };

			var ser = conCof.Serialize();

			var conCofDes = ser.Deserialize<ConditionalFactor>();

			Assert.AreEqual(conCof.Factor, conCofDes.Factor);
			Assert.IsNotNull(conCof.Predicate);
			Assert.AreEqual(conCof.Priority, conCofDes.Priority);
		}

		[TestMethod]
		public void ShouldReturnInvalidHasConditionalFactor()
		{
			var conCofs = new[]
							  {
								  new ConditionalFactor { Factor = 5, Predicate = null, Priority = 5 },
								  new ConditionalFactor { Factor = 6, Priority = 6 }
							  };

			var conCofsSer = conCofs.Serialize();

			var rule = new Rule { ConditionalFactors = conCofsSer };

			Assert.IsTrue(rule.Validate(null).Any(x => x.ErrorMessage == "Предикат условного коэффициента не может null."));
		}

		[TestMethod]
		[DeploymentItem("FilterXMLs\\EquationEqNumeric.xml", "FilterXMLs")]
		public void ShouldReturnInvalidConditionalCoefficientPriority()
		{
			var conCofs = new[]
							  {
								  new ConditionalFactor { Factor = 5, Predicate = TestHelper.ReadPredicate("EquationEqNumeric.xml"), Priority = 5 },
								  new ConditionalFactor { Factor = 6, Predicate = TestHelper.ReadPredicate("EquationEqNumeric.xml"), Priority = 5 }
							  };

			var conCofsSer = conCofs.Serialize();

			var rule = new Rule { ConditionalFactors = conCofsSer };

			Assert.IsTrue(rule.Validate(null).Any(x => x.ErrorMessage == "Приоритеты условных коэффициента должны уникальны в рамках правила."));
		}
	}
}
