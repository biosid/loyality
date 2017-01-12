namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation
{
    using System;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

    /// <summary>
	/// Реализация проверки операнд1 меньше операнд2.
	/// </summary>
	internal class LtEquation : EquationEvalBase
	{
        private readonly ILog log = LogManager.GetLogger(typeof(LtEquation));

		/// <summary>
		/// Initializes a new instance of the <see cref="LtEquation"/> class.
		/// </summary>
		/// <param name="values">
		/// Коллекция переменных уравнения/предиката.
		/// </param>
		/// <param name="factory">
		/// Фабрика конвертеров.
		/// </param>
		public LtEquation(VariableValue[] values, IEvalStrategySelector factory)
			: base(values, factory)
		{
		}

		/// <summary>
		/// Выполняет проверку операнд1 меньше операнд2.
		/// Кол-во входный значений должно быть равно 2.
		/// </summary>
		/// <param name="objects">
		/// Коллекция переменных уравнения/предиката.
		/// </param>
		/// <returns>
		/// Результат вычисления.
		/// </returns>
		protected override bool DoEval(object[] objects)
		{
			if (objects.Length != 2)
			{
				throw new InvalidPredicateFormatException("Уравнение проверки \"меньше\" должно содержать 2 переменных/литерала");
			}

			if (objects[0] == null || objects[1] == null)
			{
				return false;
			}

			var obj = objects[0] as IComparable;

			if (obj == null)
			{
				throw new NotSupportedException();
			}

			return obj.CompareTo(objects[1]) == -1;
		}

		/// <summary>
		/// Выполняет проверку операнд1 меньше операнд2 или трансформацию в SQL проверки входных значений.
		/// Кол-во входный значений должно быть равно 2.
		/// </summary>
		/// <param name="values">
		/// Коллекция значений переменных уравнения/предиката.
		/// </param>
		/// <returns>
		/// Результат вычисления.
		/// </returns>
		protected override EvaluateResult DoEvalExt(VariableValue[] values)
		{
			log.Debug("Формируем SQL проверки операнд1 больше операнд2.");
			if (values.Length != 2)
			{
				throw new InvalidPredicateFormatException("Уравнение проверки операнд1 больше операнд2 должно содержать 2 переменных/литерала");
			}

			return EvaluateResult.BuildTansformed(this.Factory.CreateEquationConverter(equationOperator.lt, values));
		}
	}
}