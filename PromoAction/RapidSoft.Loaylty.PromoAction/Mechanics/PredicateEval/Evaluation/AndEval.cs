namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.Logging;
	using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;

	/// <summary>
	/// Расчетчик объединения с оператором "И".
	/// </summary>
	internal class AndEval : IEquationEval
	{
        private readonly ILog log = LogManager.GetLogger(typeof(AndEval));

        /// <summary>
		/// Фабрика конвертеров.
		/// </summary>
		private readonly IEvalStrategySelector factory;

		/// <summary>
		/// Initializes a new instance of the <see cref="AndEval"/> class.
		/// </summary>
		/// <param name="operands">
		/// Коллекция операндов объединения.
		/// </param>
		/// <param name="factory">
		/// Фабрика конвертеров.
		/// </param>
		public AndEval(IEnumerable<IEquationEval> operands, IEvalStrategySelector factory)
		{
			this.factory = factory;
			operands.ThrowIfNull("operands");
			this.Operands = operands;
		}

		/// <summary>
		/// Коллекция операндов объединения.
		/// </summary>
		public IEnumerable<IEquationEval> Operands { get; private set; }

		/// <summary>
		/// Выполняет вычисление объединения.
		/// </summary>
		/// <returns>
		/// Результат вычисления.
		/// </returns>
		public bool Evaluate()
		{
			var args2 = this.Operands.ToArray();

			if (args2.Length == 0)
			{
				throw new InvalidPredicateFormatException("Не правильный И");
			}

			var retVal = args2[0].Evaluate();

			return args2.Skip(1).Aggregate(retVal, (current, equationEval) => current && equationEval.Evaluate());
		}

		/// <summary>
		/// Вычисляет объединение "И" выражение и возвращает результат указывающий вычислено ли выражение или поддерживающее оптимизирующую трансформацию в SQL.
		/// </summary>
		/// <returns>
		/// Результат вычисления.
		/// </returns>
		public EvaluateResult EvaluateExt()
		{
			log.Debug("Формируем SQL объединения И.");
			var convertibles = new Stack<EvaluateResult>();

			// NOTE: Сначала вычисляем уравнения,так как если хоть один вернет true, дальше можно не вычислять все выражение true (false && X = false)
			foreach (var itemResult in this.Operands.Select(equationEval => equationEval.EvaluateExt()))
			{
				switch (itemResult.Code)
				{
					case EvaluateResultCode.False:
						{
							// NOTE: Возвращаем EvaluateResultCode.True, так как "false && X = false", дальше вычислять не надо.
							return EvaluateResult.BuildEvaluated(false);
						}

					case EvaluateResultCode.True:
						{
							// NOTE: Пропускаем, так как выражение вычислено.
							break;
						}

					case EvaluateResultCode.ConvertibleToSQL:
						{
							// NOTE: Запоминаем, далее надо будет получать конвертацию в sql
							convertibles.Push(itemResult);
							break;
						}

					default:
						{
							throw new NotSupportedException();
						}
				}
			}

			if (convertibles.Count == 0)
			{
				return EvaluateResult.BuildEvaluated(true);
			}

			// NOTE: Если дошли сюда, значит нужна трансформация в sql
			return EvaluateResult.BuildTansformed(this.factory.CreateUnionConverter("and", convertibles));
		}
	}
}