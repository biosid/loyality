namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values
{
	/// <summary>
	/// Представляет переменную предикта.
	/// </summary>
	public class VariableValue
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="VariableValue"/> class.
		/// </summary>
		/// <param name="objectName">
		/// Название объекта из которого извлекается значение переменной.
		/// </param>
		/// <param name="objectPropertyName">
		/// Название свойства объекта из которого извлекается значение переменной.
		/// </param>
		/// <param name="status">
		/// Статус переменной.
		/// </param>
		/// <param name="value">
		/// Значение переменной.
		/// </param>
		private VariableValue(string objectName, string objectPropertyName, VariableStatuses status, object value)
		{
			this.ObjectName = objectName;
			this.ObjectPropertyName = objectPropertyName;
			this.Status = status;
			this.Value = value;
		}

		/// <summary>
		/// Название объекта из которого извлекается значение переменной.
		/// Используется только для переменных конвертируюмых в SQL, то есть если <see cref="Status"/> равен <see cref="VariableStatuses.SqlColumn"/>
		/// </summary>
		public string ObjectName { get; private set; }

		/// <summary>
		/// Название свойства объекта из которого извлекается значение переменной.
		/// Используется только для переменных конвертируюмых в SQL, то есть если <see cref="Status"/> равен <see cref="VariableStatuses.SqlColumn"/>
		/// </summary>
		public string ObjectPropertyName { get; private set; }
		
		/// <summary>
		/// Статус переменной.
		/// </summary>
		public VariableStatuses Status { get; private set; }

		/// <summary>
		/// Значение переменной.
		/// </summary>
		public object Value { get; private set; }

		public static VariableValue BuildValue(object value)
		{
			return new VariableValue(null, null, VariableStatuses.SimplyValue, value);
		}

		public static VariableValue BuildSql(string objectName, string objectPropertyName, string alias)
		{
			return new VariableValue(objectName, objectPropertyName, VariableStatuses.SqlColumn, alias);
		}

		public static VariableValue BuildSpecial(string objectName, string objectPropertyName, object value)
		{
			return new VariableValue(objectName, objectPropertyName, VariableStatuses.SpecialValue, value);
		}
	}
}