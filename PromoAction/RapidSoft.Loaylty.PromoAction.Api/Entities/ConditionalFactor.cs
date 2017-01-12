namespace RapidSoft.Loaylty.PromoAction.Api.Entities
{
    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;

    /// <summary>
    /// Условный коэффициент.
    /// </summary>
    public class ConditionalFactor
    {
        /// <summary>
        /// Порядковый номер, приоритет используется для определения порядка, в котором проверяется цепочка предикатов условных коэффициентов 
        /// с целью получения единственного результата.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Условие (предикат) в формате xml.
        /// </summary>
        public filter Predicate { get; set; }

        /// <summary>
        /// Числовое значение.
        /// </summary>
        public decimal Factor { get; set; }

        /// <summary>
        /// Возвращает десериализованный предикат.
        /// </summary>
        /// <returns>
        /// Экземпляр предиката (<see cref="filter"/>).
        /// </returns>
        public filter GetDeserializedPredicate()
        {
            return this.Predicate;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Factor: {0}, Priority: {1}, Predicate: {2}", this.Factor, this.Priority, this.Predicate.Serialize());
        }
    }
}