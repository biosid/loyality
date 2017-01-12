namespace RapidSoft.Loaylty.PromoAction.Repositories.Core 
{
    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// The EntityRepository interface.
    /// </summary>
    /// <typeparam name="TEntity">
    /// ��� ��������
    /// </typeparam>
    /// <typeparam name="TKey">
    /// ��� ����������� �������������� ��������
    /// </typeparam>
    public interface ITraceableEntityRepository<TEntity, in TKey> : IEntityRepositoryBase<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// ���������� �������� � ���������.
        /// </summary>
        /// <param name="entity">
        /// ����������� ��������.
        /// </param>
        void Save(TEntity entity);

        /// <summary>
        /// �������� �������� �� ��������� �� ����������� ��������������.
        /// </summary>
        /// <param name="id">
        /// ���������� �������������.
        /// </param>
        /// <param name="userId">
        /// ������������� ������������ ������������ ��������
        /// </param>
        void DeleteById(TKey id, string userId);
    }
}