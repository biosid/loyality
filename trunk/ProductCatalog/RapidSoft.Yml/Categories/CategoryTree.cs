using System.Collections.Generic;
using System.Linq;

namespace RapidSoft.YML.Categories
{
    using Entities;
    
    public class CategoryTree
    {
        private readonly CategoryNode[] _roots;
        private readonly CategoryNode[] _descendants;

        internal CategoryTree(IEnumerable<Category> searchList)
        {
            var roots = searchList.Where(c => c.ParentId == "0");
            var catGroups = searchList.GroupBy(c => c.ParentId);
            var descendants = new List<CategoryNode>();

            _roots = roots.Select(r => RecursiveParse(r, descendants, catGroups)).ToArray();
            _descendants = descendants.ToArray();
        }

        public IEnumerable<CategoryNode> RootNodes
        {
            get
            {
                return _roots;
            }
        }

        public IEnumerable<CategoryNode> Descendants
        {
            get
            {
                return _descendants;
            }
        }

        public IEnumerable<Category> Flatten(params string[] categories)
        {

            return
                Descendants
                    .Where(n => categories.Contains(n.Id))
                    .SelectMany(n => n.Flatten())
                    .Distinct();
        }

        private static CategoryNode RecursiveParse(Category cat, ICollection<CategoryNode> accumulator, IEnumerable<IGrouping<string, Category>> groups)
        {
            var node = new CategoryNode(cat);
            var children = groups.SingleOrDefault(g => g.Key == cat.Id);
            node.Children = children != null ? children.Select(c => RecursiveParse(c, accumulator, groups)) : new List<CategoryNode>();
            accumulator.Add(node);

            return node;
        }
    }
}