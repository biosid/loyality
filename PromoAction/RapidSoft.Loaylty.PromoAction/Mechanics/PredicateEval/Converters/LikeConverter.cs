namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Converters
{
    using System.IO;

    using RapidSoft.Extensions;

    /// <summary>
    /// Реализация <see cref="IPredicateSqlConverter"/> для уравнения проверки вхождения подстроки операнда2 в операнд1.
    /// </summary>
    internal class LikeConverter : IPredicateSqlConverter
    {
        /// <summary>
        /// Первый операнд.
        /// </summary>
        private readonly string operand1;

        /// <summary>
        /// Оператор уравнения.
        /// </summary>
        private readonly string @operator;

        /// <summary>
        /// Второй операнд.
        /// </summary>
        private readonly string operand2;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeConverter"/> class.
        /// </summary>
        /// <param name="operand1">
        /// Первый операнд.
        /// </param>
        /// <param name="operator">
        /// Оператор уравнения.
        /// </param>
        /// <param name="operand2">
        /// Второй операнд.
        /// </param>
        public LikeConverter(string operand1, string @operator, string operand2)
        {
            this.operand1 = operand1;
            this.@operator = @operator;
            this.operand2 = operand2;
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

            writer.Write(this.operand1);
            writer.Write(this.@operator);
            writer.Write("'%");
            writer.Write(this.operand2.Trim('\''));
            writer.Write("%'");
        }
    }
}