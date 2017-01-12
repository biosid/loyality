namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Converters
{
    using System.IO;

    using RapidSoft.Extensions;

    /// <summary>
    /// ��������� ��� ��������� ���� ������� �������� ����������������������� (NULL).
    /// </summary>
    internal class OneOperandEquationConverter : IPredicateSqlConverter
    {
        /// <summary>
        /// ������� ���������.
        /// </summary>
        private readonly string @operator;

        /// <summary>
        /// �������� � ���������������� �������.
        /// </summary>
        private readonly string operand;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneOperandEquationConverter"/> class.
        /// </summary>
        /// <param name="operand">
        /// ������� ���������.
        /// </param>
        /// <param name="operator">
        /// �������� � ���������������� �������.
        /// </param>
        public OneOperandEquationConverter(string operand, string @operator)
        {
            this.@operator = @operator;
            this.operand = operand;
        }

        /// <summary>
        /// ���������� �������� ��� ��������� SQL.
        /// </summary>
        /// <param name="writer">
        /// ����� � ������� ����������� ������.
        /// </param>
        public void Convert(TextWriter writer)
        {
            writer.ThrowIfNull("writer");

            writer.Write(this.operand);
            writer.Write(this.@operator);
        }
    }
}