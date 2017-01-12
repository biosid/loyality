namespace RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions
{
	/// <summary>
	/// Представляет ошибку возникающую когда предикат содержит ошибку, которая не позволяет выполнить вычисление предиката.
	/// </summary>
	public class InvalidPredicateFormatException : RuleEvaluationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidPredicateFormatException"/> class.
		/// </summary>
		/// <param name="message">
		/// The message.
		/// </param>
		public InvalidPredicateFormatException(string message)
			: base(message)
		{
		}
	}
}