using System;
using Vtb24.Site.Content.History.Models;
using Vtb24.Site.Content.Pages.Models;

namespace Vtb24.Arms.Site.Models.Pages
{
    public class PlainPageEditModel
    {
        public bool IsNewPage { get; set; }

        public bool IsBuiltin { get; set; }

        public Guid Id { get; set; }

        public Guid CurrentVersionId { get; set; }

        public Guid ThisVersionId { get; set; }

        public PageData Data { get; set; }

        public Snapshot[] Versions { get; set; }

        public string Query { get; set; }

        public PlainPagesQueryModel QueryModel
        {
            get
            {
                return string.IsNullOrWhiteSpace(Query)
                           ? new PlainPagesQueryModel()
                           : new PlainPagesQueryModel().MixQuery(Query);
            }
        }
    }
}
