namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values
{
	/// <summary>
	/// ������ ����������.
	/// </summary>
	public enum VariableStatuses
	{
		/// <summary>
		/// ���������� ���������� ��� ������� ��� ���������� �� ���������.
		/// </summary>
		SimplyValue,

		/// <summary>
		/// ���������� �� ���������� � �������� ����� ������� ��� ��������� SQL.
		/// </summary>
		SqlColumn,

		/// <summary>
		/// ���������� ��������� �� ������� ���������.
		/// </summary>
		SpecialValue
	}
}