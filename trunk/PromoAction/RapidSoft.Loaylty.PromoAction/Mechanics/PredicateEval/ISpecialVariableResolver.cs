namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval
{
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

    /// <summary>
	/// Интерфейс классов вычисляющих значение переменной по особому алгоритму.
	/// </summary>
	public interface ISpecialVariableResolver
	{
		bool IsResolved(value value, out VariableValue resolvedValue);
	}
}