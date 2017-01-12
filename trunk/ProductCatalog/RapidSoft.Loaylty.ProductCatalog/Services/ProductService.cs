namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using System.Configuration;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Interfaces;
    using Vtb24.Common.Configuration;

    public class ProductService : IProductService
    {
        private readonly int productPriceDropThresholdPercent;
        private readonly int productPriceRiseThresholdPercent;
        private readonly int productPriceDiscountPeriod;

        private readonly IProductsFixBasePriceDatesRepository productsFixBasePriceDatesRepository;

        public ProductService()
        {
            productPriceDropThresholdPercent = Convert.ToInt32(ConfigurationManager.AppSettings["ProductPriceDropThresholdPercent"]);
            productPriceRiseThresholdPercent = Convert.ToInt32(ConfigurationManager.AppSettings["ProductPriceRiseThresholdPercent"]);
            productPriceDiscountPeriod = Convert.ToInt32(ConfigurationManager.AppSettings["ProductPriceDiscountPeriod"]);

            productsFixBasePriceDatesRepository = new ProductsFixBasePriceDatesRepository();
        }

        public void CalculateProductBasePrice(Product oldProduct, Product newProduct, decimal newPrice, decimal? newBasePrice, bool copyBasePrice)
        {
            // если базовая цена приходит с импорта или АРМ, не считаем скидку по алгоритму и сбрасываем срок действия скидки
            if (newBasePrice.HasValue)
            {
                newProduct.BasePriceRUR = newBasePrice.Value > 0 ? newBasePrice : null;
                productsFixBasePriceDatesRepository.Reset(newProduct.ProductId);
                return;
            }

            // для новых товаров не считаем скидку
            if (oldProduct == null)
            {
                return;
            } 
            
            // если ранее проставленная через импорт или АРМ базовая цена была удалена, не забываем обновить продукт (https://jira.rapidsoft.ru/browse/VTBPLK-3214)
            if (oldProduct.BasePriceRUR.HasValue && !oldProduct.BasePriceRurDate.HasValue && !newBasePrice.HasValue)
            {
                newProduct.BasePriceRUR = null;
            }

            if (oldProduct.PriceRUR > newPrice && newPrice <= oldProduct.PriceRUR * this.productPriceDropThresholdPercent / 100)
            {
                // если новая цена упала ниже предела скидки от предыдущей цены, то фиксируем дату и время цены без скидки
                newProduct.BasePriceRUR = oldProduct.PriceRUR;
                productsFixBasePriceDatesRepository.Set(oldProduct.ProductId);
            }
            else if (oldProduct.BasePriceRUR.HasValue && newPrice >= oldProduct.BasePriceRUR.Value * this.productPriceRiseThresholdPercent / 100)
            {
                // если зафиксирована цена без скидки и новая цена превысили предел сброса скидки от нее, то сбрасываем дату и цену без скидки
                newProduct.BasePriceRUR = null;
                productsFixBasePriceDatesRepository.Reset(oldProduct.ProductId);
            }
            else if (copyBasePrice)
            {
                // если была автоматически зафиксированная цена без скидки, то сохраняем ее
                newProduct.BasePriceRUR = oldProduct.BasePriceRUR;
            }
        }

        public void CleanupProductBasePriceDate()
        {
            if (FeaturesConfiguration.Instance.Site505EnableActionPrice)
            {
                var until = DateTime.Now.AddDays(-productPriceDiscountPeriod);
                productsFixBasePriceDatesRepository.Cleanup(until);
            }
        }
    }
}
