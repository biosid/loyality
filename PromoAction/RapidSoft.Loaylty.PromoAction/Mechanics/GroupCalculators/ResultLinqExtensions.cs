namespace RapidSoft.Loaylty.PromoAction.Mechanics.GroupCalculators
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;

    /// <summary>
	/// Методы расширители для работы с коллекциями результатов.
	/// </summary>
	internal static class ResultLinqExtensions
	{
		/// <summary>
		/// Выбирает результаты с кодом <see cref="EvaluateResultCode.ConvertibleToSQL"/> плюс один первый с кодом <see cref="EvaluateResultCode.True"/>.
		/// </summary>
		/// <param name="source">
		/// The source.
		/// </param>
		/// <typeparam name="T">
		/// Тип результата (<see cref="IResult"/>)
		/// </typeparam>
		/// <returns>
		/// The <see cref="IEnumerable{T}"/>.
		/// </returns>
		public static IEnumerable<T> TakeToFirstTrue<T>(this IEnumerable<T> source) where T : IResult
		{
			foreach (var ruleResult in source.Where(ruleResult => ruleResult.Code != EvaluateResultCode.False))
			{
				if (ruleResult.Code == EvaluateResultCode.ConvertibleToSQL)
				{
					yield return ruleResult;
				}

				if (ruleResult.Code == EvaluateResultCode.True)
				{
					yield return ruleResult;
					yield break;
				}
			}
		}
	}
}