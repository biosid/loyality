namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;

    using API.Entities;
    using API.OutputResults;

    using Extensions;

    using Interfaces;

    using Services;

    internal class PartnerRepository : BaseRepository, IPartnerRepository
    {
        public PartnerRepository()
            : base(DataSourceConfig.ConnectionString)
        {
        }

        public Partner[] GetAllPartners()
        {
            var partnerSettings = GetPartnerSettings();

            using (var ctx = this.DbNewContext())
            {
                var partners = ctx.Partners.Select(FillSettings(partnerSettings)).ToArray();
                return partners;
            }
        }

        public Partner GetById(int partnerId)
        {
            var partnerSettings = GetPartnerSettings();

            using (var ctx = this.DbNewContext())
            {
                var retVal = ctx.Partners.Select(FillSettings(partnerSettings)).SingleOrDefault(x => x.Id == partnerId);
                return retVal;
            }
        }

        public Partner[] GetByIds(IEnumerable<int> ids)
        {
            var partnerSettings = GetPartnerSettings();

            using (var ctx = this.DbNewContext())
            {
                var partners = ctx.Partners.Select(FillSettings(partnerSettings)).Where(t => ids.Contains(t.Id)).ToArray();
                return partners;
            }
        }

        public void DeleteSettings(string userId, int partnerId, string[] keys)
        {
            using (var ctx = this.DbNewContext(userId))
            {
                var settings = ctx.PartnerSettings.Where(x => x.PartnerId == partnerId).Where(x => keys.Contains(x.Key)).ToList();

                foreach (var setting in settings)
                {
                    ctx.PartnerSettings.Remove(setting);
                }

                ctx.SaveChanges();
            }
        }

        public void ReplaceSettings(string userId, int partnerId, Dictionary<string, string> settings)
        {
            using (var ctx = this.DbNewContext(userId))
            {
                InternalReplaceSettings(partnerId, settings, ctx);
            }
        }

        public List<PartnerSettings> GetSettings(int? partnerId)
        {
            using (var ctx = this.DbNewContext())
            {
                var query = ctx.PartnerSettings.AsQueryable();
                if (partnerId.HasValue)
                {
                    query = query.Where(x => x.PartnerId == partnerId);
                }

                var settings = query.ToList();
                return settings;
            }
        }

        public Dictionary<int, string> GetSettingsByKey(string key)
        {
            using (var ctx = this.DbNewContext())
            {
                return ctx.PartnerSettings
                          .Where(s => s.Key == key)
                          .ToDictionary(s => s.PartnerId, s => s.Value);
            }
        }

        public int[] FilterPartners(int[] partnerIds, Expression<Func<Partner, bool>> filter)
        {
            using (var ctx = this.DbNewContext())
            {
                return partnerIds != null && partnerIds.Length > 0
                           ? ctx.Partners
                                .Where(p => partnerIds.Contains(p.Id))
                                .Where(filter)
                                .Select(p => p.Id)
                                .ToArray()
                           : ctx.Partners
                                .Where(filter)
                                .Select(p => p.Id)
                                .ToArray();
            }
        }

        public Partner GetActivePartner(int partnerId)
        {
            Partner partner = GetById(partnerId);

            if (partner == null)
            {
                throw new InvalidOperationException(string.Format("Partner with id {0} not found", partnerId));
            }

            if (partner.Status != PartnerStatus.Active)
            {
                throw new InvalidOperationException(string.Format("Partner with id {0} not Active", partnerId));
            }

            return partner;
        }

        public Dictionary<string, string> GetSettings(int partnerId)
        {
            using (var ctx = this.DbNewContext())
            {
                var settings = ctx.PartnerSettings.Where(x => x.PartnerId == partnerId);
                var retVal = settings.ToDictionary(k => k.Key, v => v.Value);
                return retVal;
            }
        }

        public Partner CreateOrUpdate(string userId, Partner partner, Dictionary<string, string> settings = null)
        {
            using (var ctx = this.DbNewContext(userId))
            {
                if (partner.Id == default(int))
                {
                    var exists = GetByName(partner.Name);

                    if (exists != null)
                    {
                        var mess = string.Format("Партнер {0} уже существует", exists.Name);
                        throw new OperationException(ResultCodes.PARTNER_WITH_NAME_EXISTS, mess);
                    }

                    ctx.Entry(partner).State = EntityState.Added;
                }
                else
                {
                    partner.UpdatedUserId.ThrowIfNull("UpdatedUserId");
                    partner.UpdatedDate.ThrowIfNull("UpdatedDate");

                    ctx.Entry(partner).State = EntityState.Modified;
                }

                ctx.SaveChanges();

                if (settings != null)
                {
                    this.InternalReplaceSettings(partner.Id, settings, ctx);

                    partner.Settings = settings;
                }
                else
                {
                    partner.Settings = ctx.PartnerSettings.Where(x => x.PartnerId == partner.Id)
                                          .ToDictionary(k => k.Key, v => v.Value);
                }

                return partner;
            }
        }

        public void Delete(int id)
        {
            using (var ctx = this.DbNewContext())
            {
                foreach (var partnerSetting in ctx.PartnerSettings.Where(s => s.PartnerId == id))
                {
                    ctx.Entry(partnerSetting).State = EntityState.Deleted;
                }

                ctx.SaveChanges();

                Partner partner = ctx.Partners.FirstOrDefault(t => t.Id == id);
                ctx.Entry(partner).State = EntityState.Deleted;
                ctx.SaveChanges();
            }
        }

        public Partner GetByName(string name)
        {
            var partnerSettings = GetPartnerSettings();

            using (var ctx = this.DbNewContext())
            {
                return ctx.Partners.Select(FillSettings(partnerSettings)).FirstOrDefault(t => t.Name == name);
            }
        }

        public int[] FindAbsent(IEnumerable<int> ids)
        {
            using (var ctx = this.DbNewContext())
            {
                return ctx.Partners.Where(p => !ids.Contains(p.Id)).Select(p => p.Id).ToArray();
            }
        }

        public void SetSetting(int partnerId, string key, string value)
        {
            using (var ctx = this.DbNewContext())
            {
                var existed = ctx.PartnerSettings.SingleOrDefault(ps => ps.PartnerId == partnerId && ps.Key == key);

                if (existed == null)
                {
                    var settings = new PartnerSettings
                                   {
                                       PartnerId = partnerId,
                                       Key = key,
                                       Value = value
                                   };

                    ctx.PartnerSettings.Add(settings);

                    ctx.Entry(settings).State = EntityState.Added;
                }
                else
                {
                    existed.Value = value;
                    
                    ctx.Entry(existed).State = EntityState.Modified;
                }

                ctx.SaveChanges();
            }
        }

        private static Func<Partner, Partner> FillSettings(PartnerSettings[] partnerSettings)
        {
            return p =>
            {
                p.Settings = partnerSettings.Where(s => s.PartnerId == p.Id).ToDictionary(s => s.Key, s => s.Value);
                return p;
            };
        }

        private void InternalReplaceSettings(int partnerId, Dictionary<string, string> settings, LoyaltyDBEntities ctx)
        {
            var exists = ctx.PartnerSettings.Where(x => x.PartnerId == partnerId);

            foreach (var setting in exists)
            {
                ctx.PartnerSettings.Remove(setting);
            }

            var partnerSettings = settings.Select(
                x => new PartnerSettings
                {
                    PartnerId = partnerId,
                    Key = x.Key,
                    Value = x.Value
                });

            foreach (var setting in partnerSettings)
            {
                ctx.Entry(setting).State = EntityState.Added;
            }

            ctx.SaveChanges();
        }

        private PartnerSettings[] GetPartnerSettings()
        {
            using (var ctx = this.DbNewContext())
            {
                return ctx.PartnerSettings.ToArray();
            }
        }
    }
}