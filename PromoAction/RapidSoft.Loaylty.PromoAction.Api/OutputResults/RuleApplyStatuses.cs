namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults 
{
	/// <summary>
	/// ���� ����� ���������� ������.
	/// </summary>
	public enum RuleApplyStatuses
	{
		/// <summary>
		/// ����������� ������ ������� �������.
		/// </summary>
		BaseOnlyRulesExecuted = 1,

		/// <summary>
		/// ����������� � ������� � �� ������� �������.
		/// </summary>
		RulesExecuted = 2,

		/// <summary>
		/// ������� �� �����������, �������� ����� �� ����������.
		/// </summary>
		RulesNotExecuted = 0
	}
}