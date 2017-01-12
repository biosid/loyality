using System;
using Vtb24.Site.Content.Pages.Models;

namespace Vtb24.Arms.Site.Models.Pages
{
    public class ChangePlainPageStatusModel
    {
        public Guid[] Ids { get; set; }

        public PageStatus Status { get; set; }
    }
}
