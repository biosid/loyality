namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Entities;

    using Interfaces;

    internal class PartnerProductCateroryLinkRepository : BaseRepository, IPartnerProductCateroryLinkRepository
    {
        private const string Delimiter = "/";

        public IList<PartnerProductCategoryLink> SetPartnerProductCateroryLink(API.Entities.PartnerProductCategoryLink link, string userId)
        {
            using (var ctx = DbNewContext(userId))
            {
                var oldLinks =
                    ctx.PartnerProductCategoryLink.Where(
                        l => l.PartnerId == link.PartnerId && l.ProductCategoryId == link.ProductCategoryId);

                foreach (var oldLink in oldLinks)
                {
                    ctx.Entry(oldLink).State = EntityState.Deleted;
                }

                ctx.SaveChanges();

                var res = new List<PartnerProductCategoryLink>();

                if (link.Paths == null)
                {
                    return res;
                }

                foreach (var categoryPath in link.Paths.Where(p => !string.IsNullOrEmpty(p.NamePath)))
                {
                    var normal = this.NormalizeNamePath(categoryPath.NamePath);
                    var entity = new PartnerProductCategoryLink
                                 {
                                     ProductCategoryId = link.ProductCategoryId,
                                     PartnerId = link.PartnerId,
                                     NamePath = normal,
                                     IncludeSubcategories = categoryPath.IncludeSubcategories,
                                     InsertedDate = DateTime.Now,
                                     InsertedUserId = userId
                                 };

                    ctx.Entry(entity).State = EntityState.Added;

                    res.Add(entity);
                }

                ctx.SaveChanges();
                return res;
            }
        }

        public IList<PartnerProductCategoryLink> GetPartnerProductCateroryLinks(int partnerId, int[] categoryIds = null)
        {
            using (var ctx = DbNewContext())
            {
                var query = ctx.PartnerProductCategoryLink.Where(l => l.PartnerId == partnerId);

                if (categoryIds != null)
                {
                    query = query.Where(l => categoryIds.Contains(l.ProductCategoryId));
                }

                var reslut = query.ToArray();

                return reslut;
            }
        }

        public bool IsUniqueLinks(int partnerId, int categoryId, string partnerCategoryName)
        {
            if (string.IsNullOrEmpty(partnerCategoryName))
            {
                return true;
            }

            partnerCategoryName = string.Format("{0}{1}{2}", partnerCategoryName.StartsWith(Delimiter) ? string.Empty : Delimiter, partnerCategoryName, partnerCategoryName.EndsWith(Delimiter) ? string.Empty : Delimiter);

            using (var ctx = DbNewContext())
            {
                return !ctx.PartnerProductCategoryLink.Any(l => l.PartnerId == partnerId && l.ProductCategoryId != categoryId && l.NamePath == partnerCategoryName);
            }
        }

        private string NormalizeNamePath(string namePath)
        {
            var retVal = namePath;

			retVal = retVal.Replace("\\", Delimiter);

			if (!retVal.StartsWith(Delimiter))
            {
                retVal = Delimiter + retVal;
            }

			if (!retVal.EndsWith(Delimiter))
            {
                retVal = retVal + Delimiter;
            }

            return retVal;
        }
    }
}