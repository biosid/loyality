namespace RapidSoft.Loaylty.PromoAction.Service
{
    using System;
    using System.Collections.Generic;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Repositories.Core;

    /// <summary>
	/// Базовая реализация сервиса управления сущностью.
	/// </summary>
	/// <typeparam name="TEntity">
	/// Тип сущности.
	/// </typeparam>
	/// <typeparam name="TKey">
	/// Тип идентификатора сущности.
	/// </typeparam>
	/// <typeparam name="TRepository">
	/// Тип репозитория сущности.
	/// </typeparam>
	public class AdminEntityService<TEntity, TKey, TRepository>
		where TEntity : class, IEntity<TKey>
		where TRepository : class, ITraceableEntityRepository<TEntity, TKey>
	{
		/// <summary>
		/// Репозиторий для работы с сущностью.
		/// </summary>
		private TRepository repository;

		protected TRepository Repository
		{
			get
			{
				return this.repository ?? (this.repository = Activator.CreateInstance<TRepository>());
			}
		}

		public virtual TEntity Get(TKey id)
		{
			var retVal = this.Repository.Get(id);

			// var writer = new MemoryStream();
			// var ser = new DataContractSerializer(typeof(TEntity));
			// ser.WriteObject(writer, retVal);
			// writer.Close();
			return retVal;
		}

		public virtual IList<TEntity> GetAll()
		{
			return this.Repository.GetAll();
		}

		public virtual TEntity Save(TEntity entity)
		{
			entity.ThrowIfNull("entity");

			var fromDb = this.Repository.Get(entity.Id);

			if (fromDb != null)
			{
				this.CopyProperty(entity, fromDb);

				this.Repository.Save(fromDb);

				return fromDb;
			}

			this.Repository.Save(entity);

			return entity;
		}

		public virtual void DeleteById(TKey id, string userId)
		{
			this.Repository.DeleteById(id, userId);
		}

		protected virtual void CopyProperty(TEntity from, TEntity to)
		{
			var resettable = to as IResettable<TEntity>;

			if (resettable == null)
			{
				throw new ApplicationException(
					string.Format(
						"Сущность {0} не реализует интерфейс {1}, необходимо переопределить копирование свойств",
						typeof(TEntity).Name,
						typeof(IResettable<TEntity>).Name));
			}

			resettable.ResetFrom(from);
		}
	}
}