using System.Collections.Generic;
using System.Linq;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models;

namespace Vtb24.Arms.Catalog.Models.Shared
{
    public class CategoryItemModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Depth { get; set; }

        public bool IsPermissinsGranted { get; set; }

        public int? ParentId { get; set; }

        public CategoryItemModel Parent { get; set; }

        public CategoryItemModel[] Children { get; set; }

        public CategoriesLevelModel GetChildrenCategoriesModel(CategoriesLevelModel baseModel)
        {
            if (Children == null || Children.Length == 0)
                return null;

            return new CategoriesLevelModel
                       {
                           CategoryItems = Children,
                           ToActionUrl = baseModel.ToActionUrl
                       };
        }

        public static CategoryItemModel Map(Category category, HashSet<int> permissions)
        {
            return new CategoryItemModel
            {
                Id = category.Id,
                Title = category.Title,
                Depth = category.Depth,
                IsPermissinsGranted = permissions == null || permissions.Contains(category.Id),
                ParentId = category.ParentId
            };
        }

        public static CategoryItemModel[] Map(IEnumerable<Category> categories, HashSet<int> permissions)
        {
            var models = new List<CategoryItemModel>();
            var stack = new Stack<CategoryItemModel>();

            foreach (var model in categories.Select(c => Map(c, permissions)))
            {
                stack.Unwind(model.Depth);
                models.Add(model);
                stack.Push(model);
            }

            stack.Unwind(1);

            var root = new CategoryItemModel
            {
                Id = -1,
                Title = "Нет",
                Depth = 0,
                IsPermissinsGranted = true
            };
            stack.Reverse().ToArray().SetParent(root);

            return models.ToArray();
        }
    }

    internal static class CategoryItemModelMappingExtensions
    {
        public static void Unwind(this Stack<CategoryItemModel> stack, int depth)
        {
            while (stack.Count > 0 && stack.Peek().Depth > depth)
            {
                var children = stack.GetChildren().Reverse().ToArray();
                children.SetParent(stack.Peek());
            }
        }

        public static void SetParent(this CategoryItemModel[] children, CategoryItemModel parent)
        {
            foreach (var child in children)
                child.Parent = parent;
            parent.Children = children;
        }

        private static IEnumerable<CategoryItemModel> GetChildren(this Stack<CategoryItemModel> stack)
        {
            var depth = stack.Peek().Depth;

            do yield return stack.Pop();
            while (stack.Peek().Depth == depth);
        }
    }

}
