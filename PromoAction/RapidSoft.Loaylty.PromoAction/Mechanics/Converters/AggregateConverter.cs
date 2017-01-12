namespace RapidSoft.Loaylty.PromoAction.Mechanics.Converters
{
    using System.Collections.Generic;
    using System.IO;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// Конвертер агрегирующего выражения.
    /// </summary>
    public class AggregateConverter : List<ISqlConvert>, ISqlConvert
    {
        /// <summary>
        /// Тип правил.
        /// </summary>
        private readonly RuleTypes ruleType;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateConverter"/> class.
        /// </summary>
        /// <param name="ruleType">
        /// Тип правил.
        /// </param>
        /// <param name="factor">
        /// Первый коэффициент.
        /// </param>
        public AggregateConverter(RuleTypes ruleType, decimal factor)
        {
            this.ruleType = ruleType;

            if (factor != this.ruleType.GetDefaultFactor())
            {
                this.Add(new FactorConverter(factor));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateConverter"/> class.
        /// </summary>
        /// <param name="ruleType">
        /// Тип правил.
        /// </param>
        public AggregateConverter(RuleTypes ruleType)
        {
            this.ruleType = ruleType;
        }

        /// <summary>
        /// Добавляет коллекция конверторов правил.
        /// </summary>
        /// <param name="converts">
        /// Коллекция конверторов правил.
        /// </param>
        public void Add(IEnumerable<ISqlConvert> converts)
        {
            this.AddRange(converts);
        }

        /// <summary>
        /// Записывает некоторое выражение в поток как выражение SQL.
        /// </summary>
        /// <param name="writer">
        /// Поток в который выполняется запись.
        /// </param>
        public void Convert(TextWriter writer)
        {
            if (this.Count == 0)
            {
                writer.Factor(this.ruleType.GetDefaultFactor());
            }

            var writeOperator = false;
            var @operator = this.ruleType.GetSqlOperation();

            foreach (var calculatorResult in this)
            {
                if (writeOperator)
                {
                    writer.Write(@operator);
                }

                var isFactor = calculatorResult is FactorConverter;

                if (!isFactor)
                {
                    writer.Write('(');
                }

                calculatorResult.Convert(writer);

                if (!isFactor)
                {
                    writer.Write(')');
                }

                writeOperator = true;
            }
        }
    }
}
