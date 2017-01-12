using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Site.Services.GiftShop.Catalog.Models;

namespace Vtb24.Site.Models.Catalog
{
    public class CatalogParameterModel
    {
        public string Name { get; set; }

        public string NameInUrl { get; set; }

        public string SelectedValue { get; set; }

        public SelectListItem[] Values { get; set; }

        public string Unit { get; set; }

        public static CatalogParameterModel Map(CatalogParameter parameter, NameValueCollection requestParams)
        {
            var nameInUrl = "param" + parameter.Definition.NameInUrl;

            var selectedValue = requestParams[nameInUrl];

            return new CatalogParameterModel
            {
                Name = parameter.Definition.Name,
                NameInUrl = nameInUrl,
                SelectedValue = selectedValue,
                Values = parameter.Values
                                  .Select(value => new SelectListItem
                                  {
                                      Text = string.IsNullOrEmpty(parameter.Definition.Unit)
                                                 ? value
                                                 : value + " " + parameter.Definition.Unit,
                                      Value = value,
                                      Selected = value == selectedValue
                                  })
                                  .ToArray(),
                Unit = parameter.Definition.Unit
            };
        }
    }
}
