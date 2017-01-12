namespace RapidSoft.Loaylty.PromoAction.Api.Entities
{
    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;

    /// <summary>
    /// �������� �����������.
    /// </summary>
    public class ConditionalFactor
    {
        /// <summary>
        /// ���������� �����, ��������� ������������ ��� ����������� �������, � ������� ����������� ������� ���������� �������� ������������� 
        /// � ����� ��������� ������������� ����������.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// ������� (��������) � ������� xml.
        /// </summary>
        public filter Predicate { get; set; }

        /// <summary>
        /// �������� ��������.
        /// </summary>
        public decimal Factor { get; set; }

        /// <summary>
        /// ���������� ����������������� ��������.
        /// </summary>
        /// <returns>
        /// ��������� ��������� (<see cref="filter"/>).
        /// </returns>
        public filter GetDeserializedPredicate()
        {
            return this.Predicate;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Factor: {0}, Priority: {1}, Predicate: {2}", this.Factor, this.Priority, this.Predicate.Serialize());
        }
    }
}