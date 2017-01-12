using System;
using Vtb24.Site.Content.Pages.Models;

namespace Vtb24.Arms.Site.Models.Pages
{
    public class PlainPageModel
    {
        public Guid Id { get; set; }

        public bool IsBuiltIn { get; set; }

        public PageStatus Status { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string Author { get; set; }

        public DateTime WhenModified { get; set; }

        public static PlainPageModel Map(Page page)
        {
            return new PlainPageModel
            {
                Id = page.Id,
                IsBuiltIn = page.IsBuiltin,
                Status = page.Status,
                Title = page.CurrentVersion.Data.Title,
                Url = page.CurrentVersion.Data.Url,
                Author = page.CurrentVersion.Author,
                WhenModified = page.CurrentVersion.When
            };
        }
    }
}
