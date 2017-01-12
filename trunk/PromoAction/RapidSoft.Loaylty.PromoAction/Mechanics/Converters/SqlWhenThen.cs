namespace RapidSoft.Loaylty.PromoAction.Mechanics.Converters
{
	using System.IO;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;

	/// <summary>
	/// Реализация интерфейса <see cref="ISqlWhenThen"/>
	/// </summary>
	public class SqlWhenThen : ISqlWhenThen
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SqlWhenThen"/> class.
		/// </summary>
		/// <param name="predicateSqlConverter">
		/// Предикат выражения.
		/// </param>
		/// <param name="then">
		/// Выражение для ветки THEN.
		/// </param>
		public SqlWhenThen(IPredicateSqlConverter predicateSqlConverter, ISqlConvert then)
		{
			predicateSqlConverter.ThrowIfNull("predicateSqlConverter");
			then.ThrowIfNull("then");
			
			this.PredicateSqlConverter = predicateSqlConverter;
			this.Then = then;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlWhenThen"/> class.
		/// </summary>
		/// <param name="predicateSqlConverter">
		/// Предикат выражения.
		/// </param>
		/// <param name="then">
		/// Значение для ветки THEN.
		/// </param>
		public SqlWhenThen(IPredicateSqlConverter predicateSqlConverter, decimal then)
		{
			predicateSqlConverter.ThrowIfNull("predicateSqlConverter");

			this.PredicateSqlConverter = predicateSqlConverter;
			this.Then = new FactorConverter(then);
		}

		/// <summary>
		/// Предикат выражения.
		/// </summary>
		public IPredicateSqlConverter PredicateSqlConverter { get; private set; }

		/// <summary>
		/// Выражение для ветки THEN.
		/// </summary>
		public ISqlConvert Then { get; private set; }

		/// <summary>
		/// Записывает выражение WHEN ПРЕДИКАТ THEN ВЫРАЖЕНИЕ в поток как выражение SQL.
		/// </summary>
		/// <param name="writer">
		/// Поток в который выполняется запись.
		/// </param>
		public void Convert(TextWriter writer)
		{
			writer.ThrowIfNull("writer");
			writer.When();
			this.PredicateSqlConverter.Convert(writer);
			writer.Then();
			this.Then.Convert(writer);
		}
	}
}