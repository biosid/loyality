namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

    /// <summary>
    /// Интерфейс выбора стратегии вычисления уравнения/объединения и создания конвертера в SQL.
    /// </summary>
    public interface IEvalStrategySelector
    {
        /// <summary>
        /// Выбирает стратегию вычисления уравнения/объединения.
        /// </summary>
        /// <param name="filter">
        /// Вычисляемое уравнение/объединение.
        /// </param>
        /// <param name="settings">
        /// Настройки вычисления.
        /// </param>
        /// <returns>
        /// Вычислятель уравнения/объединения.
        /// </returns>
        IEquationEval SelectEvalStrategy(filter filter, Settings settings);

        /// <summary>
        /// Создание конвертера объединения.
        /// </summary>
        /// <param name="unionType">
        /// Тип объединения.
        /// </param>
        /// <param name="operands">
        /// Коллекция операндов.
        /// </param>
        /// <returns>
        /// Конвертер объединения.
        /// </returns>
        IPredicateSqlConverter CreateUnionConverter(string unionType, IEnumerable<EvaluateResult> operands);

        /// <summary>
        /// Создание конвертера уравнения.
        /// </summary>
        /// <param name="equationOperator">
        /// Тип уравнения.
        /// </param>
        /// <param name="values">
        /// Переменные уравнения.
        /// </param>
        /// <returns>
        /// Конвертер уравнения.
        /// </returns>
        IPredicateSqlConverter CreateEquationConverter(equationOperator equationOperator, params VariableValue[] values);
    }
}