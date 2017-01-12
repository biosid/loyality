namespace RapidSoft.Loaylty.PromoAction.Mechanics.Converters
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;

	/// <summary>
	/// Реализация интерфейса <see cref="IRuleSqlConverter"/>
	/// </summary>
	public class RuleSqlConverter : IRuleSqlConverter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RuleSqlConverter"/> class.
		/// </summary>
		/// <param name="whenThen">
		/// Коллекция конвертеров <see cref="ISqlWhenThen"/>.
		/// </param>
		/// <param name="else">
		/// Конвертер выражения ветки ELSE.
		/// </param>
		public RuleSqlConverter(ICollection<ISqlWhenThen> whenThen, ISqlConvert @else)
		{
			whenThen.ThrowIfNull("whenThen");
			if (whenThen.Count == 0)
			{
				throw new ArgumentException("Коллекция конвертеров whenThen должна содержать минимум один элемент.");
			}

			@else.ThrowIfNull("@else");

			this.WhenThen = whenThen;
			this.Else = @else;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RuleSqlConverter"/> class.
		/// </summary>
		/// <param name="whenThen">
		/// Коллекция конвертеров <see cref="ISqlWhenThen"/>.
		/// </param>
		/// <param name="else">
		/// Конвертер выражения ветки ELSE.
		/// </param>
		public RuleSqlConverter(ICollection<ISqlWhenThen> whenThen, decimal @else)
		{
			whenThen.ThrowIfNull("whenThen");
			if (whenThen.Count == 0)
			{
				throw new ArgumentException("Коллекция конвертеров whenThen должна содержать минимум один элемент.");
			}

			@else.ThrowIfNull("@else");

			this.WhenThen = whenThen;
			this.Else = new FactorConverter(@else);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RuleSqlConverter"/> class.
		/// </summary>
		/// <param name="whenThen">
		/// Конвертер выражения WHEN предикат THEN выражение.
		/// </param>
		/// <param name="else">
		/// Конвертер выражения ветки ELSE.
		/// </param>
		public RuleSqlConverter(ISqlWhenThen whenThen, ISqlConvert @else)
		{
			whenThen.ThrowIfNull("whenThen");
			@else.ThrowIfNull("@else");

			this.WhenThen = new[] { whenThen };
			this.Else = @else;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RuleSqlConverter"/> class.
		/// </summary>
		/// <param name="whenThen">
		/// Конвертер выражения WHEN предикат THEN выражение.
		/// </param>
		/// <param name="else">
		/// Значение ветки ELSE.
		/// </param>
		public RuleSqlConverter(ISqlWhenThen whenThen, decimal @else)
		{
			whenThen.ThrowIfNull("whenThen");
			@else.ThrowIfNull("@else");

			this.WhenThen = new[] { whenThen };
			this.Else = new FactorConverter(@else);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RuleSqlConverter"/> class.
		/// </summary>
		/// <param name="predicateSqlConverter">
		/// Предикат выражения.
		/// </param>
		/// <param name="then">
		/// Конвертер выражения ветки THEN.
		/// </param>
		/// <param name="else">
		/// Значение ветки ELSE.
		/// </param>
		public RuleSqlConverter(IPredicateSqlConverter predicateSqlConverter, ISqlConvert then, decimal @else)
		{
			predicateSqlConverter.ThrowIfNull("predicateSqlConverter");
			then.ThrowIfNull("then");
			@else.ThrowIfNull("@else");

			this.WhenThen = new[] { new SqlWhenThen(predicateSqlConverter, then) };
			this.Else = new FactorConverter(@else);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RuleSqlConverter"/> class.
		/// </summary>
		/// <param name="predicateSqlConverter">
		/// Предикат выражения.
		/// </param>
		/// <param name="then">
		/// Значение для ветки THEN.
		/// </param>
		/// <param name="else">
		/// Значение ветки ELSE.
		/// </param>
		public RuleSqlConverter(IPredicateSqlConverter predicateSqlConverter, decimal then, decimal @else)
		{
			predicateSqlConverter.ThrowIfNull("predicateSqlConverter");
			then.ThrowIfNull("then");
			@else.ThrowIfNull("@else");

			this.WhenThen = new[] { new SqlWhenThen(predicateSqlConverter, then) };
			this.Else = new FactorConverter(@else);
		}

		/// <summary>
		/// Коллекция конвертеров <see cref="ISqlWhenThen"/>
		/// </summary>
		public IEnumerable<ISqlWhenThen> WhenThen { get; private set; }

		/// <summary>
		/// Конвертер выражения ветки ELSE.
		/// </summary>
		public ISqlConvert Else { get; private set; }

		/// <summary>
		/// Записывает правило как выражение SQL.
		/// </summary>
		/// <param name="writer">
		/// Поток в который выполняется запись.
		/// </param>
		public void Convert(TextWriter writer)
		{
			writer.ThrowIfNull("writer");

			writer.Case();

			foreach (var sqlWhenThen in this.WhenThen)
			{
				sqlWhenThen.Convert(writer);
			}

			writer.Else();
			this.Else.Convert(writer);
			writer.End();
		}
	}
}