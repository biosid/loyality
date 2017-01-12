using System;

namespace Vtb24.Site.Content.Snapshots.Models
{
    public class Snapshot<T> where T : class
    {
        public string Id { get; set; }

        public T Entity { get; set; }

        public string Author { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
