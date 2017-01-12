namespace Vtb24.Site.Content.Pages.Models.Inputs
{
    public struct GetPlainPagesOptions
    {
        public bool? IsBuiltin { get; set; }

        public PageStatus? Status { get; set; }

        public bool LoadFullHistory { get; set; }

        public PagesSortOrder SortOrder { get; set; }
    }
}
