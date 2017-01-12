namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation
{
	using System;
	using System.Linq;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
	using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

	/// <summary>
	/// Базовая реализация вычисления уравнения/предиката (<see cref="equation"/>) поддреживающая трансформацию в SQL.
	/// </summary>
	internal abstract class EquationEvalBase : IEquationEval
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EquationEvalBase"/> class.
		/// </summary>
		/// <param name="values">
		/// Коллекция переменных уравнения/предиката.
		/// </param>
		/// <param name="factory">
		/// Фабрика конвертеров.
		/// </param>
		protected EquationEvalBase(VariableValue[] values, IEvalStrategySelector factory)
		{
			values.ThrowIfNull("values");
			this.Values = values;
			this.Factory = factory;
		}

		/// <summary>
		/// Коллекция переменных уравнения/предиката.
		/// </summary>
		protected VariableValue[] Values { get; private set; }

		/// <summary>
		/// Фабрика конвертеров.
		/// </summary>
		protected IEvalStrategySelector Factory { get; private set; }

		/// <summary>
		/// Выполняет проверку переменных, извелечение значений и вызывает шаблонный метод <see cref="DoEval"/> для выполнения вычисления.
		/// </summary>
		/// <returns>
		/// Результат вычисления уравнения/предиката.
		/// </returns>
		public bool Evaluate()
		{
			if (this.Values.Any(x => x.Status != VariableStatuses.SimplyValue))
			{
				throw new NotSupportedException("Выполнение вычисления работает только с переменнаями в статусе VariableStatuses.SimplyValue");
			}

			var objects = this.Values.Select(x => x.Value).ToArray();
			return this.DoEval(objects);
		}

		/// <summary>
		/// Вычисляет выражение и возвращает результат указывающий вычислено ли выражение или поддерживающее оптимизирующую трансформацию в SQL.
		/// </summary>
		/// <returns>
		/// Результат вычисления.
		/// </returns>
		public EvaluateResult EvaluateExt()
		{
			if (this.Values.All(x => x.Status == VariableStatuses.SimplyValue))
			{
				var objects = this.Values.Select(x => x.Value).ToArray();
				var result = this.DoEval(objects);
				return EvaluateResult.BuildEvaluated(result);
			}

			return this.DoEvalExt(this.Values);
		}

		/// <summary>
		/// Шаблонный методы выполняющий вычисления или трансформацию в T-SQL конкретного уравнения/предиката. Реализуется наследниками.
		/// </summary>
		/// <param name="values">
		/// Коллекция значений переменных уравнения/предиката.
		/// </param>
		/// <returns>
		/// Результат вычисления.
		/// </returns>
		protected abstract EvaluateResult DoEvalExt(VariableValue[] values);

		/// <summary>
		/// Шаблонный методы выполняющий вычисления конкретного уравнения/предиката. Реализуется наследниками.
		/// </summary>
		/// <param name="objects">
		/// Коллекция переменных уравнения/предиката.
		/// </param>
		/// <returns>
		/// Результат вычисления.
		/// </returns>
		protected abstract bool DoEval(object[] objects);
	}
}