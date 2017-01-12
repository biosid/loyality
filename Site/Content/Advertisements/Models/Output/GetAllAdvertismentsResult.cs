using System.Collections.Generic;
using Vtb24.Site.Content.Models;

namespace Vtb24.Site.Content.Advertisements.Models.Output
{
	public class GetAllAdvertisementsResult : PagedResult<ClientAdvertisement>
	{
		public GetAllAdvertisementsResult(IEnumerable<ClientAdvertisement> result, int totalCount, PagingSettings paging)
			: base(result, totalCount, paging)
		{
		}
	}

}
