namespace RapidSoft.Loaylty.PromoAction.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.Validation;
	using System.Linq;
	using System.Text;
	using System.Transactions;

	using RapidSoft.Loaylty.Logging;
	using RapidSoft.Loaylty.PromoAction.Api.Entities;
	using RapidSoft.Loaylty.PromoAction.Api.Entities.History;

	using Rule = RapidSoft.Loaylty.PromoAction.Api.Entities.Rule;

	/// <summary>
	/// Контекст доступа к данным на основне EF.
	/// </summary>
	public partial class MechanicsDbContext
	{
        private readonly ILog log = LogManager.GetLogger(typeof(MechanicsDbContext));

		/// <summary>
		/// Saves all changes made in this context to the underlying database.
		/// В дополнение к стандратному поведению проверяет сущности на необходимость ведения истории и сохраняет историю изменения.
		/// </summary>
		/// <returns>
		/// The number of objects written to the underlying database.
		/// </returns>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the context has been disposed.</exception>
		public override int SaveChanges()
		{
			var entities = this.ChangeTracker.Entries().ToList();

			this.ResetApproved(entities);

			var historyEntities = this.BuildHistoryEntities(entities).ToList();

			using (var ts = new TransactionScope())
			{
				int result;
				try
				{
					result = base.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					this.LogDbEntityValidationException(ex);
					throw;
				}
				catch (Exception ex)
				{
					log.Error("Ошибка сохранения объекта в БД: ", ex);
					throw;
				}

				if (historyEntities.Count > 0)
				{
					foreach (var historyEntity in historyEntities)
					{
						this.Set(historyEntity.GetType()).Add(historyEntity);
					}

					try
					{
						base.SaveChanges();
					}
					catch (DbEntityValidationException ex)
					{
						this.LogDbEntityValidationException(ex);
						throw;
					}
					catch (Exception ex)
					{
						log.Error("Ошибка сохранения объекта версионинга в БД: ", ex);
						throw;
					}
				}

				ts.Complete();

				return result;
			}
		}

		private void ResetApproved(IEnumerable<DbEntityEntry> entities)
		{
			foreach (var dbEntityEntry in entities)
			{
				if (dbEntityEntry.State == EntityState.Modified && dbEntityEntry.Entity is Rule)
				{
					var rule = dbEntityEntry.Entity as Rule;

					if (rule.IsBase())
					{
						// NOTE: Базовое правило всегда подтверждено.
						continue;
					}

					var orginal = dbEntityEntry.OriginalValues;

					if (this.MustResetApproved(rule, orginal))
					{
						rule.Approved = ApproveStatus.NotApproved;
						rule.ApproveDescription = null;
					}
				}
			}
		}

		private bool MustResetApproved(Rule rule, DbPropertyValues orginal)
		{
			return !string.Equals(rule.Predicate, orginal["Predicate"])
				   || !rule.Factor.Equals(orginal["Factor"])
				   || !string.Equals(rule.ConditionalFactors, orginal["ConditionalFactors"])
				   || !rule.Priority.Equals(orginal["Priority"]) 
                   || !rule.DateTimeFrom.Equals(orginal["DateTimeFrom"])
				   || !rule.DateTimeTo.Equals(orginal["DateTimeTo"])
				   || !rule.IsBase().Equals(((RuleTypes)orginal["Type"]).IsBase());
		}

		/// <summary>
		/// Записывает в лог развернутое сообщение об ошибке валидации сущности.
		/// </summary>
		/// <param name="exception">
		/// Ошибка валидании сущности.
		/// </param>
		private void LogDbEntityValidationException(DbEntityValidationException exception)
		{
			var sb = new StringBuilder();
			sb.Append(Environment.NewLine);
			foreach (var entityValidationResult in exception.EntityValidationErrors)
			{
				sb.Append("\t").AppendLine(entityValidationResult.Entry.Entity.GetType().Name);
				foreach (var validationError in entityValidationResult.ValidationErrors)
				{
					sb.Append("\t\t").Append(string.IsNullOrWhiteSpace(validationError.PropertyName) ? "-" : validationError.PropertyName)
						.Append(" : ").AppendLine(validationError.ErrorMessage);
				}
			}

			log.Error("Ошибка валидации сущностей: " + sb, exception);
		}

		/// <summary>
		/// Создает коллекцию исторических сущностей.
		/// </summary>
		/// <param name="entityEntries">
		/// The entity entries.
		/// </param>
		/// <returns>
		/// Коллекция исторических сущностей.
		/// </returns>
		private IEnumerable<object> BuildHistoryEntities(IEnumerable<DbEntityEntry> entityEntries)
		{
// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var entityEntry in entityEntries)
// ReSharper restore LoopCanBeConvertedToQuery
			{
				var historyEntity = this.BuildHistoryEntity(entityEntry);
				if (historyEntity != null)
				{
					yield return historyEntity;
				}
			}
		}

		/// <summary>
		/// При необходимости вызывает создание исторической сущности и возвращает ее, иначе <c>null</c>.
		/// </summary>
		/// <param name="entityEntry">
		/// The db entity entry.
		/// </param>
		/// <returns>
		/// Созданная историческая сущность.
		/// </returns>
		private object BuildHistoryEntity(DbEntityEntry entityEntry)
		{
			if (entityEntry.State == EntityState.Detached || entityEntry.State == EntityState.Unchanged)
			{
				return null;
			}

			var traceable = entityEntry.Entity as IHistoryTraceable;

			return traceable != null
					   ? traceable.ToHistoryEntity(entityEntry.State.ToHistoryEvent())
					   : null;
		}
	}
}