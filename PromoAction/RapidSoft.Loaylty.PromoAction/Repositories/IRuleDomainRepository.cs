namespace RapidSoft.Loaylty.PromoAction.Repositories
{
    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    public interface IRuleDomainRepository
    {
        /// <summary>
        /// ��������� ������ ������ �� ��������.
        /// </summary>
        /// <param name="name">
        /// �������� ������ ������.
        /// </param>
        /// <returns>
        /// ��������� ����� ������.
        /// </returns>
        RuleDomain GetByName(string name);
    }
}