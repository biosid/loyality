namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval
{
    using System.IO;

    /// <summary>
    /// Результат трансформирующего вычисления.
    /// </summary>
    public class EvaluateResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateResult"/> class.
        /// </summary>
        private EvaluateResult()
        {
        }

        /// <summary>
        /// Конвертер в SQL.
        /// </summary>
        public IPredicateSqlConverter Converter { get; private set; }

        /// <summary>
        /// Код результата вычисления.
        /// </summary>
        public EvaluateResultCode Code { get; private set; }

        /// <summary>
        /// Статический конструктор объекта-результата с кодами <see cref="EvaluateResultCode.True"/> или <see cref="EvaluateResultCode.False"/> 
        /// в зависимости от <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// Результат вычисления не требующего трансформации в SQL.
        /// </param>
        /// <returns>
        /// Созданный объект-результат.
        /// </returns>
        public static EvaluateResult BuildEvaluated(bool value)
        {
            return new EvaluateResult { Code = value ? EvaluateResultCode.True : EvaluateResultCode.False, Converter = null };
        }

        /// <summary>
        /// Статический конструктор объекта-результата с кодом <see cref="EvaluateResultCode.ConvertibleToSQL"/>.
        /// </summary>
        /// <param name="converter">
        /// Конвертер предиката в SQL.
        /// </param>
        /// <returns>
        /// Созданный объект-результат.
        /// </returns>
        public static EvaluateResult BuildTansformed(IPredicateSqlConverter converter)
        {
            return new EvaluateResult { Code = EvaluateResultCode.ConvertibleToSQL, Converter = converter };
        }

        /// <summary>
        /// Записывает в поток результат вычисления как выражение SQL.
        /// </summary>
        /// <param name="write">
        /// Поток в который выполняется запись.
        /// </param>
        public void Convert(TextWriter write)
        {
            if (this.Converter == null)
            {
                return;
            }

            this.Converter.Convert(write);
        }
    }
}