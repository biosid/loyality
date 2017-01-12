namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using API.Entities;

    public interface IPartnerRepository
    {
        Partner[] GetAllPartners();

        Partner CreateOrUpdate(string userId, Partner partner, Dictionary<string, string> settings = null);

        void Delete(int id);

        Partner GetByName(string name);

        Partner GetById(int id);

        Partner[] GetByIds(IEnumerable<int> ids);

        void DeleteSettings(string userId, int partnerId, string[] keys);

        void ReplaceSettings(string userId, int partnerId, Dictionary<string, string> settings);

        List<PartnerSettings> GetSettings(int? partnerId);

        Dictionary<int, string> GetSettingsByKey(string key);

        int[] FilterPartners(int[] partnerIds, Expression<Func<Partner, bool>> filter);

        Partner GetActivePartner(int partnerId);
    }
}
