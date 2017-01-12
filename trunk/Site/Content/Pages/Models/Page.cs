using System.ComponentModel.DataAnnotations;
using Vtb24.Site.Content.History.Models;
using Vtb24.Site.Content.Pages.Models.Configuration;

namespace Vtb24.Site.Content.Pages.Models
{
    public class Page : EntityWithHistory<Page, PageHistory, PageSnapshot>
    {
        [Required]
        public bool IsBuiltin { get; set; }

        [Required]
        public PageStatus Status { get; set; }

        [Required]
        public PageType Type { get; set; }

        public string ExternalId { get; set; }

        public static Page Map(BuiltinPageElement configPage)
        {
            return new Page
            {
                IsBuiltin = true,
                Status = PageStatus.Active,
                Type = PageType.Plain,
                History = new PageHistory
                {
                    CurrentVersion = new PageSnapshot
                    {
                        Data = new PageData
                        {
                            Url = configPage.Url,
                            Title = configPage.Title,
                            Keywords = configPage.Keywords,
                            Description = configPage.Description
                        }
                    }
                }
            };
        }
    }
}
