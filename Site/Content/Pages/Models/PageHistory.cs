using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vtb24.Site.Content.History;
using Vtb24.Site.Content.History.Models;

namespace Vtb24.Site.Content.Pages.Models
{
    public class PageHistory : EntityHistory, IEntityHistory<PageSnapshot>
    {
        [Required]
        public PageSnapshot CurrentVersion { get; set; }

        public ICollection<PageSnapshot> Versions { get; set; }

        public PageHistory()
        {
            Versions = new HashSet<PageSnapshot>();
        }
    }
}
