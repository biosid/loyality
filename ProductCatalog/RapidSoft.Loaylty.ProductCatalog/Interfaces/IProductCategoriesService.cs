using System;

using RapidSoft.Loaylty.ProductCatalog.Entities;

namespace RapidSoft.Loaylty.Interfaces
{
    //Подсистема управления каталогом товаров
    //Ведение справочника «Категории товаров»
    public interface IProductCategoriesService
    {
        /// <summary>
        /// Добавление категории;
        /// </summary>
        Guid AddProductCategory(ProductCategory category);

        /// <summary>
        /// Редактирование категории;
        /// </summary>
        void EditProductCategory(Guid categoryId, ProductCategory category);

        /// <summary>
        /// Удаление категории;
        /// </summary>
        void DeleteProductCategory(Guid categoryId);

        /// <summary>
        //  Просмотр списка подкатегорий категории
        /// </summary>
        void GetProductCategories(int? parentCategoryId);

        /// <summary>
        /// Перенесение категории из одной ветки дерева категорий в другую
        /// </summary>
        void MoveProductCategory(int fromParentCategoryId, int toParentCategoryId);

        /// <summary>
        /// Слияние двух категорий
        /// </summary>
        /// <param name="firstCategoryId"></param>
        /// <param name="secondCategoryId"></param>
        void MergeProductCategories(int firstCategoryId, int secondCategoryId);

        /// <summary>
        ///	Добавление товара в категорию
        /// Изменение категории, к которой отнесен товар
        /// </summary>
        void AddProductToCategory();

        /// <summary>
        /// Удаление товара из категории
        ///	Изменение категории, к которой отнесен товар
        /// </summary>
        void DeleteProductFromCategory();

        /// <summary>
        /// Настройку доступности категорий товаров для разных типов клиентов (VIP, стандартных)
        /// Включение/отключение для каждой категории общего дерева возможности публиковать (импортировать) партнером товары в эту категорию
        /// </summary>
        void SetProductCategoryPermission(int categoryId, UserStatuses userStatus);

        /// <summary>
        /// быстрый поиск по имени
        /// </summary>
        void SearchProductCategoryQuick(string productCategoryName);
    }
}