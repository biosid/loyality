using System.Collections.Generic;

namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    public class ProductParamComparer : IEqualityComparer<ProductParam>
    {
        public bool Equals(ProductParam x, ProductParam y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return string.Equals(x.Name, y.Name) && string.Equals(x.Unit, y.Unit) && string.Equals(x.Value, y.Value);
        }

        public int GetHashCode(ProductParam obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            return (string.IsNullOrEmpty(obj.Name) ? 0 : obj.Name.GetHashCode())
                ^ (string.IsNullOrEmpty(obj.Unit) ? 0 : obj.Unit.GetHashCode())
                ^ (string.IsNullOrEmpty(obj.Value) ? 0 : obj.Value.GetHashCode()); 
        }
    }
}
