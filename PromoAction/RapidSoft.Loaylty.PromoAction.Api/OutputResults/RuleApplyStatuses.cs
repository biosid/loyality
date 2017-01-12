namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults 
{
	/// <summary>
	///  оды факта применени€ правил.
	/// </summary>
	public enum RuleApplyStatuses
	{
		/// <summary>
		/// ѕримен€лись только базовые правила.
		/// </summary>
		BaseOnlyRulesExecuted = 1,

		/// <summary>
		/// ѕримен€лись и базовые и не базовые правила.
		/// </summary>
		RulesExecuted = 2,

		/// <summary>
		/// ѕравила не примен€лись, исходное число не изменилось.
		/// </summary>
		RulesNotExecuted = 0
	}
}