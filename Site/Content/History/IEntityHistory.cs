using System.Collections.Generic;
using Vtb24.Site.Content.History.Models;

namespace Vtb24.Site.Content.History
{
    public interface IEntityHistory<TSnapshot> where TSnapshot : Snapshot
    {
        TSnapshot CurrentVersion { get; set; }

        ICollection<TSnapshot> Versions { get; set; }
    }
}
