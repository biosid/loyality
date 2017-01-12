namespace RapidSoft.Loaylty.PromoAction.Repositories.Core
{
    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// ������� ���������� ���������� <see cref="IEntityRepository{TEntity,TKey}"/>
    /// </summary>
    /// <typeparam name="TEntity">
    /// ��� ��������.
    /// </typeparam>
    public abstract class EntityRepository<TEntity> : EntityRepositoryBase<TEntity>, IEntityRepository<TEntity, long>
        where TEntity : class, IEntity<long>
    {
        /// <summary>
        /// ���������� �������� � ���������.
        /// </summary>
        /// <param name="entity">
        /// ����������� ��������.
        /// </param>
        public void Save(TEntity entity)
        {
            this.BeforeSave(entity);

            entity.ThrowIfNull("entity");

            this.ExecuteSave(entity);
        }

        /// <summary>
        /// �������� �������� �� ��������� �� ����������� ��������������.
        /// </summary>
        /// <param name="id">
        /// ���������� �������������.
        /// </param>
        public void DeleteById(long id)
        {
            var entity = this.GetEntityDbSet().Find(id);

            this.ExecuteDelete(entity);
        }
    }
}