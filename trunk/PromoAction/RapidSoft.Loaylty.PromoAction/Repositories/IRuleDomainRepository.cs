namespace RapidSoft.Loaylty.PromoAction.Repositories
{
    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    public interface IRuleDomainRepository
    {
        /// <summary>
        /// Получение домена правил по названию.
        /// </summary>
        /// <param name="name">
        /// Название домена правил.
        /// </param>
        /// <returns>
        /// Найденный домен правил.
        /// </returns>
        RuleDomain GetByName(string name);
    }
}