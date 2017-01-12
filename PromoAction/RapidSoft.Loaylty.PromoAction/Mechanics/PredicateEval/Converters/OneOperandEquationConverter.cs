namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Converters
{
    using System.IO;

    using RapidSoft.Extensions;

    /// <summary>
    /// Конвертер для уравнения вида Операнд Оператор ПредопределенныйОперанд (NULL).
    /// </summary>
    internal class OneOperandEquationConverter : IPredicateSqlConverter
    {
        /// <summary>
        /// Операнд уравнения.
        /// </summary>
        private readonly string @operator;

        /// <summary>
        /// Оператор и предопределенный операнд.
        /// </summary>
        private readonly string operand;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneOperandEquationConverter"/> class.
        /// </summary>
        /// <param name="operand">
        /// Операнд уравнения.
        /// </param>
        /// <param name="operator">
        /// Оператор и предопределенный операнд.
        /// </param>
        public OneOperandEquationConverter(string operand, string @operator)
        {
            this.@operator = @operator;
            this.operand = operand;
        }

        /// <summary>
        /// Записывает предикат как выражение SQL.
        /// </summary>
        /// <param name="writer">
        /// Поток в который выполняется запись.
        /// </param>
        public void Convert(TextWriter writer)
        {
            writer.ThrowIfNull("writer");

            writer.Write(this.operand);
            writer.Write(this.@operator);
        }
    }
}