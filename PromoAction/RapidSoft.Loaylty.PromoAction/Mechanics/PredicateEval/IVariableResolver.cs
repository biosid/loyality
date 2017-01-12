namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval
{
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

    /// <summary>
    /// Интерфейс определения значения переменной или строкового литерала.
    /// </summary>
    public interface IVariableResolver
    {
        /// <summary>
        /// Определяет значения переменной по контексту или набору алиасов.
        /// </summary>
        /// <param name="value">
        /// Переменная\литерал уравнения.
        /// </param>
        /// <returns>
        /// Полученная переменная предиката.
        /// </returns>
        VariableValue ResolveVariable(value value);

        /// <summary>
        /// Определяет значения переменной по контексту или набору алиасов.
        /// </summary>
        /// <param name="objectName">
        /// The object name.
        /// </param>
        /// <param name="objectPropertyName">
        /// The object property name.
        /// </param>
        /// <param name="valueType">
        /// The value type.
        /// </param>
        /// <returns>
        /// The <see cref="VariableValue"/>.
        /// </returns>
        VariableValue ResolveVariable(string objectName, string objectPropertyName, valueType valueType);
    }
}
