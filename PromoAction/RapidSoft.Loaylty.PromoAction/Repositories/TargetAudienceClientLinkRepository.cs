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

    /// <summary>
    /// Репозиторий для работы с сущностью-связкой "Целевая аудитория"-"Клиент (Профиль клиента)" по идентифкаторам.
    /// </summary>
    public class TargetAudienceClientLinkRepository : ITargetAudienceClientLinkRepository
    {
        /// <summary>
        /// Контекст доступа к БД.
        /// </summary>
        protected MechanicsDbContext Context
        {
            get
            {
                return MechanicsDbContext.Get();
            }
        }

        /// <summary>
        /// Получение связки "Целевая аудитория"-"Клиент (Профиль клиента)" по идентифкаторам.
        /// </summary>
        /// <param name="targetAudienceId">
        /// Идентификатор целевой аудитории.
        /// </param>
        /// <param name="clientId">
        /// Идентификатор клиента.
        /// </param>
        /// <returns>
        /// Найденная связка "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </returns>
        public TargetAudienceClientLink Get(string targetAudienceId, string clientId)
        {
            return this.GetEntityDbSet().SingleOrDefault(x => x.TargetAudienceId == targetAudienceId && x.ClientId == clientId);
        }

        /// <summary>
        /// Получение коллекции всех связок "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </summary>
        /// <returns>
        /// Коллекция связок "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </returns>
        public IList<TargetAudienceClientLink> GetAll()
        {
            return this.GetEntityDbSet().ToList();
        }

        /// <summary>
        /// Сохранение связки "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </summary>
        /// <remarks>
        /// Выполняется добавление, обновление не выполяется!
        /// </remarks>
        /// <param name="targetAudienceClientLink">
        /// Связка "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </param>
        public void Insert(TargetAudienceClientLink targetAudienceClientLink)
        {
            targetAudienceClientLink.ThrowIfNull("targetAudienceClientLink");

            var now = DateTime.Now;
            var ctx = MechanicsDbContext.Get();

            using (var ts = new TransactionScope())
            {
                targetAudienceClientLink.CreateDateTime = now;
                targetAudienceClientLink.CreateDateTimeUtc = now.ToUniversalTime();

                try
                {
                    ctx.TargetAudienceClientLinks.Add(targetAudienceClientLink);
                    ctx.SaveChanges();
                }
                catch (Exception)
                {
                    ctx.Entry(targetAudienceClientLink).State = EntityState.Detached;
                    throw;
                }

                var userId = targetAudienceClientLink.CreateUserId;
                var historyEntity = targetAudienceClientLink.ToHistoryEntity(Api.Entities.History.HistoryEvent.Create, userId);
                try
                {
                    ctx.TargetAudienceClientLinkHistories.Add(historyEntity);
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

        /// <summary>
        /// Удаление связки "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </summary>
        /// <param name="targetAudienceId">
        /// Идентификатор целевой аудитории.
        /// </param>
        /// <param name="clientId">
        /// Идентификатор клиента.
        /// </param>
        /// <param name="userId">
        /// Пользователь выполняющий удаление.
        /// </param>
        public void DeleteById(string targetAudienceId, string clientId, string userId)
        {
            var ctx = MechanicsDbContext.Get();

            var targetAudience = ctx.TargetAudienceClientLinks.Find(targetAudienceId, clientId);

            var historyEntity = targetAudience.ToHistoryEntity(Api.Entities.History.HistoryEvent.Delete, userId);
            try
            {
                ctx.TargetAudienceClientLinks.Remove(targetAudience);
                ctx.TargetAudienceClientLinkHistories.Add(historyEntity);
                ctx.SaveChanges();
            }
            catch (Exception)
            {
                ctx.Entry(targetAudience).State = EntityState.Detached;
                ctx.Entry(historyEntity).State = EntityState.Detached;
                throw;
            }
        }

        public void DeleteSegment(string clientId, string userId)
        {
            var ctx = MechanicsDbContext.Get();

            var targetAudienceToDelete = ctx.TargetAudienceClientLinks.Join(ctx.TargetAudiences, tal => tal.TargetAudienceId, ta => ta.Id, (tal, ta) => new
            {
                tal, ta
            }).Where(t => t.ta.IsSegment && t.tal.ClientId == clientId).Select(t => t.tal).ToList();

            foreach (var targetAudience in targetAudienceToDelete)
            {
                var historyEntity = targetAudience.ToHistoryEntity(Api.Entities.History.HistoryEvent.Delete, userId);
                try
                {
                    ctx.TargetAudienceClientLinks.Remove(targetAudience);
                    ctx.TargetAudienceClientLinkHistories.Add(historyEntity);
                    ctx.SaveChanges();
                }
                catch (Exception)
                {
                    ctx.Entry(targetAudience).State = EntityState.Detached;
                    ctx.Entry(historyEntity).State = EntityState.Detached;
                    throw;
                }
            }
        }

        /// <summary>
        /// Метод сохранения изменений
        /// </summary>
        protected void Commit()
        {
            this.Context.SaveChanges();
        }

        /// <summary>
        /// Метод получения коллекции типизированных сущностей
        /// </summary>
        /// <returns>
        /// Коллекция типизированных сущностей
        /// </returns>
        protected virtual DbSet<TargetAudienceClientLink> GetEntityDbSet()
        {
            return this.Context.Set<TargetAudienceClientLink>();
        }
    }
}