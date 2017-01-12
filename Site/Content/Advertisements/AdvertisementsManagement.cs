using System;
using System.Data.Entity;
using System.Linq;
using Vtb24.Site.Content.Advertisements.Models.Output;
using Vtb24.Site.Content.DataAccess;
using Vtb24.Site.Content.Models;

namespace Vtb24.Site.Content.Advertisements
{
    public class AdvertisementsManagement : IAdvertisementsManagement
    {
		public GetAllAdvertisementsResult GetAdvertisements(string clientId, PagingSettings paging)
		{
			using (var dbContext = new ContentServiceDbContext())
			{
			    var query = dbContext.ClientAdvertisements
			                         .Where(m => m.ClientId == clientId)
			                         .OrderBy(m => m.ShowCounter);

				var totalCount = query.Count();

			    var advertisements = query.Skip(paging.Skip)
			                              .Take(paging.Take)
			                              .Include(m => m.Advertisement)
			                              .ToArray();

				return new GetAllAdvertisementsResult(advertisements, totalCount, paging);
			}
		}

        public void IncreaseShowCounter(long id, string clientId)
        {
            using (var dbContext = new ContentServiceDbContext())
            {
                var clientAdvertisement = dbContext.ClientAdvertisements.Find(id, clientId);

                clientAdvertisement.ShowCounter++;

                dbContext.SaveChanges();
            }
        }
    }
}
