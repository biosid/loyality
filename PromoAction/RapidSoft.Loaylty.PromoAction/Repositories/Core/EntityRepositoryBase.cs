namespace RapidSoft.Loaylty.PromoAction.Repositories.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    /// <summary>
    /// ������� ���������� ���������� <see cref="IEntityRepositoryBase{TEntity,TKey}"/>
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public class EntityRepositoryBase<TEntity> : IEntityRepositoryBase<TEntity, long>
        where TEntity : class, IEntity<long>
    {
        /// <summary>
        /// �������� ������� � ��.
        /// </summary>
        protected MechanicsDbContext Context
        {
            get
            {
                return MechanicsDbContext.Get();
            }
        }

        /// <summary>
        /// ��������� �������� �� ����������� �������������
        /// </summary>
        /// <param name="id">
        /// ���������� �������������.
        /// </param>
        /// <returns>
        /// ���������� ��������.
        /// </returns>
        public TEntity Get(long id)
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
        /// ����� ��������� ��������� �������������� ���������
        /// </summary>
        /// <returns>
        /// ��������� �������������� ���������
        /// </returns>
        protected virtual DbSet<TEntity> GetEntityDbSet()
        {
            return this.Context.Set<TEntity>();
        }

        /// <summary>
        /// �������� �������� �� ��������� �� ����������� ��������������.
        /// </summary>
        /// <param name="entity">
        /// ��������� ��������.
        /// </param>
        protected void ExecuteDelete(TEntity entity)
        {
            this.GetEntityDbSet().Remove(entity);

            this.Commit();
        }

        protected void ExecuteSave(TEntity entity)
        {
            try
            {
                if (entity.Id == 0)
                {
                    this.GetEntityDbSet().Add(entity);
                }
                else
                {
                    this.Context.Entry(entity).State = EntityState.Modified;
                }

                this.Commit();
            }
            catch (Exception)
            {
                this.Context.Entry(entity).State = EntityState.Detached;
                throw;
            }
        }

        /// <summary>
        /// ����� ���������� ���������
        /// </summary>
        protected void Commit()
        {
            this.Context.SaveChanges();
        }

        /// <summary>
        /// ����������� �����, ���������� ����� ����������� �������� � ��, ��� ���������� ���������� ������ ��������, 
        /// ��������, ��������� ����������� �������������� ��� �������� ������������ �������, 
        /// ����� ����� �������� �� ����� ���� ��������� ���������� �� ��� ����������� <see cref="IValidatableObject"/>
        /// </summary>
        /// <param name="entity">
        /// ����������� ��������.
        /// </param>
        protected virtual void BeforeSave(TEntity entity)
        {
        }
    }
}