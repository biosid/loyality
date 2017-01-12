namespace RapidSoft.Loaylty.PromoAction.Tests.PredicateEval
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class VariableResolveExtensionsTests
	{
		[TestMethod]
		public void ShouldResolveConstantValue()
		{
			var resolver = new VariableResolver(null, null);
			var val = new value { type = valueType.numeric, Text = new[] { "5.5678" } };
			var resolvedValue = resolver.ResolveVariable(val);

			Assert.AreEqual(resolvedValue.ObjectName, null);
			Assert.AreEqual(resolvedValue.ObjectPropertyName, null);
			Assert.AreEqual(resolvedValue.Status, VariableStatuses.SimplyValue);
			Assert.AreEqual(resolvedValue.Value, decimal.Parse("5.5678", CultureInfo.InvariantCulture));

			val.type = valueType.@string;
			val.Text = new[] { "Busya" };
			resolvedValue = resolver.ResolveVariable(val);

			Assert.AreEqual(resolvedValue.ObjectName, null);
			Assert.AreEqual(resolvedValue.ObjectPropertyName, null);
			Assert.AreEqual(resolvedValue.Status, VariableStatuses.SimplyValue);
			Assert.AreEqual(resolvedValue.Value, "Busya");

			val.type = valueType.boolean;
			val.Text = new[] { "true" };
			resolvedValue = resolver.ResolveVariable(val);

			Assert.AreEqual(resolvedValue.ObjectName, null);
			Assert.AreEqual(resolvedValue.ObjectPropertyName, null);
			Assert.AreEqual(resolvedValue.Status, VariableStatuses.SimplyValue);
			Assert.AreEqual(resolvedValue.Value, true);

			var dt = DateTime.Now;
			val.type = valueType.datetime;
			val.Text = new[] { dt.ToString(CultureInfo.InvariantCulture) };
			resolvedValue = resolver.ResolveVariable(val);

			Assert.AreEqual(resolvedValue.ObjectName, null);
			Assert.AreEqual(resolvedValue.ObjectPropertyName, null);
			Assert.AreEqual(resolvedValue.Status, VariableStatuses.SimplyValue);
// ReSharper disable SpecifyACultureInStringConversionExplicitly
			Assert.AreEqual(resolvedValue.Value.ToString(), dt.ToString());
// ReSharper restore SpecifyACultureInStringConversionExplicitly
		}

		[TestMethod]
		public void ShouldResolveAttrAsArray()
		{
			const string ContextValue = "5.5;6.6;7.7";
			var context = new Dictionary<string, string> { { "Product.City", ContextValue } };

			var resolver = new VariableResolver(context, null);

			var val = new value
				          {
					          type = valueType.attr,
					          attr = new[] { new valueAttr { @object = "Product", name = "City", type = valueType.numeric } }
				          };

			var valueFromContext = resolver.ResolveVariable(val);

			Assert.AreEqual(valueFromContext.ObjectName, null);
			Assert.AreEqual(valueFromContext.ObjectPropertyName, null);
			Assert.AreEqual(valueFromContext.Status, VariableStatuses.SimplyValue);
			var list = valueFromContext.Value as IList;
			Assert.IsNotNull(list);
			Assert.IsTrue(list.Count == 3);
		}

		[TestMethod]
		public void ShouldResolveNumericAttrValue()
		{
			const string ContextValue = "5.5";
			var context = new Dictionary<string, string> { { "Product.City", ContextValue } };

			var resolver = new VariableResolver(context, null);

			var val = new value
				            {
					            type = valueType.attr,
					            attr =
						            new[] { new valueAttr { @object = "Product", name = "City", type = valueType.numeric } }
				            };

			var valueFromContext = resolver.ResolveVariable(val);

			Assert.AreEqual(valueFromContext.ObjectName, null);
			Assert.AreEqual(valueFromContext.ObjectPropertyName, null);
			Assert.AreEqual(valueFromContext.Status, VariableStatuses.SimplyValue);
			Assert.AreEqual(valueFromContext.Value, decimal.Parse(ContextValue, CultureInfo.InvariantCulture));
		}

		[TestMethod]
		public void ShouldResolveAttrValueByAliases()
		{
			const string AliasValue = "алиас-столбца";
			var aliases = new Dictionary<string, string> { { "Product.City", AliasValue } };
			var resolver = new VariableResolver(null, aliases);

			var val = new value
				          {
					          type = valueType.attr,
					          attr = new[] { new valueAttr { @object = "Product", name = "City", type = valueType.numeric } }
				          };

			var valueFromContext = resolver.ResolveVariable(val);

			Assert.AreEqual(valueFromContext.ObjectName, "Product");
			Assert.AreEqual(valueFromContext.ObjectPropertyName, "City");
			Assert.AreEqual(valueFromContext.Status, VariableStatuses.SqlColumn);
			Assert.AreEqual(valueFromContext.Value, "алиас-столбца");
		}
	}
}
