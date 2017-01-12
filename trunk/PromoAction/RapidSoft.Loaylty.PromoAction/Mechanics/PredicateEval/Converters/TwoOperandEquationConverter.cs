namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Converters
{
    using System.IO;

    using RapidSoft.Extensions;

    /// <summary>
    /// ��������� ��� ��������� ���� Operand1 Oprerator Operand2.
    /// </summary>
    internal class TwoOperandEquationConverter : IPredicateSqlConverter
    {
        /// <summary>
        /// ������ �������.
        /// </summary>
        private readonly string operand1;

        /// <summary>
        /// �������� ���������.
        /// </summary>
        private readonly string @operator;

        /// <summary>
        /// ������ �������.
        /// </summary>
        private readonly string operand2;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoOperandEquationConverter"/> class.
        /// </summary>
        /// <param name="operand1">
        /// ������ �������.
        /// </param>
        /// <param name="operator">
        /// �������� ���������.
        /// </param>
        /// <param name="operand2">
        /// ������ �������.
        /// </param>
        public TwoOperandEquationConverter(string operand1, string @operator, string operand2)
        {
            this.@operator = @operator;
            this.operand1 = operand1;
            this.operand2 = operand2;
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

            writer.Write(this.operand1);
            writer.Write(this.@operator);
            writer.Write(this.operand2);
        }
    }
}