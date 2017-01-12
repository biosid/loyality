namespace RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions
{
	/// <summary>
	/// Представляет ошибку уникальности правил в группе.
	/// </summary>
	public class InvalidPriorityException : RuleEvaluationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidPriorityException"/> class.
		/// </summary>
		/// <param name="message">
		/// The message.
		/// </param>
		public InvalidPriorityException(string message)
			: base(message)
		{
		}
	}
}
