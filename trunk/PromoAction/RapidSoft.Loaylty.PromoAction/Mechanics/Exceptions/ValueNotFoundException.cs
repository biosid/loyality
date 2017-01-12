namespace RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions
{
	/// <summary>
	/// Представляет ошибку возникающую когда в контексте не найдено значение переменной.
	/// </summary>
	public class ValueNotFoundException : RuleEvaluationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ValueNotFoundException"/> class.
		/// </summary>
		/// <param name="object">
		/// Название объекта.
		/// </param>
		/// <param name="name">
		/// Название свойства объекта.
		/// </param>
		public ValueNotFoundException(string @object, string name) :
			base(string.Format("Не найдено значение для переменной: object = {0}, name = {1}.", @object, name))
		{
			this.Object = @object;
			this.Name = name;
		}

		/// <summary>
		/// Название объекта.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Название свойства объекта.
		/// </summary>
		public string Object { get; private set; }
	}
}