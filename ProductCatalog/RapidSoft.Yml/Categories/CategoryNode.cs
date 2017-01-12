
namespace RapidSoft.YML.Categories
{
    using System.Collections.Generic;
    using System.Linq;

    using Entities;

    public class CategoryNode
    {
        private readonly Category _category;
        private IEnumerable<CategoryNode> _branch;

        public CategoryNode(Category category)
        {
            _category = category;
        }

        public string Id
        {
            get
            {
                return Value.Id;  
            }
        }

        public Category Value
        {
            get
            {
                return _category;
            }
        }

        public IEnumerable<CategoryNode> Children
        {
            get;
            internal set;
        }

        public IEnumerable<CategoryNode> Descendants
        {
            get
            {
                return _branch ?? (_branch = Children.Concat(Children.SelectMany(c => c.Descendants)));
            }
        }

        public IEnumerable<CategoryNode> DescendantsAndSelf
        {
            get
            {
                yield return this;
                foreach (var descendant in Descendants)
                {
                    yield return descendant;
                }
            }
        }

        public IEnumerable<Category> Flatten()
        {
            return DescendantsAndSelf.Select(n => n.Value);
        }
    }
}