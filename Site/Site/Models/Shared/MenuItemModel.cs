using System.Collections.Generic;

namespace Vtb24.Site.Models.Shared
{
    public class MenuItemModel
    {
        public MenuItemModel()
        {
            Children = new List<MenuItemModel>();
        }

        public string Id { get; set; }

        public string Text { get; set; }
        
        public string Url { get; set; }

        public bool IsActive { get; set; }

        public bool IsRoot { get; set; }

        public int? BadgeNumber { get; set; }

        public string BadgeName { get; set; }

        public bool HasActiveChild { get; set; }

        public int Depth { get; set; }

        public List<MenuItemModel> Children { get; set; }

        public MenuItemModel Parent { get; set; }
    }
}