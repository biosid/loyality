namespace RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator
{
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Converters;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;

    /// <summary>
	/// Представляет результат вычисления условного коэффициента.
	/// </summary>
	public class ConditionalResult : IResult
	{
		/// <summary>
		/// Вычисленный коэффициент правила, так как правило может иметь условные коэффициенты <see cref="Rule.ConditionalFactor"/>.
		/// </summary>
		public decimal Factor { get; protected set; }

		/// <summary>
		/// Конвертер условного коэффициент в выражение WHEN предикат THEN коэффициент.
		/// </summary>
		public ISqlWhenThen Converter { get; protected set; }

		/// <summary>
		/// Код результата вычисления.
		/// </summary>
		public EvaluateResultCode Code { get; protected set; }

		public static ConditionalResult BuildFalse(RuleTypes ruleType)
		{
			return new ConditionalResult
				       {
					       Code = EvaluateResultCode.False,
					       Factor = ruleType.GetDefaultFactor(),
					       Converter = null
				       };
		}

		public static ConditionalResult BuildTrue(decimal factor)
		{
			return new ConditionalResult
				       {
					       Code = EvaluateResultCode.True, 
					       Factor = factor, 
					       Converter = null
				       };
		}

		public static ConditionalResult BuildConvertible(RuleTypes ruleType, IPredicateSqlConverter predicate, decimal factor)
		{
			return new ConditionalResult
				       {
					       Code = EvaluateResultCode.ConvertibleToSQL,
					       Factor = ruleType.GetDefaultFactor(),
					       Converter = new SqlWhenThen(predicate, factor)
				       };
		}
	}
}