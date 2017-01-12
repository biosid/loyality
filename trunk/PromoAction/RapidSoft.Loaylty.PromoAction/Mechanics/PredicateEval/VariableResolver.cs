namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval
{
	using System.Collections.Generic;
	using System.Linq;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.Logging;
	using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
	using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
	using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

	public class VariableResolver : IVariableResolver
	{
        private readonly ILog log = LogManager.GetLogger(typeof(VariableResolver));

        private readonly IDictionary<string, string> context;

		private readonly IDictionary<string, string> aliases;

		private readonly IEnumerable<ISpecialVariableResolver> specialVariableResolvers;

		public VariableResolver(
			IDictionary<string, string> context, 
			IDictionary<string, string> aliases, 
			IEnumerable<ISpecialVariableResolver> specialVariableResolvers)
		{
			this.context = context;
			this.aliases = aliases;
			this.specialVariableResolvers = specialVariableResolvers;
		}

		public VariableResolver(IDictionary<string, string> context, IDictionary<string, string> aliases)
		{
			this.context = context;
			this.aliases = aliases;
			this.specialVariableResolvers = null;
		}

		public VariableValue ResolveVariable(value value)
		{
			value.ThrowIfNull("value");
			log.Debug("Определение значение переменных по контексту или алиасов столбцов: " + value);

			if (value.type != valueType.attr)
			{
				return this.CreateSimplyValue(value);
			}

			VariableValue retVal;
			if (this.IsSpecialValue(value, out retVal))
			{
				return retVal;
			}

			var attr = value.attr.SingleOrDefault();

			if (attr == null)
			{
				throw new InvalidPredicateFormatException("Для узла \"value\" c типом \"attr\", не задан вложенный узел \"attr\"");
			}

			return this.InternalResolveVariableByContext(attr.@object, attr.name, attr.type);
		}

		public VariableValue ResolveVariable(string objectName, string objectPropertyName, valueType valueType)
		{
			return this.InternalResolveVariableByContext(objectName, objectPropertyName, valueType);
		}

		private VariableValue InternalResolveVariableByContext(string objectName, string objectPropertyName, valueType valueType)
		{
			var contextKey = string.Concat(objectName, ".", objectPropertyName);

			if (this.context != null && this.context.ContainsKey(contextKey))
			{
				var contextValue = this.context[contextKey];

				var convertedValue = contextValue.ConvertedValue(valueType);

				return VariableValue.BuildValue(convertedValue);
			}

			var aliaseValue = this.aliases != null && this.aliases.ContainsKey(contextKey)
				? this.aliases[contextKey]
				: contextKey;

			return VariableValue.BuildSql(objectName, objectPropertyName, aliaseValue);
		}

		private bool IsSpecialValue(value value, out VariableValue resolvedValue)
		{
			if (this.specialVariableResolvers == null)
			{
				resolvedValue = null;
				return false;
			}

			var attr = value.attr.SingleOrDefault();

			if (attr == null)
			{
				throw new InvalidPredicateFormatException("Для узла \"value\" c типом \"attr\", не задан вложенный узел \"attr\"");
			}

			foreach (var specialVariableResolver in this.specialVariableResolvers)
			{
				if (specialVariableResolver.IsResolved(value, out resolvedValue))
				{
					return true;
				}
			}

			resolvedValue = null;
			return false;
		}
		
		private VariableValue CreateSimplyValue(value value)
		{
			var valueText = value.ValueText();
			var valueType = value.ValueType();

			var convertedValue = valueText.ConvertedValue(valueType);

			return VariableValue.BuildValue(convertedValue);
		}
	}
}