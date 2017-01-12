namespace RapidSoft.Loaylty.ProductCatalog.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;

    using AutoMapper;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Entities;
    using RapidSoft.Loaylty.ProductCatalog.Import.ProductCorrectingFilter;
    using RapidSoft.Loaylty.ProductCatalog.Services;

    using Vtb24.Common.Configuration;
    using YML;
    using YML.Entities;

    public static class ProductImportSteps
    {
        private const int ProductsImportYmlBatchSize = 2000;
        private const int CategoriesImportBatchSize = 2000;

        private const string LoadStepId = "77124b40-8478-438f-b614-df6bb0a80233";
        private const string InitTableStepId = "77d9055e-b1af-4568-83d1-00b9c3124b9f";
        private const string ImportPartnerCategoriesStepId = "77e271a9-74c5-4d5d-8d9b-451a772a45ae";
        private const string ImportPartnerProductsStepId = "77823c3b-e0c0-473a-9d98-abab33d6e61f";
        private const string SwitchToNewCatalogStepId = "77a3f058-c537-4334-9863-18fd5579f16c";
        private const string UpdateProductsFromAllPartnersStepId = "E20FAD23-C0EE-4151-BCAB-8A8947906F05";

        private static readonly ProductCatalogsDataSource ProductCatalogsDataSource = new ProductCatalogsDataSource();
        private static readonly ProductCategoriesDataSource ProductCategoriesDataSource = new ProductCategoriesDataSource();
        private static readonly ProductsDataSource ProductsDataSource = new ProductsDataSource();
        private static readonly ProductCategoryRepository ProductCategoriesRepository = new ProductCategoryRepository();
        private static readonly ProductService ProductService = new ProductService();
        private static readonly ProductsFixBasePriceDatesRepository ProductsFixBasePriceDatesRepository = new ProductsFixBasePriceDatesRepository();

        static ProductImportSteps()
        {
            Mapper.CreateMap<OfferParam, ProductParam>();
            Mapper.CreateMap<GenericOffer, Product>()
                  .ForMember(p => p.Param, o => o.MapFrom(offer => offer.Params))
                  .ForMember(p => p.Pictures, o => o.MapFrom(offer => offer.Picture))
                  .ForMember(p => p.PartnerProductId, o => o.MapFrom(offer => offer.Id))
                  .ForMember(
                      p => p.Name,
                      o => o.ResolveUsing(
                          offer =>
                          {
                              var tempName = !string.IsNullOrWhiteSpace(offer.Name) ? offer.Name : offer.Title;
                              tempName = !string.IsNullOrWhiteSpace(tempName)
                                  ? tempName
                                  : string.Join(" ", offer.Vendor, offer.Model);
                              return tempName;
                          }));
        }

        public static void LoadFile(Logger logger, string source, string destination)
        {
            try
            {
                logger.LogStart(LoadStepId, "Загрузка файла: " + source);
                var webRequest = WebRequest.Create(source);
                webRequest.Method = "GET";

                using (var response = webRequest.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var fileStream = File.Create(destination))
                        {
                            var buffer = new byte[1024];
                            int bytesRead;
                            do
                            {
                                bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                                fileStream.Write(buffer, 0, bytesRead);
                            }
                            while (bytesRead > 0);
                        }
                    }
                }

                logger.LogEnd(LoadStepId, "Загрузка файла выполнена");
            }
            catch (Exception ex)
            {
                logger.LogError(LoadStepId, ex.ToString());
                throw;
            }
        }

        public static void InitTables(Logger logger, int partnerId, string dateKey)
        {
            try
            {
                logger.LogStart(InitTableStepId, "Создание таблиц");

                var intPartnerId = Convert.ToInt32(partnerId);

                logger.LogInfo(InitTableStepId, "Создание записи в PartnerProductCatalogs");

                string importKey = ProductCatalogsDataSource.InsertCatalog(intPartnerId, dateKey);

                logger.LogInfo(InitTableStepId, "Создание таблицы PartnerProductCaterories_*");

                ProductCategoriesDataSource.CreatePartnerProductCateroriesTable(importKey);

                logger.LogInfo(InitTableStepId, "Создание таблицы Products_*");

                ProductsDataSource.CreateProductsTable(importKey, partnerId);

                logger.LogEnd(InitTableStepId, "Таблицы созданы");
            }
            catch (Exception ex)
            {
                logger.LogError(InitTableStepId, ex.ToString());
                throw;
            }
        }

        public static void ImportPartnerCategories(Logger logger, int partnerId, string dateKey, string filePath)
        {
            try
            {
                logger.LogStart(ImportPartnerCategoriesStepId, "Загрузка партнерских категорий");

                var intPartnerId = Convert.ToInt32(partnerId);
                string importKey = LoyaltyDBSpecification.GetImportKey(intPartnerId, dateKey);
                int intCategoryBatchSize = Convert.ToInt32(CategoriesImportBatchSize);

                var ymlReader = new YmlReader(filePath);
                int index = 1;
                foreach (IEnumerable<Category> categoryBatch in ymlReader.ReadCategoriesBatches(intCategoryBatchSize))
                {
                    logger.LogInfo(ImportPartnerCategoriesStepId, "Загрузка пачки партнерских категорий " + index);

                    var partnerCategories = categoryBatch.Select(
                        x => new PartnerProductCategory
                             {
                                 PartnerId = intPartnerId,
                                 Id = x.Id,
                                 ParentId = x.ParentId == "0" ? null : x.ParentId,
                                 Name = x.Name,
                                 Status = ProductCategoryStatuses.Active
                             }).ToArray();

                    ProductCategoriesDataSource.InsertPartnerProductCategories(partnerCategories, importKey);

                    logger.LogInfo(ImportPartnerCategoriesStepId, "Загружено " + partnerCategories.Length + " партнерских категорий");

                    index++;
                }

                logger.LogInfo(ImportPartnerCategoriesStepId, "Расчет путей партнерских категорий");

                ProductCategoriesDataSource.RecalculatePartnerProductCategoriesPath(importKey);

                logger.LogEnd(ImportPartnerCategoriesStepId, "Партнерские категорий загружены");
            }
            catch (Exception ex)
            {
                logger.LogError(ImportPartnerCategoriesStepId, ex.ToString());
                throw;
            }
        }

        public static void ImportPartnerProducts(Logger logger, Partner partner, string dateKey, string filePath, ProductImportTask task)
        {
            try
            {
                logger.LogStart(ImportPartnerProductsStepId, "Загрузка продуктов");

                string importKey = LoyaltyDBSpecification.GetImportKey(partner.Id, dateKey);

                int intProductBatchSize = Convert.ToInt32(ProductsImportYmlBatchSize);

                // Загрузка ключа активного каталога
                PartnerProductCatalog activePartnerCatalog = ProductCatalogsDataSource.GetActiveCatalog(partner.Id);

                var activeCatalogKey = activePartnerCatalog != null ? activePartnerCatalog.Key : null;

                // Загрузка маппингов
                IList<CategoryMappingProjection> catMappings = ProductCategoriesRepository.GetCategoryMappings(partner.Id, importKey);
                
                var ymlReader = new YmlReader(filePath);
                int index = 1;
                var productImportCounters = new ProductImportCounters(logger);
                foreach (IEnumerable<GenericOffer> offersBatch in ymlReader.ReadOffersBatches(intProductBatchSize, true))
                {
                    logger.LogInfo(ImportPartnerProductsStepId, "Загрузка пачки предложений " + index);

                    var productsBatch =
                        offersBatch.Select(offer => MapOfferToProduct(partner.Id, offer)).ToArray();

                    logger.LogInfo(ImportPartnerProductsStepId, "Прочитано " + productsBatch.Length + " товаров из файла");

                    LoadProductChuck(
                        logger,
                        ImportPartnerProductsStepId,
                        partner,
                        productsBatch,
                        importKey,
                        activeCatalogKey,
                        catMappings,
                        task,
                        productImportCounters);

                    index++;
                }

                logger.LogEnd(ImportPartnerProductsStepId, "Продукты загружены");
            }
            catch (Exception ex)
            {
                logger.LogError(ImportPartnerProductsStepId, ex.ToString());
                throw;
            }
        }

        public static void SwitchToNewCatalog(Logger logger, int partnerId, string dateKey)
        {
            try
            {
                logger.LogStart(SwitchToNewCatalogStepId, "Переключение каталога");

                var intPartnerId = Convert.ToInt32(partnerId);
                var importKey = LoyaltyDBSpecification.GetImportKey(intPartnerId, dateKey);

                ProductsDataSource.AddHistoryTriggers(importKey);

                ProductsDataSource.RebuildIndexes(importKey);

                var oldCatalogKey = ProductCatalogsDataSource.GetActiveCatalog(intPartnerId);

                ProductCatalogsDataSource.SetActiveCatalog(intPartnerId, importKey);

                if (oldCatalogKey != null)
                {
                    ProductCatalogsDataSource.DeleteForeingKeyFromCatalog(oldCatalogKey);
                }

                logger.LogEnd(SwitchToNewCatalogStepId, "Переключение каталога выполнено");
            }
            catch (Exception ex)
            {
                logger.LogError(SwitchToNewCatalogStepId, ex.ToString());
                throw;
            }
        }

        public static void CreateConstraints(Logger logger, int partnerId, string dateKey)
        {
            try
            {
                var intPartnerId = Convert.ToInt32(partnerId);

                string importKey = LoyaltyDBSpecification.GetImportKey(intPartnerId, dateKey);

                logger.LogStart(InitTableStepId, "Создание констрейнта по ид партнёра на таблицу");
                
                ProductsDataSource.CreateProductsTableConstraints(importKey, partnerId);

                logger.LogEnd(InitTableStepId, "Констрнейнт создан");
            }
            catch (Exception ex)
            {
                logger.LogError(InitTableStepId, ex.ToString());
                throw;
            }
        }

        public static void UpdateProductsFromAllPartners(Logger logger, int partnerId, string dateKey)
        {
            try
            {
                logger.LogStart(UpdateProductsFromAllPartnersStepId, "Обновление данных ProductsFromAllPartners");

                ProductsDataSource.UpdateProductsFromAllPartners();

                logger.LogEnd(UpdateProductsFromAllPartnersStepId, "Обновление данных ProductsFromAllPartners");
            }
            catch (Exception ex)
            {
                logger.LogError(UpdateProductsFromAllPartnersStepId, ex.ToString());
                throw;
            }
        }

        private static Product MapOfferToProduct(int partnerId, GenericOffer offer)
        {
            var product = Mapper.Map<GenericOffer, Product>(offer);
            
            product.ProductId = LoyaltyDBSpecification.GetProductId(partnerId, offer.Id);

            product.PartnerId = partnerId;
            product.Status = ProductStatuses.Active;

            product.PriceRUR = offer.Price;
            product.PartnerCategoryId = offer.Categories.First();

            return product;
        }

        private static void LoadProductChuck(
            Logger logger,
            string stepId,
            Partner partner,
            Product[] newProductsChunk,
            string importKey,
            string activeCatalogKey,
            IList<CategoryMappingProjection> catMappings, 
            ProductImportTask task,
            ProductImportCounters counters)
        {
            var productImportTaskRepository = new ImportTaskRepository();

            var productIds = newProductsChunk.Select(p => p.PartnerProductId);

            // Загрузка продуктов из текущего каталога
            var existedProducts = activeCatalogKey == null
                ? new Product[0]
                : ProductsDataSource.GetProductsByPartnerProductIds(productIds, activeCatalogKey);

            // Получение списка ID продуктов с автоматически установленной ценой без скидки
            var productIdsWithBasePriceFixDate =
                FeaturesConfiguration.Instance.Site505EnableActionPrice
                    ? ProductsFixBasePriceDatesRepository.GetByProductIds(existedProducts.Select(p => p.ProductId).ToArray())
                    : null;

            // Построение стратегии определния статуса модерации продукта
            var moderStatusCalc = ModerationStatusCalculatorFactory.Build(partner, existedProducts);
            
            var insertList = new List<Product>(newProductsChunk.Length);
            var errorsCount = 0;

            var filters = new IProductCorrectingFilter[]
                              {
                                  new ParamsCorrectingFilter(task.ParamsProcessType),
                                  new WeightCorrectingFilter(task.WeightProcessType), 
                                  new CategoryCorrectingFilter(catMappings),
                                  new ParamsHideFilter()
                              };

            foreach (var itemProduct in newProductsChunk.AsParallel())
            {
                var product = itemProduct;
                var existedProduct = existedProducts.SingleOrDefault(p => p.PartnerProductId == product.PartnerProductId);

                var result = new Result(product);
                foreach (var productCorrectingFilter in filters)
                {
                    result = productCorrectingFilter.Execute(product);

                    if (!result.IsValid)
                    {
                        break;
                    }

                    product = result.Product;
                }

                if (!result.IsValid)
                {
                    logger.LogWarn(stepId, result.ErrorDescription);
                    errorsCount++;
                    continue;
                }

                product = result.Product;

                // Определение статуса продукта
                var moderStatus = moderStatusCalc.CalcModerationStatus(product);
                
                product.ModerationStatus = moderStatus;

                product.Status = existedProduct == null ? ProductStatuses.Active : existedProduct.Status;
                product.InsertedDate = existedProduct == null ? DateTime.Now : existedProduct.InsertedDate;
                product.IsRecommended = existedProduct != null && existedProduct.IsRecommended;

                if (FeaturesConfiguration.Instance.Site505EnableActionPrice)
                {
                    // если цена без скидки была зафиксирована автоматически, то ее нужно скопировать в новый продукт
                    var copyBasePrice = existedProduct != null &&
                                        productIdsWithBasePriceFixDate.Contains(existedProduct.ProductId);

                    ProductService.CalculateProductBasePrice(existedProduct, product, product.PriceRUR, product.BasePriceRUR, copyBasePrice);
                }

                insertList.Add(product);
            }

            ProductsDataSource.BulkInsertPartnerProducts(insertList, importKey);
            logger.LogInfo(stepId, "Успешно загружено " + insertList.Count);
            
            counters.AddSuccess(insertList.Count);

            logger.LogInfo(stepId, "Не загружено " + errorsCount);
            
            counters.AddErrors(errorsCount);

            task.CountSuccess += insertList.Count;
            task.CountFail += errorsCount;

            task = productImportTaskRepository.SaveProductImportTask(task);
        }

        private class ProductImportCounters
        {
            private readonly Logger logger;

            private long success;

            private long error;

            public ProductImportCounters(Logger logger)
            {
                this.logger = logger;
            }

            public void AddSuccess(int count)
            {
                success += count;
                logger.LogCounter("Продукты", "Успешно загружено", success);
            }

            public void AddErrors(int count)
            {
                error += count;
                logger.LogCounter("Продукты", "Не загружено", error);
            }
        }
    }
}