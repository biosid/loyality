using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using RapidSoft.Loaylty.PartnersConnector.Litres.DataAccess.Entities;

namespace RapidSoft.Loaylty.PartnersConnector.Litres.DataAccess.Repositories
{
    public class LitresDownloadCodesRepository : IDisposable
    {
        private readonly LitresContext _context = new LitresContext();

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public string[] BindCodes(string partnerProductId, int count, int orderId)
        {
            if (string.IsNullOrEmpty(partnerProductId))
            {
                throw new ArgumentException("партнерский ID продукта не должен быть пустым", "partnerProductId");
            }

            if (count <= 0)
            {
                throw new ArgumentException("количество кодов должно быть положительным числом", "count");
            }

            var codes = _context.LitresDownloadCodes
                                .Where(c => c.PartnerProductId == partnerProductId && c.OrderId == null)
                                .OrderBy(c => c.Id)
                                .Take(count)
                                .ToArray();

            if (codes.Length != count)
            {
                throw new InvalidOperationException("недостаточно кодов для вознаграждения: id = " + partnerProductId + ", count = " + count.ToString("D"));
            }

            foreach (var code in codes)
            {
                code.OrderId = orderId;
            }

            return codes.Select(c => c.Code).ToArray();
        }

        public int GetRemainingCount(string partnerProductId)
        {
            return _context.LitresDownloadCodes.Count(c => c.PartnerProductId == partnerProductId && c.OrderId == null);
        }

        public IEnumerable<LitresRemainingCodesCount> GetAllRemainingCodesCount()
        {
            const string QUERY = @";with
Codes as (select PartnerProductId, Code from PartnersConnectorDB.connect.LitresDownloadCodes where OrderId is null),
AllCodes as (select p.ProductId, count(c.Code) as RemainingCount from ProductCatalogDB.prod.ProductsFromAllPartners p left join Codes c on p.PartnerProductId = c.PartnerProductId where p.PartnerId = 110 group by p.ProductId)
select p.PartnerProductId, p.Name as ProductName, c.RemainingCount from AllCodes c join ProductCatalogDB.prod.ProductsFromAllPartners p on c.ProductId = p.ProductId order by p.Name";

            var litresPartnerId = Configuration.LitresPartnerId;
            if (!litresPartnerId.HasValue)
            {
                throw new InvalidOperationException("ID партнера \"Литрес\" не задан в конфигурации");
            }

            return _context.Database.SqlQuery<LitresRemainingCodesCount>(QUERY, new SqlParameter("partnerId", litresPartnerId.Value));
        }
    }
}
