namespace RapidSoft.Loaylty.PartnersConnector.QuarzTasks
{
    using Extensions;

    using Quartz;

	/// <summary>
	/// Класс содержит набор методов-расширителей для работы с <see cref="JobDataMap"/>.
	/// </summary>
	public static class JobDataMapExtentions
	{
		/// <summary>
		/// Помещает значение <paramref name="value"/> в набор данных задачи под ключом <paramref name="key"/>.
		/// </summary>
		/// <param name="jobDataMap">
		/// Коллекция данных задачи.
		/// </param>
		/// <param name="key">
		/// Ключ данных.
		/// </param>
		/// <param name="value">
		/// Добавляемые данные.
		/// </param>
		/// <typeparam name="T">
		/// Тип данных.
		/// </typeparam>
		public static void Put<T>(this JobDataMap jobDataMap, DataKeys key, T value)
		{
			var str = key.ToString();

			jobDataMap.Put(str, value);
		}

		/// <summary>
		/// Возвращает данные из набора данных задачи.
		/// </summary>
		/// <param name="jobDataMap">
		/// Коллекция данных задачи.
		/// </param>
		/// <param name="key">
		/// Ключ данных.
		/// </param>
		/// <returns>
		/// Найденные данные или <c>null</c>.
		/// </returns>
		public static object Get(this JobDataMap jobDataMap, DataKeys key)
		{
			jobDataMap.ThrowIfNull("jobDataMap");

			var str = key.ToString();

			return jobDataMap[str];
		}

		/// <summary>
		/// Возвращает данные из набора данных задачи как строку. Вызывается метод <see cref="object.ToString"/>.
		/// </summary>
		/// <param name="jobDataMap">
		/// Коллекция данных задачи.
		/// </param>
		/// <param name="dataKeys">
		/// Ключ данных.
		/// </param>
		/// <returns>
		/// Строка данных.
		/// </returns>
		public static string GetAsString(this JobDataMap jobDataMap, DataKeys dataKeys)
		{
			jobDataMap.ThrowIfNull("jobDataMap");

			var key = dataKeys.ToString();

			var result = jobDataMap[key];

			return result == null ? null : result.ToString();
		}

		/// <summary>
		/// Возвращает данные из набора данных задачи как экземпляр класс <typeparamref name="T"/>.
		/// </summary>
		/// <param name="jobDataMap">
		/// Коллекция данных задачи.
		/// </param>
		/// <param name="dataKeys">
		/// Ключ данных.
		/// </param>
		/// <typeparam name="T">
		/// Тип возвращаемых данных.
		/// </typeparam>
		/// <returns>
		/// Данные типа <paramref name="T"/>.
		/// </returns>
		public static T GetAs<T>(this JobDataMap jobDataMap, DataKeys dataKeys) where T : class
		{
			jobDataMap.ThrowIfNull("jobDataMap");

			var key = dataKeys.ToString();

			var result = jobDataMap[key];

			return result == null ? null : result as T;
		}

		/// <summary>
		/// Возвращает данные из набора данных задачи как число.
		/// </summary>
		/// <param name="jobDataMap">
		/// Коллекция данных задачи.
		/// </param>
		/// <param name="dataKeys">
		/// Ключ данных.
		/// </param>
		/// <returns>
		/// Найденное число или 0.
		/// </returns>
		public static int GetIntValue(this JobDataMap jobDataMap, DataKeys dataKeys)
		{
			jobDataMap.ThrowIfNull("jobDataMap");

			var key = dataKeys.ToString();

			return jobDataMap.GetIntValue(key);
		}
	}
}