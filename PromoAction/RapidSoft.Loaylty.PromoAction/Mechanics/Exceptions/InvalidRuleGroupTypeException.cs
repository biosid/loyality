namespace RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions
{
	/// <summary>
	/// Представляет ошибку создания вычислятеля правил с не правильным типом правил.
	/// </summary>
	public class InvalidRuleGroupTypeException : RuleEvaluationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidRuleGroupTypeException"/> class.
		/// </summary>
		/// <param name="message">
		/// The message.
		/// </param>
		public InvalidRuleGroupTypeException(string message)
			: base(message)
		{
		}
	}
}
