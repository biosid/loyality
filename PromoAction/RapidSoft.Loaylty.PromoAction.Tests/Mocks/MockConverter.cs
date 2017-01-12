namespace RapidSoft.Loaylty.PromoAction.Tests.Mocks
{
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;

	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	internal class MockConverter : IPredicateSqlConverter
	{
		private readonly string predicate;

		[DebuggerStepThrough]
		public MockConverter(string predicate)
		{
			this.predicate = predicate;
		}

		[DebuggerStepThrough]
		public void Convert(TextWriter writer)
		{
			writer.ThrowIfNull("writer");

			writer.Write(this.predicate);
		}
	}
}
