namespace RapidSoft.Loaylty.PromoAction.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Transactions;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;
    using RapidSoft.Loaylty.PromoAction.Repositories.Core;

    /// <summary>
    /// Репоизиторий целевых аудиторий.
    /// </summary>
    public class TargetAudienceRepository : EntityRepository2Base<TargetAudience>, ITargetAudienceRepository
    {
        /// <summary>
        /// Сохранение целевой аудитории.
        /// </summary>
        /// <param name="targetAudience">
        /// Целевая аудитория.
        /// </param>
        public void Save(TargetAudience targetAudience)
        {
            targetAudience.ThrowIfNull("targetAudience");

            if (this.Get(targetAudience.Id) == null)
            {
                this.Insert(targetAudience);
            }
            else
            {
                this.Update(targetAudience);
            }
        }

        /// <summary>
        /// Удаление целевой аудитории.
        /// </summary>
        /// <param name="id">
        /// Идентификатор целевой аудитории.
        /// </param>
        /// <param name="userId">
        /// Пользователь выполняющий удаление.
        /// </param>
        public void DeleteById(string id, string userId)
        {
            var ctx = MechanicsDbContext.Get();

            var targetAudience = ctx.TargetAudiences.Find(id);

            var historyEntity = targetAudience.ToHistoryEntity(HistoryEvent.Delete, userId);
            try
            {
                ctx.TargetAudiences.Remove(targetAudience);
                ctx.TargetAudienceHistories.Add(historyEntity);
                ctx.SaveChanges();
            }
            catch (Exception)
            {
                ctx.Entry(targetAudience).State = EntityState.Detached;
                ctx.Entry(historyEntity).State = EntityState.Detached;
                throw;
            }
        }

        /// <summary>
        /// Возвращает набор целевых аудиторий в которые входит клиент.
        /// </summary>
        /// <param name="clientId">
        /// Уникальный идентификатор клиента.
        /// </param>
        /// <returns>
        /// Набор целевых аудиторий в которые входит клиент.
        /// </returns>
        public IList<TargetAudience> GetByClientId(string clientId)
        {
            var retVal =
                this.GetContext()
                    .Set<TargetAudienceClientLink>()
                    .Where(x => x.ClientId == clientId)
                    .Select(x => x.TargetAudience)
                    .Distinct();
            return retVal.ToList();
        }

        public IList<TargetAudience> GetBySegment(bool? isSegment)
        {
            IQueryable<TargetAudience> query = this.GetContext().Set<TargetAudience>();

            if (isSegment.HasValue)
            {
                var inner = isSegment.Value;
                query = query.Where(x => x.IsSegment == inner);
            }

            var retVal = query.ToList();

            return retVal;
        }

        /// <summary>
        /// Check if exists by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(string id)
        {
            return GetContext().Set<TargetAudience>().Any(t => t.Id == id);
        }

        /// <summary>
        /// Контекст доступа к БД.
        /// </summary>
        /// <returns>
        /// Контекст доступа к данным.
        /// </returns>
        protected override DbContext GetContext()
        {
            return MechanicsDbContext.Get();
        }

        /// <summary>
        /// Изменяет целевую аудиторию в хранилище.
        /// </summary>
        /// <param name="targetAudience">
        /// Целевая аудитория.
        /// </param>
        private void Update(TargetAudience targetAudience)
        {
            var now = DateTime.Now;
            var ctx = MechanicsDbContext.Get();
            targetAudience.UpdateDateTime = now;
            targetAudience.UpdateDateTimeUtc = now.ToUniversalTime();

            var userId = targetAudience.UpdateUserId;
            var historyEntity = targetAudience.ToHistoryEntity(HistoryEvent.Update, userId);

            try
            {
                ctx.TargetAudiences.Attach(targetAudience);
                ctx.Entry(targetAudience).State = EntityState.Modified;
                ctx.TargetAudienceHistories.Add(historyEntity);
                ctx.SaveChanges();
            }
            catch (Exception)
            {
                ctx.Entry(targetAudience).State = EntityState.Detached;
                ctx.Entry(historyEntity).State = EntityState.Detached;
                throw;
            }
        }

        /// <summary>
        /// Добавляет целевую аудиторию в хранилище.
        /// </summary>
        /// <param name="targetAudience">
        /// Целевая аудитория.
        /// </param>
        private void Insert(TargetAudience targetAudience)
        {
            var now = DateTime.Now;
            var ctx = MechanicsDbContext.Get();

            using (var ts = new TransactionScope())
            {
                targetAudience.CreateDateTime = now;
                targetAudience.CreateDateTimeUtc = now.ToUniversalTime();

                try
                {
                    ctx.TargetAudiences.Add(targetAudience);
                    ctx.SaveChanges();
                }
                catch (Exception)
                {
                    ctx.Entry(targetAudience).State = EntityState.Detached;
                    throw;
                }

                var userId = targetAudience.CreateUserId;
                var historyEntity = targetAudience.ToHistoryEntity(HistoryEvent.Create, userId);
                try
                {
                    ctx.TargetAudienceHistories.Add(historyEntity);
                    ctx.SaveChanges();
                }
                catch (Exception)
                {
                    ctx.Entry(historyEntity).State = EntityState.Detached;
                    throw;
                }

                ts.Complete();
            }
        }
    }
}
