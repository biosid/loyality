namespace RapidSoft.Loaylty.PromoAction.Repositories.Core
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// ������� ���������� ���������� <see cref="IEntityRepositoryBase{TEntity,TKey}"/>
    /// </summary>
    /// <typeparam name="TEntity">
    /// ��� ��������
    /// </typeparam>
    public abstract class EntityRepository2Base<TEntity> : IEntityRepositoryBase<TEntity, string>
        where TEntity : class, IEntity<string>
    {
        /// <summary>
        /// ��������� �������� �� ����������� �������������
        /// </summary>
        /// <param name="id">
        /// ���������� �������������.
        /// </param>
        /// <returns>
        /// ���������� ��������.
        /// </returns>
        public TEntity Get(string id)
        {
            return this.GetEntityDbSet().SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// ��������� ��������� ���� ���������.
        /// </summary>
        /// <returns>
        /// ��������� ���������.
        /// </returns>
        public IList<TEntity> GetAll()
        {
            return this.GetEntityDbSet().ToList();
        }

        /// <summary>
        /// �������� ������� � ��.
        /// </summary>
        /// <returns>
        /// �������� ������� � ������.
        /// </returns>
        protected abstract DbContext GetContext();

        /// <summary>
        /// ����� ��������� ��������� �������������� ���������
        /// </summary>
        /// <returns>
        /// ��������� �������������� ���������
        /// </returns>
        protected virtual DbSet<TEntity> GetEntityDbSet()
        {
            return this.GetContext().Set<TEntity>();
        }
    }
}