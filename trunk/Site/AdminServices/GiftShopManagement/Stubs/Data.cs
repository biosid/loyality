using System.Collections.Generic;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Stubs
{
    public static class Data
    {
        static Data()
        {
            Categories = new List<Category>();
            CategoryRoots = new List<Category>();
        }

        public static List<Category> Categories { get; private set; }

        public static List<Category> CategoryRoots { get; private set; }
    }
}