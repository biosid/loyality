using System;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Site.Models.Catalog
{
    public class FilterModel
    {
        public const string VENDOR_NOT_SET = "Производитель не указан";

        public bool UsePostMethod { get; set; }

        public decimal PriceScaleMax { get; set; }

        public CatalogQueryModel Query { get; set; }

        public SelectListItem[] Vendors { get; set; }

        public CatalogParameterModel[] Parameters { get; set; }

        public bool HasExtendedFilters
        {
            get
            {
                return (Vendors != null && Vendors.Length > 0) ||
                       (Parameters != null && Parameters.Length > 0);
            }
        }

        public bool IsExpanded
        {
            get
            {
                return (Query.vendor != null && Query.vendor.Any(v => !string.IsNullOrEmpty(v))) ||
                       (Parameters != null && Parameters.Any(p => !string.IsNullOrEmpty(p.SelectedValue)));
            }
        }

        public static FilterModel Map(bool usePostMethod, CatalogQueryModel query, decimal maxPrice, string[] vendors = null, CatalogParameterModel[] parameters = null)
        {
            return new FilterModel
            {
                UsePostMethod = usePostMethod,
                Query = query,
                PriceScaleMax = Math.Ceiling(maxPrice),

                Vendors = vendors != null && vendors.Length == 1 && string.IsNullOrEmpty(vendors.First()) ? 
                            null :
                            vendors.MaybeSelect(v => string.IsNullOrEmpty(v)
                                                  ? new SelectListItem { Text = VENDOR_NOT_SET, Value = VENDOR_NOT_SET }
                                                  : new SelectListItem
                                                  {
                                                      Text = v,
                                                      Value = v,
                                                      Selected = query.vendor != null && query.vendor.Contains(v)
                                                  })
                                 .MaybeToArray(),

                Parameters = parameters
            };
        }
    }
}
