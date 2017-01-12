namespace RapidSoft.Loaylty.PromoAction.Repositories.Core 
{
    using System;

    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// ������� ���������� ���������� <see cref="ITraceableEntityRepository{TEntity,TKey}"/>
    /// </summary>
    /// <typeparam name="TEntity">
    /// ��� ��������.
    /// </typeparam>
    public abstract class TraceableEntityRepository<TEntity> : EntityRepositoryBase<TEntity>, ITraceableEntityRepository<TEntity, long> where TEntity : class, ITraceableEntity<long>, IEntity<long>
    {
        /// <summary>
        /// ���������� �������� � ���������.
        /// </summary>
        /// <param name="entity">
        /// ����������� ��������.
        /// </param>
        public virtual void Save(TEntity entity)
        {
            this.BeforeSave(entity);

            entity.ThrowIfNull("entity");

            entity.InsertedDate = DateTime.Now;

            this.ExecuteSave(entity);
        }

        /// <summary>
        /// �������� �������� �� ��������� �� ����������� ��������������.
        /// </summary>
        /// <param name="id">
        /// ���������� �������������.
        /// </param>
        /// <param name="userId">
        /// ������������� ������������ ������������ ��������
        /// </param>
        public virtual void DeleteById(long id, string userId)
        {
            var entity = this.GetEntityDbSet().Find(id);

            if (entity == null)
            {
                return;
            }

            entity.InsertedDate = DateTime.Now;
            entity.UpdatedUserId = userId;

            this.ExecuteDelete(entity);
        }
    }
}