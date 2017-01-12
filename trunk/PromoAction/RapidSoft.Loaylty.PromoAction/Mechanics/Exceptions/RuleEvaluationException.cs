namespace RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions
{
    using System;

    /// <summary>
	/// Представляет ошибки вычисления предиката.
	/// </summary>
	[Serializable]
	public class RuleEvaluationException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RuleEvaluationException"/> class.
		/// </summary>
		/// <param name="message">
		/// The message.
		/// </param>
		public RuleEvaluationException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RuleEvaluationException"/> class.
		/// </summary>
		protected RuleEvaluationException()
		{
		}
	}
}