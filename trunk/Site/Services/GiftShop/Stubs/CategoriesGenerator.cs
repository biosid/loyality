using System.Collections.Generic;
using System.Linq;
using Vtb24.Site.Services.GiftShop.Catalog.Models;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Site.Services.GiftShop.Stubs
{
    internal static class CategoriesGenerator
    {
        private const string Data = @"Путешествия
Красота, здоровье, фитнес
Развлечения и образование
Техника и связь
    Мобильные телефоны
        Сотовые телефоны
        Смартфоны
    Компьютеры и ноутбуки
    Планшеты и книги
Подарки
    Цветы
    Книги, пресса
    Музыка
    Антиквариат
    Подарочные корзины
    Другие подарки
Дом и семья
Другое";

        public static List<CatalogCategory> Generate()
        {
            var categoryIndex = new List<CatalogCategory>();

            var id = 0;
            Data.ParsePlainTextList<CatalogCategory>((txt, parent) =>
            {
                // маппинг
                var cat = new CatalogCategory
                {
                    Id = ++id,
                    CategoryPath = txt,
                    Title = txt,

                    SubCategories = new List<CatalogCategory>(),
                    Products = new List<CatalogProduct>()
                };

                // добавляем в индекс
                categoryIndex.Add(cat);

                // сборка иерархии
                if (parent != null)
                {
                    parent.SubCategories.Add(cat);
                    parent.SubCategoriesCount++;
                    cat.ParentId = parent.Id;
                    cat.CategoryPath = parent.CategoryPath + "/" + cat.CategoryPath;
                }

                return cat;
            }).ToArray();

            return categoryIndex;
        }
    }
}
