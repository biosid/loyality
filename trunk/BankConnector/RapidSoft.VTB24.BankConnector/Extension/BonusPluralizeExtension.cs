namespace RapidSoft.VTB24.BankConnector.Extension
{
	using System;

	public class BonusPluralizeExtension
	{
		public static string GetPlural(decimal bonus)
		{
			var plural = Math.Round(bonus).Pluralize("{1:N0} бонус", "{2:N0} бонуса", "{5:N0} бонусов");
			return plural;
		}
	}
}
