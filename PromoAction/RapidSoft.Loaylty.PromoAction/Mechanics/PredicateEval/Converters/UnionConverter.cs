namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Converters
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using RapidSoft.Extensions;

	/// <summary>
	/// Конвертер объединений И и ИЛИ.
	/// </summary>
	internal class UnionConverter : IPredicateSqlConverter
	{
		/// <summary>
		/// Оператор объединения.
		/// </summary>
		private readonly string @operator;

		/// <summary>
		/// Коллекция операндов.
		/// </summary>
		private readonly IEnumerable<EvaluateResult> operands;

		/// <summary>
		/// Initializes a new instance of the <see cref="UnionConverter"/> class.
		/// </summary>
		/// <param name="operator">
		/// Оператор объединения.
		/// </param>
		/// <param name="operands">
		/// Коллекция операндов объединения.
		/// </param>
		public UnionConverter(string @operator, IEnumerable<EvaluateResult> operands)
		{
			operands.ThrowIfNull("operands");
			this.@operator = @operator;
			this.operands = operands;
		}

		/// <summary>
		/// Записывает предикат как выражение SQL.
		/// </summary>
		/// <param name="writer">
		/// Поток в который выполняется запись.
		/// </param>
		public void Convert(TextWriter writer)
		{
			writer.ThrowIfNull("writer");

			var args = this.operands.ToArray();

			if (args.Length == 0)
			{
				return;
			}

			if (args.Length == 1)
			{
				args[0].Convert(writer);
				return;
			}

			var isFirst = true;
			writer.Write('(');
			foreach (var evaluateResult in this.operands.Select(x => x.Converter))
			{
				if (!isFirst)
				{
					writer.Write(this.@operator);
				}

				evaluateResult.Convert(writer);
				isFirst = false;
			}

			writer.Write(')');
		}
	}
}