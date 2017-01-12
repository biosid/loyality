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
    /// Базовая реализация интерфейса <see cref="IEntityRepositoryBase{TEntity,TKey}"/>
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public class EntityRepositoryBase<TEntity> : IEntityRepositoryBase<TEntity, long>
        where TEntity : class, IEntity<long>
    {
        /// <summary>
        /// Контекст доступа к БД.
        /// </summary>
        protected MechanicsDbContext Context
        {
            get
            {
                return MechanicsDbContext.Get();
            }
        }

        /// <summary>
        /// Получение сущности по уникальному идентифкатору
        /// </summary>
        /// <param name="id">
        /// Уникальный идентификатор.
        /// </param>
        /// <returns>
        /// Полученная сущность.
        /// </returns>
        public TEntity Get(long id)
        {
            return this.GetEntityDbSet().SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Получение коллекции всех сущностей.
        /// </summary>
        /// <returns>
        /// Коллекция сущностей.
        /// </returns>
        public IList<TEntity> GetAll()
        {
            return this.GetEntityDbSet().ToList();
        }

        /// <summary>
        /// Метод получения коллекции типизированных сущностей
        /// </summary>
        /// <returns>
        /// Коллекция типизированных сущностей
        /// </returns>
        protected virtual DbSet<TEntity> GetEntityDbSet()
        {
            return this.Context.Set<TEntity>();
        }

        /// <summary>
        /// Удаление сущности из хранилища по уникальному идентификатору.
        /// </summary>
        /// <param name="entity">
        /// Удаляемая сущность.
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
        /// Метод сохранения изменений
        /// </summary>
        protected void Commit()
        {
            this.Context.SaveChanges();
        }

        /// <summary>
        /// Виртуальный метод, вызываемый перед сохранением сущности в БД, для реализации уникальной логики сущности, 
        /// например, генерация уникального идентификатора или проверка корректности правила, 
        /// когда такая проверка не может быть выполнена средствами БД или реализацией <see cref="IValidatableObject"/>
        /// </summary>
        /// <param name="entity">
        /// Сохраняемая сущность.
        /// </param>
        protected virtual void BeforeSave(TEntity entity)
        {
        }
    }
}