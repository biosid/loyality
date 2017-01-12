namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation
{
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

    /// <summary>
    /// Реализация проверки начилия значения.
    /// </summary>
    internal class NemEquation : EquationEvalBase
    {
        private readonly ILog log = LogManager.GetLogger(typeof(NemEquation));

        /// <summary>
        /// Initializes a new instance of the <see cref="NemEquation"/> class.
        /// </summary>
        /// <param name="values">
        /// Коллекция переменных уравнения/предиката.
        /// </param>
        /// <param name="factory">
        /// Фабрика конвертеров.
        /// </param>
        public NemEquation(VariableValue[] values, IEvalStrategySelector factory)
            : base(values, factory)
        {
        }
        
        /// <summary>
        /// Выполняет проверки начилия значения.
        /// Кол-во входный значений должно быть равно 1.
        /// </summary>
        /// <param name="objects">
        /// Коллекция переменных уравнения/предиката.
        /// </param>
        /// <returns>
        /// Результат вычисления.
        /// </returns>
        protected override bool DoEval(object[] objects)
        {
            if (objects.Length != 1)
            {
                throw new InvalidPredicateFormatException("Уравнение проверки на наличие значения должно содержать 1 переменную/литерал");
            }

            var str = objects[0] as string;

            if (objects[0] is string)
            {
                return !string.IsNullOrWhiteSpace(str);
            }

            return objects[0] != null;
        }

        /// <summary>
        /// Выполняет проверку наличия значения или трансформацию в SQL проверки входного значения.
        /// Кол-во входный значений должно быть равно 1.
        /// </summary>
        /// <param name="values">
        /// Коллекция значений переменных уравнения/предиката.
        /// </param>
        /// <returns>
        /// Результат вычисления.
        /// </returns>
        protected override EvaluateResult DoEvalExt(VariableValue[] values)
        {
            log.Debug("Формируем SQL проверки наличия значения.");
            if (values.Length != 1)
            {
                throw new InvalidPredicateFormatException("Уравнение проверки на наличие значения должно содержать 1 переменную/литерал");
            }

            return EvaluateResult.BuildTansformed(this.Factory.CreateEquationConverter(equationOperator.nem, values));
        }
    }
}