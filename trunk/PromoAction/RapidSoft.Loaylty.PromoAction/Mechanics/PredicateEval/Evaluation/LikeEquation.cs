namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation
{
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

    /// <summary>
    /// Реализация проверки вхождения подстроки операнда2 в операнд1.
    /// </summary>
    internal class LikeEquation : EquationEvalBase
    {
        private readonly ILog log = LogManager.GetLogger(typeof(LikeEquation));

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeEquation"/> class.
        /// </summary>
        /// <param name="values">
        /// Коллекция переменных уравнения/предиката.
        /// </param>
        /// <param name="factory">
        /// Фабрика конвертеров.
        /// </param>
        public LikeEquation(VariableValue[] values, IEvalStrategySelector factory)
            : base(values, factory)
        {
        }

        /// <summary>
        /// Выполняет проверку вхождения подстроки операнда2 в операнд1.
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
            log.Debug("Вычисляем проверку эквивалентности.");
            if (objects.Length != 2)
            {
                throw new InvalidPredicateFormatException("Уравнение проверки на вхождение подстроки операнда2 в операнд1 должно содержать 2 переменных/литерала");
            }

            if (objects[0] == null || objects[1] == null)
            {
                return false;
            }

            var operand1 = objects[0] as string;
            var operand2 = objects[1] as string;

            if (operand1 == null || operand2 == null)
            {
                throw new InvalidPredicateFormatException("Уравнение ппроверки на вхождение подстроки операнда2 в операнд1 возможно только для строк");
            }

            return operand1.Contains(operand2);
        }

        /// <summary>
        /// Выполняет проверку вхождения подстроки операнда2 в операнд1 или трансформацию в SQL проверки входных значений.
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
            log.Debug("Формируем SQL проверки эквивалентности.");
            if (values.Length != 2)
            {
                throw new InvalidPredicateFormatException("Уравнение проверки на эквивалентность должно содержать 2 переменных/литерала");
            }

            return EvaluateResult.BuildTansformed(this.Factory.CreateEquationConverter(equationOperator.like, values));
        }
    }
}