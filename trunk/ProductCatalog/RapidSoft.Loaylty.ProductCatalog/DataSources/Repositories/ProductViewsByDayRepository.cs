namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Entities;

    public class ProductViewsByDayRepository : IProductViewsByDayRepository
    {
        public void SaveViews(DateTime date, KeyValuePair<string, int>[] views)
        {
            date = date.Date;

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                if (ctx.ProductViewsByDays.Any(v => v.ViewsDate == date))
                {
                    throw new InvalidOperationException("для данной даты данные уже загружены");
                }

                foreach (var view in views)
                {
                    ctx.ProductViewsByDays.Add(new ProductViewsByDay
                    {
                        ViewsDate = date,
                        ProductId = view.Key,
                        ViewsCount = view.Value
                    });
                }

                ctx.SaveChanges();

                ctx.Database.ExecuteSqlCommand(
                    "[prod].[FillPopularProducts] @viewsDate",
                    new SqlParameter("@viewsDate", date));
            }
        }
    }
}
