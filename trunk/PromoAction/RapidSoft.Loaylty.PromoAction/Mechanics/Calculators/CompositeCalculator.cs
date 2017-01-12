namespace RapidSoft.Loaylty.PromoAction.Mechanics.Calculators
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.PromoAction.Api.Entities;
	using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
	using RapidSoft.Loaylty.PromoAction.Mechanics.GroupCalculators;

	/// <summary>
	/// Композитный калькулятор групп правил.
	/// </summary>
	public class CompositeCalculator
	{
		/// <summary>
		/// Калькулятор базовых аддитивных правил.
		/// </summary>
		private readonly IRuleGroupCalculator _baseAddition;

		/// <summary>
		/// Калькулятор базовых мульпликативных правил.
		/// </summary>
		private readonly IRuleGroupCalculator _baseMultiplication;

		/// <summary>
		/// Калькулятор не базовых аддитивных правил.
		/// </summary>
		private readonly IRuleGroupCalculator _addition;

		/// <summary>
		/// Калькулятор не базовых мульпликативных правил.
		/// </summary>
		private readonly IRuleGroupCalculator _multiplication;

		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeCalculator"/> class.
		/// </summary>
		/// <param name="baseAddition">
		/// Калькулятор базовых аддитивных правил.
		/// </param>
		/// <param name="baseMultiplication">
		/// Калькулятор базовых мульпликативных правил.
		/// </param>
		/// <param name="addition">
		/// Калькулятор не базовых аддитивных правил.
		/// </param>
		/// <param name="multiplication">
		/// Калькулятор не базовых мульпликативных правил.
		/// </param>
		public CompositeCalculator(IRuleGroupCalculator baseAddition, IRuleGroupCalculator baseMultiplication, IRuleGroupCalculator addition, IRuleGroupCalculator multiplication)
		{
			this._baseAddition = baseAddition;
			this._baseMultiplication = baseMultiplication;
			this._addition = addition;
			this._multiplication = multiplication;
		}

		/// <summary>
		/// Вычисляет группы правил.
		/// </summary>
		/// <param name="rules">
		/// Набор правил.
		/// </param>
		/// <returns>
		/// Коллекция результатов групп правил.
		/// </returns>
		public IList<RuleGroupResult> Calculate(IEnumerable<Rule> rules)
		{
			rules.ThrowIfNull("rules");

			List<RuleGroupResult> retVal;
			try
			{
				retVal = this.GetCalculators().AsParallel().Select(x => x.Calculate(rules)).ToList();
			}
			catch (AggregateException ex)
			{
				if (ex.InnerException is RuleEvaluationException)
				{
					throw ex.InnerException;
				}

				throw;
			}

			return retVal;
		}

		/// <summary>
		/// Возвращает коллекцию калькуляторов групп правил.
		/// </summary>
		/// <returns>
		/// Коллекция калькуляторов групп правил.
		/// </returns>
		private IEnumerable<IRuleGroupCalculator> GetCalculators()
		{
			yield return this._baseAddition;
			yield return this._baseMultiplication;
			yield return this._addition;
			yield return this._multiplication;
		}
	}
}