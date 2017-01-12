using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;

namespace Vtb24.Site.Content.History.Models
{
    public abstract class EntityWithHistory<TEntity, TEntityHistory, TSnapshot>
        where TSnapshot : Snapshot
        where TEntityHistory : EntityHistory, IEntityHistory<TSnapshot>
        where TEntity : EntityWithHistory<TEntity, TEntityHistory, TSnapshot>, new()
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public TEntityHistory History { get; set; }

        [NotMapped]
        public TSnapshot CurrentVersion
        {
            get { return History.CurrentVersion; }
        }

        public static TEntity Create(TSnapshot snapshot, DbContext context)
        {
            context.Set<TSnapshot>().Add(snapshot);
            context.SaveChanges();

            var historySet = context.Set<TEntityHistory>();

            var history = historySet.Create();

            history.Versions.Add(snapshot);
            history.CurrentVersion = snapshot;

            historySet.Add(history);

            context.SaveChanges();

            return new TEntity { History = history };
        }

        public static void Update(Guid id, TSnapshot snapshot, DbContext context)
        {
            var entity = context.Set<TEntity>().Find(id);
            if (entity == null)
                throw new ObjectNotFoundException();

            context.Entry(entity).Reference(e => e.History).Load();

            context.Set<TSnapshot>().Add(snapshot);

            context.SaveChanges();

            entity.History.Versions.Add(snapshot);
            entity.History.CurrentVersion = snapshot;

            context.SaveChanges();
        }
    }
}
