using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Vtb24.Site.Services.GiftShop.Catalog.Models;
using Vtb24.Site.Services.GiftShop.Model;

namespace Vtb24.Site.Services.GiftShop.Stubs
{
    internal static class ProductGenerator
    {
        public static List<CatalogProduct> Generate(int count, List<CatalogCategory> categories)
        {
            if (categories == null)
                return null;

            var products = new List<CatalogProduct>();

            var id = 0;

            foreach (var cat in categories.Where(c => c.SubCategoriesCount == 0))
            {
                for (var i = 0; i < count; i++)
                {
                    var product = new CatalogProduct
                    {
                        Product = new Product
                        {
                            Id = (id++).ToString(CultureInfo.InvariantCulture),
                            CategoryId = cat.Id,
                            CategoryTitle = cat.Title,
                            Title = string.Format("Продукт {0}", id),
                            Thumbnail = null,
                            Vendor = "Rapidsoft",
                            VendorCode = null,
                            Price = 300,
                        },
                        ProductStatus = ProductStatus.Available,
                        Description = @"Товарищи! постоянное информационно-пропагандистское обеспечение нашей деятельности позволяет выполнять важные задания по разработке позиций, занимаемых участниками в отношении поставленных задач. Значимость этих проблем настолько очевидна, что сложившаяся структура организации обеспечивает широкому кругу (специалистов) участие в формировании систем массового участия. Таким образом реализация намеченных плановых заданий влечет за собой процесс внедрения и модернизации форм развития. Не следует, однако забывать, что начало повседневной работы по формированию позиции требуют от нас анализа форм развития. Значимость этих проблем настолько очевидна, что постоянный количественный рост и сфера нашей активности требуют определения и уточнения позиций, занимаемых участниками в отношении поставленных задач.",
                        Pictures = null,
                        Parameters = new[]
                        {
                            new CatalogProduct.Parameter {Name = "Цвет", Value = "Синий"}, 
                            new CatalogProduct.Parameter {Name = "Высота", Value = "10", Unit = "см"}
                        }
                    };
                    cat.Products.Add(product);

                    products.Add(product);
                }
            }
            // TODO: сделать расчет количества товаров в категории

            return products;
        }
    }
}
