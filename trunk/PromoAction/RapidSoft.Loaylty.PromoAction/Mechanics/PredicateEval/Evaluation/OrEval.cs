namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;

    /// <summary>
	/// Расчетчик объединения с оператором "ИЛИ".
	/// </summary>
	internal class OrEval : IEquationEval
	{
        private readonly ILog log = LogManager.GetLogger(typeof(OrEval));

        /// <summary>
		/// Фабрика конвертеров.
		/// </summary>
		private readonly IEvalStrategySelector factory;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrEval"/> class.
		/// </summary>
		/// <param name="operands">
		/// Коллекция операндов объединения.
		/// </param>
		/// <param name="factory">
		/// Фабрика конвертеров.
		/// </param>
		public OrEval(IEnumerable<IEquationEval> operands, IEvalStrategySelector factory)
		{
			this.factory = factory;
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
			log.Debug("Вычисляем объединение ИЛИ.");
			var args2 = this.Operands.ToArray();

			if (args2.Length == 0)
			{
				throw new InvalidPredicateFormatException("Не правильный ИЛИ");
			}

			var retVal = args2[0].Evaluate();

			return args2.Skip(1).Aggregate(retVal, (current, equationEval) => current || equationEval.Evaluate());
		}

		/// <summary>
		/// Вычисляет объединение "ИЛИ" выражение и возвращает результат указывающий вычислено ли выражение или поддерживающее оптимизирующую трансформацию в SQL.
		/// </summary>
		/// <returns>
		/// Результат вычисления.
		/// </returns>
		public EvaluateResult EvaluateExt()
		{
			log.Debug("Формируем SQL объединения ИЛИ.");
			var convertibles = new Stack<EvaluateResult>();

			// NOTE: Сначала вычисляем уравнения,так как если хоть один вернет true, дальше можно не вычислять все выражение true (true || X = true)
			foreach (var itemResult in this.Operands.Select(equationEval => equationEval.EvaluateExt()))
			{
				switch (itemResult.Code)
				{
					case EvaluateResultCode.True:
						{
							// NOTE: Возвращаем EvaluateResultCode.True, так как "true || X = true", дальше вычислять не надо.
							return EvaluateResult.BuildEvaluated(true);
						}

					case EvaluateResultCode.False:
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
				return EvaluateResult.BuildEvaluated(false);
			}

			// NOTE: Если дошли сюда, значит нужна трансформация в sql
			return EvaluateResult.BuildTansformed(this.factory.CreateUnionConverter("or", convertibles));
		}
	}
}