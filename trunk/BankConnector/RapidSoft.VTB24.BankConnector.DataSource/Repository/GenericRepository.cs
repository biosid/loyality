using System.Linq;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource.Interface;

namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{
		internal BankConnectorDBContext Context;
		internal DbSet<TEntity> Entities;

        public GenericRepository(BankConnectorDBContext context)
		{
            Context = context;
			Entities = Context.Set<TEntity>();
		}

		public virtual IQueryable<TEntity> GetAll()
		{
			return Entities;
		}

        public virtual TEntity GetById(Expression<Func<TEntity, bool>> key)
		{
			return Entities.SingleOrDefault(key);
		}

		public virtual void Add(TEntity entity)
		{
			Entities.Add(entity);
		}

		public virtual void Delete(Expression<Func<TEntity, bool>> key)
		{
			var entityToDelete = Entities.Single(key);
			Delete(entityToDelete);
		}

		public virtual void Delete(TEntity entityToDelete)
		{
			if (Context.Entry(entityToDelete).State == EntityState.Detached)
			{
				Entities.Attach(entityToDelete);
			}

			Entities.Remove(entityToDelete);
		}

		public virtual void Update(TEntity entityToUpdate)
		{
            if (Context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                Entities.Attach(entityToUpdate);
            }

			Context.Entry(entityToUpdate).State = EntityState.Modified;
		}
	}
}
