namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

    /// <summary>
	/// Реализация проверки не вхождения в набор значений.
	/// </summary>
	internal class NotContainEquation : ContainEquationBase
    {
        private readonly ILog log = LogManager.GetLogger(typeof(NotContainEquation));

		public NotContainEquation(VariableValue[] values)
			: base(values)
		{
		}

		/// <summary>
		/// Выполняет вычисление уравнения/объединения.
		/// </summary>
		/// <returns>
		/// Результат вычисления уравнения/объединения.
		/// </returns>
		public override bool Evaluate()
		{
			log.Info("Вычисляем проверку не вхождения в набор значений");
			if (this.Values.Length != 2)
			{
				throw new InvalidPredicateFormatException("Уравнение проверки не вхождения в набор значений должно содержать 2 переменных/литерала");
			}

			var asList = this.Values[0].Value as IList;

			var list = asList == null ? new List<object> { this.Values[0].Value } : asList.Cast<object>();

			var literal = this.Values[1].Value;

			if (literal == null)
			{
				return list.All(i => i != null);
			}

			return !list.Any(literal.Equals);
		}
	}
}