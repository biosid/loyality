namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;

    internal class CategoryPermissionRepository : BaseRepository, ICategoryPermissionRepository
    {
        public CategoryPermissionRepository()
            : base(DataSourceConfig.ConnectionString)
        {
        }

        public CategoryPermissionRepository(string connectionString)
            : base(connectionString)
        {
        }

        public string[] GetProductIdsHavingPermissions(string[] productIds)
        {
            using (var ctx = DbNewContext())
            {
                return ctx.ProductSortProjections
                          .Where(p => productIds.Contains(p.ProductId) &&
                                      ctx.CategoryPermissions.Any(cp => cp.PartnerId == p.PartnerId &&
                                                                        cp.CategoryId == p.CategoryId))
                          .Select(p => p.ProductId)
                          .ToArray();
            }
        }

        public IList<CategoryPermission> GetByPartner(int partnerId)
        {
            using (var ctx = this.DbNewContext())
            {
                var retList = ctx.CategoryPermissions.Where(x => x.PartnerId == partnerId).ToList();

                return retList;
            }
        }

        public void Save(string userId, IList<CategoryPermission> categoryPermissions)
        {
            using (var ctx = this.DbNewContext(userId))
            {
                foreach (var categoryPermission in categoryPermissions)
                {
                    var entity =
                    ctx.CategoryPermissions.SingleOrDefault(x => x.PartnerId == categoryPermission.PartnerId && x.CategoryId == categoryPermission.CategoryId);

                    if (entity == null)
                    {
                        ctx.CategoryPermissions.Add(categoryPermission);
                    }
                }

                ctx.SaveChanges();
            }
        }

        public void Delete(string userId, int partnerId, IList<int> categoryIds)
        {
            using (var ctx = this.DbNewContext(userId))
            {
                var entities = ctx.CategoryPermissions.Where(x => x.PartnerId == partnerId && categoryIds.Contains(x.CategoryId));

                foreach (var categoryPermission in entities)
                {
                    ctx.CategoryPermissions.Remove(categoryPermission);
                }

                ctx.SaveChanges();
            }
        }

        public void Delete(IList<int> categoryIds)
        {
            using (var ctx = this.DbNewContext())
            {
                var entities = ctx.CategoryPermissions.Where(x => categoryIds.Contains(x.CategoryId));

                foreach (var categoryPermission in entities)
                {
                    ctx.CategoryPermissions.Remove(categoryPermission);
                }

                ctx.SaveChanges();
            }    
        }
    }
}