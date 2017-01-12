namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation
{
	using System;
	using System.Linq;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
	using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

	/// <summary>
	/// ������� ���������� ���������� ���������/��������� (<see cref="equation"/>) �������������� ������������� � SQL.
	/// </summary>
	internal abstract class EquationEvalBase : IEquationEval
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EquationEvalBase"/> class.
		/// </summary>
		/// <param name="values">
		/// ��������� ���������� ���������/���������.
		/// </param>
		/// <param name="factory">
		/// ������� �����������.
		/// </param>
		protected EquationEvalBase(VariableValue[] values, IEvalStrategySelector factory)
		{
			values.ThrowIfNull("values");
			this.Values = values;
			this.Factory = factory;
		}

		/// <summary>
		/// ��������� ���������� ���������/���������.
		/// </summary>
		protected VariableValue[] Values { get; private set; }

		/// <summary>
		/// ������� �����������.
		/// </summary>
		protected IEvalStrategySelector Factory { get; private set; }

		/// <summary>
		/// ��������� �������� ����������, ����������� �������� � �������� ��������� ����� <see cref="DoEval"/> ��� ���������� ����������.
		/// </summary>
		/// <returns>
		/// ��������� ���������� ���������/���������.
		/// </returns>
		public bool Evaluate()
		{
			if (this.Values.Any(x => x.Status != VariableStatuses.SimplyValue))
			{
				throw new NotSupportedException("���������� ���������� �������� ������ � ������������ � ������� VariableStatuses.SimplyValue");
			}

			var objects = this.Values.Select(x => x.Value).ToArray();
			return this.DoEval(objects);
		}

		/// <summary>
		/// ��������� ��������� � ���������� ��������� ����������� ��������� �� ��������� ��� �������������� �������������� ������������� � SQL.
		/// </summary>
		/// <returns>
		/// ��������� ����������.
		/// </returns>
		public EvaluateResult EvaluateExt()
		{
			if (this.Values.All(x => x.Status == VariableStatuses.SimplyValue))
			{
				var objects = this.Values.Select(x => x.Value).ToArray();
				var result = this.DoEval(objects);
				return EvaluateResult.BuildEvaluated(result);
			}

			return this.DoEvalExt(this.Values);
		}

		/// <summary>
		/// ��������� ������ ����������� ���������� ��� ������������� � T-SQL ����������� ���������/���������. ����������� ������������.
		/// </summary>
		/// <param name="values">
		/// ��������� �������� ���������� ���������/���������.
		/// </param>
		/// <returns>
		/// ��������� ����������.
		/// </returns>
		protected abstract EvaluateResult DoEvalExt(VariableValue[] values);

		/// <summary>
		/// ��������� ������ ����������� ���������� ����������� ���������/���������. ����������� ������������.
		/// </summary>
		/// <param name="objects">
		/// ��������� ���������� ���������/���������.
		/// </param>
		/// <returns>
		/// ��������� ����������.
		/// </returns>
		protected abstract bool DoEval(object[] objects);
	}
}