namespace RapidSoft.Loaylty.PromoAction.Repositories
{
    using System.Data;

    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;

    /// <summary>
    /// Конвертер статусов сущность EF в статусы версионинга.
    /// </summary>
    public static class EntityStateConvertor
    {
        /// <summary>
        /// Преобразовывает <see cref="EntityState"/> в <see cref="HistoryEvent"/>.
        /// </summary>
        /// <param name="entityState">
        /// Статус сущности.
        /// </param>
        /// <returns>
        /// Тип события.
        /// </returns>
        public static HistoryEvent ToHistoryEvent(this EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Modified:
                    {
                        return HistoryEvent.Update;
                    }

                case EntityState.Added:
                    {
                        return HistoryEvent.Create;
                    }

                case EntityState.Deleted:
                    {
                        return HistoryEvent.Delete;
                    }

                default:
                    {
                        return HistoryEvent.Unknow;
                    }
            }
        }
    }
}