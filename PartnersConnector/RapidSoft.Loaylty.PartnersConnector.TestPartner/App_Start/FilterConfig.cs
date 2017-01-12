using System.Web;
using System.Web.Mvc;

namespace RapidSoft.Loaylty.PartnersConnector.TestPartner
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}