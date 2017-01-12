namespace RapidSoft.VTB24.BankConnector.Extension
{
	using System;

	public static class PluralizeExtensions
	{
		#region Склонение с шаблонами (расширения)

		/// <summary>
		/// Обработать нужный шаблон в зависимости от склонения.
		/// <code>
		/// <![CDATA[
		///     // с тремя формами
		///     var message = n.Pluralize("{1} кочерга", "{2} кочерги", "{5} кочерёг");
		///     
		///     // c двумя формами
		///     var message2 = n.Pluralize("{1} кочергой", "{2} кочергами");
		///     
		///     // с форматированием
		///     var message3 = n.Pluralize("{1:N2} рубль", "{2:N2} рубля", "{5:N2} рублей");
		/// ]]>
		/// </code>
		/// </summary>
		/// <param name="num">Число</param>
		/// <param name="singular">Форма единственного числа (один)</param>
		/// <param name="plural">Паукальная счётная форма (два, пара); либо множественная, если plural2 = null</param>
		/// <param name="plural2">Форма множественного числа (много)</param>
		/// <returns>Результат склонения</returns>
		public static string Pluralize(this long num, string singular, string plural, string plural2 = null)
		{
			return Pluralize(num, () => Decline(num, singular, plural, plural2));
		}

		public static string Pluralize(this int num, string singular, string plural, string plural2 = null)
		{
			return Pluralize(num, () => Decline(num, singular, plural, plural2));
		}

		public static string Pluralize(this short num, string singular, string plural, string plural2 = null)
		{
			return Pluralize(num, () => Decline(num, singular, plural, plural2));
		}

		public static string Pluralize(this byte num, string singular, string plural, string plural2 = null)
		{
			return Pluralize(num, () => Decline(num, singular, plural, plural2));
		}

		public static string Pluralize(this decimal num, string singular, string plural, string plural2 = null)
		{
			return Pluralize(num, () => Decline(num, singular, plural, plural2));
		}

		public static string Pluralize(this double num, string singular, string plural, string plural2 = null)
		{
			return Pluralize(num, () => Decline(num, singular, plural, plural2));
		}

		public static string Pluralize(this float num, string singular, string plural, string plural2 = null)
		{
			return Pluralize(num, () => Decline(num, singular, plural, plural2));
		}

		#endregion

		#region Выбор склонения

		/// <summary>
		/// Выбрать склонение.
		/// <code>
		/// <![CDATA[
		///     // три формы
		///     var poker = PluralizeExtensions.Decline(n, "кочерга", "кочерги", "кочерёг");
		///     
		///     // две формы
		///     var poker2 = PluralizeExtensions.Decline(n, "кочергой", "кочергами");
		/// ]]>
		/// </code>
		/// </summary>
		/// <param name="num">Число</param>
		/// <param name="singular">Форма единственного числа (один)</param>
		/// <param name="plural">Паукальная счётная форма (два, пара); либо множественная, если plural2 = null</param>
		/// <param name="plural2">Форма множественного числа (много)</param>
		/// <typeparam name="T">Тип сущности для склонения</typeparam>
		/// <returns>Результат склонения T</returns>
		public static T Decline<T>(long num, T singular, T plural, T plural2 = null) where T : class
		{
			if (plural2 == null)
			{
				return num % 10 == 1 && num % 100 != 11 ? singular : plural;
			}

			var x = num % 100;
			if (x >= 5 && x <= 20)
			{
				return plural2;
			}

			switch (num % 10)
			{
				case 1:
					return singular;
				case 2:
				case 3:
				case 4:
					return plural;
				default:
					return plural2;
			}
		}

		public static T Decline<T>(decimal num, T singular, T plural, T plural2 = null) where T : class
		{
			return num.Equals((long)num) ? Decline((long)num, singular, plural, plural2) : plural;
		}

		public static T Decline<T>(double num, T singular, T plural, T plural2 = null) where T : class
		{
			return num.Equals((long)num) ? Decline((long)num, singular, plural, plural2) : plural;
		}

		#endregion

		#region Приватные методы

		private static string Pluralize<T>(T num, Func<string> plur) where T : struct
		{
			var declension = plur();
			return ParseTemplate(num, declension);
		}

		private static string ParseTemplate<T>(T num, string template) where T : struct
		{
			var t = template.Replace("{1", "{0").Replace("{2", "{0").Replace("{5", "{0");
			return t.Contains("{0") ? string.Format(t, num) : template;
		}

		#endregion
	}
}
