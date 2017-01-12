namespace RapidSoft.Loaylty.PromoAction.Repositories.Core
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// ������� �������� �������� ��������� �� ���������, ��� ��������� �� ������� ��������� ����.
    /// </summary>
    /// <typeparam name="TEntity">
    /// ��� ��������.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// ��� ����������� �������������� ��������.
    /// </typeparam>
    public interface IEntityRepositoryBase<TEntity, in TKey> where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// ��������� �������� �� ����������� �������������.
        /// </summary>
        /// <param name="id">
        /// ���������� �������������.
        /// </param>
        /// <returns>
        /// ���������� ��������.
        /// </returns>
        TEntity Get(TKey id);

        /// <summary>
        /// ��������� ��������� ���� ���������.
        /// </summary>
        /// <returns>
        /// ��������� ���������.
        /// </returns>
        IList<TEntity> GetAll();
    }
}