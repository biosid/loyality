using System.Linq;
using System;
using System.Linq.Expressions;

namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        TEntity GetById(Expression<Func<TEntity, bool>> key);

        void Add(TEntity entity);

        void Delete(Expression<Func<TEntity, bool>> key);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);
    }
}
