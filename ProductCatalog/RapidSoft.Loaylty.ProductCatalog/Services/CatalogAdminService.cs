namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Extensions;

    using Monitoring;

    using RapidSoft.Etl.LogSender;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.Logging.Wcf;
    using RapidSoft.Loaylty.ProductCatalog.API;
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.QuarzJobs;
    using RapidSoft.Loaylty.ProductCatalog.Settings;

    using VTB24.ArmSecurity.Interfaces;

    using ArmPermissions = RapidSoft.VTB24.ArmSecurity.ArmPermissions;
    using PartnerProductCategoryLink = RapidSoft.Loaylty.ProductCatalog.API.Entities.PartnerProductCategoryLink;

    [LoggingBehavior]
    public class CatalogAdminService : SupportService, ICatalogAdminService
    {
        private readonly ILogEmailSender logEmailSender;
        private readonly ICategoriesSearcher categoriesSearcher;
        private readonly IPartnerProductCateroryLinkRepository partnerProductCateroryLinkRepository;
        private readonly IProductCategoriesDataSource productCategoriesDataSource;
        private readonly IProductCategoriesRepository productCategoriesRepository;
        private readonly IPartnerRepository partnerRepository;
        private readonly IProductsSearcher productsSearcher;
        private readonly ICategoryPermissionRepository categoryPermissionRepository;
        private readonly IImportTaskRepository importTaskRepository;
        private readonly IOrdersDataSource ordersDataSource;
        private readonly IOrdersHistoryRepository ordersHistoryRepository;
        private readonly IOrdersRepository ordersRepository;
        private readonly IDeliveryRatesRepository deliveryRatesRepository;
        private readonly IGeoPointProvider geoPointProvider;
        private readonly IProductViewsByDayRepository productViewsByDayRepository;

        private readonly ProductsDataSource productsDataSource;
        private readonly ProductsRepository productsRepository;

        private readonly ILog log = LogManager.GetLogger(typeof(CatalogAdminService));
        private readonly ISecurityChecker securityChecker;

        private readonly OrdersStatusChanger ordersStatusChanger;

        public CatalogAdminService(
            ICategoriesSearcher categoriesSearcher = null,
            IProductsSearcher productsSearcher = null,
            IProductCategoriesDataSource productCategoriesDataSource = null,
            IPartnerProductCateroryLinkRepository partnerProductCateroryLinkRepository = null,
            ICategoryPermissionRepository categoryPermissionRepository = null,
            IPartnerRepository partnerRepository = null,
            IImportTaskRepository importTaskRepository = null,
            IOrdersHistoryRepository ordersHistoryRepository = null,
            IOrdersRepository ordersRepository = null,
            IDeliveryRatesRepository deliveryRatesRepository = null,
            IGeoPointProvider geoPointProvider = null,
            IBonusGatewayProvider bonusGatewayProvider = null,
            IAdvancePaymentProvider advancePaymentProvider = null,
            ISecurityChecker securityChecker = null,
            IProductCategoriesRepository productCategoriesRepository = null,
            ILogEmailSender logEmailSender = null,
            IProductViewsByDayRepository productViewsByDayRepository = null)
        {
            this.logEmailSender = logEmailSender ?? new LogEmailSender();
            this.productCategoriesRepository = productCategoriesRepository ?? new ProductCategoryRepository();
            this.ordersRepository = ordersRepository ?? new OrdersRepository();
            this.categoriesSearcher = categoriesSearcher ?? new CategoriesSearcher();
            this.productsSearcher = productsSearcher ?? new ProductsSearcher();
            this.productCategoriesDataSource = productCategoriesDataSource ?? new ProductCategoriesDataSource();
            this.partnerProductCateroryLinkRepository = partnerProductCateroryLinkRepository ?? new PartnerProductCateroryLinkRepository();
            this.categoryPermissionRepository = categoryPermissionRepository ?? new CategoryPermissionRepository();
            this.partnerRepository = partnerRepository ?? new PartnerRepository();
            this.importTaskRepository = importTaskRepository ?? new ImportTaskRepository();
            this.productsDataSource = new ProductsDataSource();
            this.ordersDataSource = new OrdersDataSource();
            this.productsRepository = new ProductsRepository();
            this.ordersHistoryRepository = ordersHistoryRepository ?? new OrdersHistoryRepository();
            this.deliveryRatesRepository = deliveryRatesRepository ?? new DeliveryRatesRepository();
            this.geoPointProvider = geoPointProvider ?? new GeoPointProvider();
            this.productViewsByDayRepository = productViewsByDayRepository ?? new ProductViewsByDayRepository();
            this.securityChecker = securityChecker ?? new ArmSecurityChecker();

            ordersStatusChanger = new OrdersStatusChanger(
                this.ordersDataSource,
                this.ordersRepository,
                bonusGatewayProvider ?? new BonusGatewayProvider(),
                advancePaymentProvider ?? new AdvancePaymentProvider());
        }

        public CatalogAdminService() : this(null)
        {
        }

        public ImportProductsFromYmlResult ImportProductsFromYmlHttp(ImportProductsFromYmlHttpParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.UserId.ThrowIfNull("parameters.UserId");

                securityChecker.CheckPermissions(
                    parameters.UserId,
                    parameters.PartnerId,
                    ArmPermissions.Products,
                    ArmPermissions.ProductsImportCatalog);

                var task = new ProductImportTask(
                    parameters.PartnerId,
                    parameters.FullFilePath,
                    parameters.UserId,
                    WeightProcessTypes.WeightRequired,
                    ParamsProcessTypes.NotAcceptParamDuplicate);

                var partner = this.partnerRepository.GetById(parameters.PartnerId);
                if (partner == null)
                {
                    return
                        ServiceOperationResult.BuildFailResult<ImportProductsFromYmlResult>(
                            ResultCodes.PARTNER_NOT_FOUND, "Партнер не найден");
                }

                if (partner.Type == PartnerType.Online)
                {
                    return
                        ServiceOperationResult.BuildFailResult<ImportProductsFromYmlResult>(
                            ResultCodes.PARTNER_CANT_IMPORT_CATALOG, "Импорт каталога для online партнера не возможен");
                }

                var saved = this.importTaskRepository.SaveProductImportTask(task);

                ImportYmlJob.AddJob(saved.Id);

                return ImportProductsFromYmlResult.BuildSuccess(saved.Id);
            }
            catch (Exception ex)
            {
                log.Error("Необработанная ошибка при формировании задачи импорта", ex);
                return ServiceOperationResult.BuildErrorResult<ImportProductsFromYmlResult>(ex);
            }
        }

        public ImportDeliveryRatesFromHttpResult ImportDeliveryRatesFromHttp(int partnerId, string fileUrl, string userId)
        {
            try
            {
                userId.ThrowIfNull("userId");
                fileUrl.ThrowIfNull("fileUrl");

                securityChecker.CheckPermissions(userId, partnerId, ArmPermissions.PartnersDeliveryMatrix);

                var jobId = ImportDeliveryRatesJob.AddJob(partnerId, fileUrl, userId);
                return ImportDeliveryRatesFromHttpResult.BuildSuccess(jobId);
            }
            catch (Exception ex)
            {
                log.Error("Необработанная ошибка при формировании задачи импорта", ex);
                return ServiceOperationResult.BuildErrorResult<ImportDeliveryRatesFromHttpResult>(ex);
            }
        }

        public GetProductCatalogImportTasksHistoryResult GetProductCatalogImportTasksHistory(
            GetImportTasksHistoryParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.UserId.ThrowIfNull("parameters.UserId");

                securityChecker.CheckPermissions(
                    parameters.UserId,
                    parameters.PartnerId,
                    ArmPermissions.Products,
                    ArmPermissions.ProductsImportCatalog);

                var internalCountToTake = parameters.CountToTake ?? ApiSettings.MaxResultsCountImportYmlTasks;

                var page = this.importTaskRepository.GetPageProductImportTask(
                    parameters.PartnerId, parameters.CountToSkip, internalCountToTake, parameters.CalcTotalCount);

                return GetProductCatalogImportTasksHistoryResult.BuidlSuccess(page.ToArray(), page.TotalCount, internalCountToTake);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка при получении списка задач импортирования yml-каталога", ex);
                return ServiceOperationResult.BuildErrorResult<GetProductCatalogImportTasksHistoryResult>(ex);
            }
        }

        public GetDeliveryRateImportTasksHistoryResult GetDeliveryRateImportTasksHistory(GetImportTasksHistoryParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.UserId.ThrowIfNull("parameters.UserId");

                securityChecker.CheckPermissions(
                    parameters.UserId,
                    parameters.PartnerId,
                    ArmPermissions.Products,
                    ArmPermissions.ProductsImportCatalog);

                var internalCountToTake = parameters.CountToTake ?? ApiSettings.MaxResultsCountImportDeliveryRateTasks;

                var page = this.importTaskRepository.GetPageDeliveryRateImportTask(
                    parameters.PartnerId, parameters.CountToSkip, internalCountToTake, parameters.CalcTotalCount);

                return GetDeliveryRateImportTasksHistoryResult.BuidlSuccess(page, page.TotalCount, internalCountToTake);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка при получении списка задач импорта тарифов доставки", ex);
                return ServiceOperationResult.BuildErrorResult<GetDeliveryRateImportTasksHistoryResult>(ex);
            }
        }

        public GetSubCategoriesResult GetAllSubCategories(GetAllSubCategoriesParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.UserId.ThrowIfNull("parameters.userId");

                securityChecker.CheckPermissions(parameters.UserId, ArmPermissions.ProductCategories);

                var retVal = this.categoriesSearcher.GetAllSubCategories(parameters);
                return retVal;
            }
            catch (Exception ex)
            {
                log.Error("Ошибка при получении списка суб-категорий", ex);
                return ServiceOperationResult.BuildErrorResult<GetSubCategoriesResult>(ex);
            }
        }

        public SearchProductsResult SearchProducts(AdminSearchProductsParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.UserId.ThrowIfNull("parameters.UserId");

                securityChecker.CheckPermissions(parameters.UserId, ArmPermissions.Products);

                return this.productsSearcher.AdminSearchProducts(parameters);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка при поиске продуктов/товаров", ex);
                return ServiceOperationResult.BuildErrorResult<SearchProductsResult>(ex);
            }
        }

        public AdminGetProductResult GetProductById(ArmGetProductByIdParameters parameters)
        {
            try
            {
                Utils.CheckArgument(parameters == null, "parameters");
                Utils.CheckArgument(parameters.UserId == null, "parameters.UserId");

                Utils.CheckArgument(string.IsNullOrEmpty(parameters.ProductId), "parameters.ProductId");

                securityChecker.CheckPermissions(parameters.UserId, ArmPermissions.Products);

                var partnerId = productsRepository.GetProductPartnerId(parameters.ProductId);

                var prod = productsDataSource.GetById(parameters.ProductId, partnerId);

                return AdminGetProductResult.BuildSuccess(prod);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка при поиске продуктов/товаров", ex);
                return ServiceOperationResult.BuildErrorResult<AdminGetProductResult>(ex);
            }
        }

        public CreateCategoryResult CreateCategory(CreateCategoryParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.UserId.ThrowIfNull("parameters.userId");

                if (string.IsNullOrEmpty(parameters.Name))
                {
                    throw new ArgumentException("Name");
                }

                securityChecker.CheckPermissions(
                   parameters.UserId, ArmPermissions.ProductCategories, ArmPermissions.ProductCategoriesManage);

                if (string.IsNullOrEmpty(parameters.OnlineCategoryUrl) && parameters.Type == ProductCategoryTypes.Online)
                {
                    throw new ArgumentException("OnlineCategoryUrl должен быть заполнен для динамической категории");
                }

                var category = this.productCategoriesDataSource.CreateProductCategory(parameters);
                var createCategoryResult = new CreateCategoryResult
                {
                    ResultCode = ResultCodes.SUCCESS,
                    Category = category
                };
                return createCategoryResult;
            }
            catch (Exception ex)
            {
                log.Error("Ошибка создания категории", ex);
                return ServiceOperationResult.BuildErrorResult<CreateCategoryResult>(ex);
            }
        }

        public UpdateCategoryResult UpdateCategory(UpdateCategoryParameters parameters)
        {
            try
            {
                Utils.CheckArgument(p => p == null, parameters, "parameters is null");
                Utils.CheckArgument(p => p < 0, parameters.CategoryId, "Id must be >= 0");
                Utils.CheckArgument(p => p == null, parameters.UserId, "userId is null");
                
                securityChecker.CheckPermissions(
                   parameters.UserId, ArmPermissions.ProductCategories, ArmPermissions.ProductCategoriesManage);
                
                Utils.CheckArgument(string.IsNullOrEmpty, parameters.NewName, "Name");

                var cat = this.productCategoriesDataSource.UpdateCategory(parameters);

                return new UpdateCategoryResult
                {
                    ResultCode = ResultCodes.SUCCESS,
                    Category = cat
                };
            }
            catch (Exception ex)
            {
                log.Error("Ошибка обновления категории", ex);
                return ServiceOperationResult.BuildErrorResult<UpdateCategoryResult>(ex);
            }
        }

        public ResultBase DeleteCategory(string userId, int categoryId)
        {
            try
            {
                Utils.CheckArgument(string.IsNullOrEmpty, userId, "userId");
                securityChecker.CheckPermissions(
                    userId, ArmPermissions.ProductCategories, ArmPermissions.ProductCategoriesManage);
                Utils.CheckArgument(c => c == 0, categoryId, "categoryId");

                return this.productCategoriesRepository.DeleteCategory(userId, categoryId);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка удаления категории", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        public CreatePartnerResult CreatePartner(CreatePartnerParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.UserId.ThrowIfNull("parameters.UserId");
                parameters.Name.ThrowIfNull("parameters.Name");
                parameters.Settings.ThrowIfNull("parameters.Settings");

                securityChecker.CheckPermissions(
                    parameters.UserId, ArmPermissions.Partners, ArmPermissions.PartnersCreateUpdateDelete);

                var t = partnerRepository.GetByName(parameters.Name);
                if (t != null)
                {
                    var res = new CreatePartnerResult
                    {
                        ResultCode = ResultCodes.PARTNER_WITH_NAME_EXISTS,
                        ResultDescription = "Партнер с таким именем уже существует"
                    };

                    return res;
                }

                if (parameters.CarrierId != null)
                {
                    var carrier = partnerRepository.GetById(parameters.CarrierId.Value);
                    if (carrier == null)
                    {
                        var res = new CreatePartnerResult
                        {
                            ResultCode = ResultCodes.CARRIER__NOT_FOUND,
                            ResultDescription = "Курьер не найден"
                        };

                        return res;
                    }
                }

                var p = new Partner
                            {
                                InsertedUserId = parameters.UserId,
                                Name = parameters.Name,
                                Type = parameters.Type,
                                Status = parameters.Status,
                                ThrustLevel = parameters.ThrustLevel,
                                Description = parameters.Description,
                                InsertedDate = DateTime.Now,
                                CarrierId = parameters.CarrierId,
                                IsCarrier = parameters.IsCarrier
                            };

                this.partnerRepository.CreateOrUpdate(parameters.UserId, p, parameters.Settings);

                // Создаём активный каталог для партнёра
                var dateKey = LoyaltyDBSpecification.GetDateKey(DateTime.Now);
                var importKey = ProductCatalogsDataSource.InsertCatalog(p.Id, dateKey);
                ProductsDataSource.CreateProductsTable(importKey, p.Id);
                ProductsDataSource.CreateProductsTableConstraints(importKey, p.Id);
                ProductCatalogsDataSource.SetActiveCatalog(p.Id, importKey);

                var createPartnerResult = new CreatePartnerResult
                {
                    Partner = p,
                    ResultCode = ResultCodes.SUCCESS
                };
                return createPartnerResult;
            }
            catch (Exception ex)
            {
                log.Error("Ошибка создания партнера", ex);
                return ServiceOperationResult.BuildErrorResult<CreatePartnerResult>(ex);
            }
        }

        public UpdatePartnerResult UpdatePartner(UpdatePartnerParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.UpdatedUserId.ThrowIfNull("parameters.UpdatedUserId");
                parameters.NewName.ThrowIfNull("parameters.NewName");
                parameters.NewSettings.ThrowIfNull("parameters.NewSettings");

                securityChecker.CheckPermissions(
                    parameters.UpdatedUserId,
                    ArmPermissions.Partners,
                    ArmPermissions.PartnersCreateUpdateDelete);

                if (parameters.NewName != null)
                {
                    var t = this.partnerRepository.GetByName(parameters.NewName);
                    if ((t != null) && (t.Id != parameters.Id))
                    {
                        var res = new UpdatePartnerResult
                        {
                            ResultCode = ResultCodes.PARTNER_WITH_NAME_EXISTS,
                            ResultDescription = "Партнер с таким именем уже существует"
                        };

                        return res;
                    }
                }

                var p = this.partnerRepository.GetById(parameters.Id);
                if (p == null)
                {
                    var res = new UpdatePartnerResult
                    {
                        ResultCode = ResultCodes.PARTNER_NOT_FOUND,
                        ResultDescription = "Партнер не найден"
                    };

                    return res;
                }

                p.UpdatedUserId = parameters.UpdatedUserId;

                if (parameters.NewName != null)
                {
                    p.Name = parameters.NewName;
                }

                p.Type = parameters.NewType;
                p.Status = parameters.NewStatus;
                p.ThrustLevel = parameters.NewThrustLevel;
                p.UpdatedDate = DateTime.Now;
                p.Description = parameters.NewDescription;

                if (parameters.NewCarrierId.HasValue)
                {
                    var carrier = partnerRepository.GetById(parameters.NewCarrierId.Value);
                    if (carrier == null)
                    {
                        var res = new UpdatePartnerResult
                                      {
                                          ResultCode = ResultCodes.CARRIER__NOT_FOUND,
                                          ResultDescription = "Курьер не найден"
                                      };

                        return res;
                    }

                    p.CarrierId = parameters.NewCarrierId;
                    p.Carrier = carrier;
                }
                else
                {
                    p.CarrierId = null;
                    p.Carrier = null;
                }

                this.partnerRepository.CreateOrUpdate(parameters.UpdatedUserId, p, settings: parameters.NewSettings);
                var updatePartnerResult = new UpdatePartnerResult
                {
                    Partner = p,
                    ResultCode = ResultCodes.SUCCESS,
                };
                return updatePartnerResult;
            }
            catch (Exception ex)
            {
                log.Error("Ошибка обновления партнера", ex);
                return ServiceOperationResult.BuildErrorResult<UpdatePartnerResult>(ex);
            }
        }

        public GetPartnerResult GetPartners(int[] ids, string userId)
        {
            try
            {
                securityChecker.CheckPermissions(userId, ArmPermissions.Partners);

                var partners = this.InternalGetPartners(ids);

                if (!partners.Any())
                {
                    return ServiceOperationResult.BuildFailResult<GetPartnerResult>(
                        ResultCodes.PARTNER_NOT_FOUND, "Партнеры не найдены");
                }

                var searchPartnerResult = GetPartnerResult.BuildSuccess(partners);

                return searchPartnerResult;
            }
            catch (Exception ex)
            {
                log.Error("Ошибка при поиске партнера", ex);
                return ServiceOperationResult.BuildErrorResult<GetPartnerResult>(ex);
            }
        }

        public GetPartnersInfoResult GetPartnersInfo(int[] ids, string userId)
        {
            try
            {
                var partners = this.InternalGetPartners(ids);

                if (!partners.Any())
                {
                    return ServiceOperationResult.BuildFailResult<GetPartnersInfoResult>(
                        ResultCodes.PARTNER_NOT_FOUND, "Партнеры не найдены");
                }

                var dtos = partners.Select(x => new PartnerInfo(x));
                var searchPartnerResult = GetPartnersInfoResult.BuildSuccess(dtos);

                return searchPartnerResult;
            }
            catch (Exception ex)
            {
                log.Error("Ошибка при поиске партнера", ex);
                return ServiceOperationResult.BuildErrorResult<GetPartnersInfoResult>(ex);
            }
        }

        public GetPartnerInfoByIdResult GetPartnerInfoById(int id, string userId)
        {
            try
            {
                userId.ThrowIfNull("userId");

                var partner = this.partnerRepository.GetById(id);
                if (partner == null)
                {
                    return ServiceOperationResult.BuildFailResult<GetPartnerInfoByIdResult>(
                            ResultCodes.PARTNER_NOT_FOUND, "Партнер не найден");
                }

                var dto = new PartnerInfo(partner);
                var searchPartnerResult = GetPartnerInfoByIdResult.BuildSuccess(dto);

                return searchPartnerResult;
            }
            catch (Exception ex)
            {
                log.Error("Ошибка при поиске партнера", ex);
                return ServiceOperationResult.BuildErrorResult<GetPartnerInfoByIdResult>(ex);
            }
        }

        public GetPartnerByIdResult GetPartnerById(int id, string userId)
        {
            try
            {
                userId.ThrowIfNull("userId");

                securityChecker.CheckPermissions(userId, ArmPermissions.Partners);

                var p = this.partnerRepository.GetById(id);
                if (p == null)
                {
                    return new GetPartnerByIdResult
                               {
                                   ResultCode = ResultCodes.PARTNER_NOT_FOUND,
                                   ResultDescription = "Партнер не найден",
                               };
                }

                var searchPartnerResult = new GetPartnerByIdResult
                {
                    Partner = p,
                    ResultCode = ResultCodes.SUCCESS
                };

                return searchPartnerResult;
            }
            catch (Exception ex)
            {
                log.Error("Ошибка при поиске партнера", ex);
                return ServiceOperationResult.BuildErrorResult<GetPartnerByIdResult>(ex);
            }
        }

        public ResultBase DeletePartnerSettings(string userId, int partnerId, string[] keys)
        {
            try
            {
                userId.ThrowIfNull("userId");
                securityChecker.CheckPermissions(
                    userId, ArmPermissions.Partners, ArmPermissions.PartnersCreateUpdateDelete);

                this.partnerRepository.DeleteSettings(userId, partnerId, keys);

                return ResultBase.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.Error("Необработанная ошибка при удалении настроек партнера", ex);
                return ServiceOperationResult.BuildErrorResult<ImportDeliveryRatesFromHttpResult>(ex);
            }
        }

        public PartnersSettignsResult GetPartnersSettings(string userId, int? partnerId)
        {
            try
            {
                userId.ThrowIfNull("userId");
                securityChecker.CheckPermissions(userId, ArmPermissions.Partners);

                var retVal = this.partnerRepository.GetSettings(partnerId);

                return PartnersSettignsResult.BuildSuccess(retVal);
            }
            catch (Exception ex)
            {
                log.Error("Необработанная ошибка получении настроек партнера", ex);
                return ServiceOperationResult.BuildErrorResult<PartnersSettignsResult>(ex);
            }
        }

        public ResultBase SetPartnerSettings(string userId, int partnerId, Dictionary<string, string> settings)
        {
            try
            {
                userId.ThrowIfNull("userId");
                securityChecker.CheckPermissions(
                    userId, ArmPermissions.Partners, ArmPermissions.PartnersCreateUpdateDelete);

                this.partnerRepository.ReplaceSettings(userId, partnerId, settings);

                return ResultBase.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.Error("Необработанная ошибка при сохранении настроек партнера", ex);
                return ServiceOperationResult.BuildErrorResult<ImportDeliveryRatesFromHttpResult>(ex);
            }
        }

        public ResultBase MoveCategory(MoveCategoryParameters parameters)
        {
            try
            {
                Utils.CheckArgument(c => c == 0, parameters.CategoryId, "parameters.CategoryId is 0");
                Utils.CheckArgument(c => c == null, parameters.UserId, "parameters.userId");
                securityChecker.CheckPermissions(
                    parameters.UserId, ArmPermissions.ProductCategories, ArmPermissions.ProductCategoriesManage);
                Utils.CheckArgument(c => c == 0, parameters.ReferenceCategoryId, "parameters.ReferenceCategoryId is 0");

                if (parameters.CategoryId == parameters.ReferenceCategoryId)
                {
                    throw new ArgumentException("Категория не может быть родителем сама для себя");
                }

                // NOTE: Проверка что не происходит перемещение родителя в дочку
                if (parameters.ReferenceCategoryId.HasValue)
                {
                    var subCategories = this.GetSubCategories(parameters.CategoryId);

                    if (subCategories.Any(x => x.Id == parameters.ReferenceCategoryId.Value))
                    {
                        return ResultBase.BuildFail(
                                    ResultCodes.CATEGORY_INVALID_MOVE,
                                    "Родительская категория не может стать дочерней своего ребенка");
                    }
                }

                if (parameters.PositionType == CategoryPositionTypes.Append || parameters.PositionType == CategoryPositionTypes.Prepend)
                {
                    var categories = parameters.ReferenceCategoryId.HasValue
                        ? this.GetSubCategories(parameters.ReferenceCategoryId.Value, 1)
                        : this.GetRootCategories();

                    if (categories.Length == 0)
                    {
                        // Обновить родителя
                        this.productCategoriesRepository.UpdateParent(parameters.UserId, parameters.CategoryId, parameters.ReferenceCategoryId);
                        return ResultBase.BuildSuccess();
                    }

                    var referenceCategoryId = parameters.PositionType == CategoryPositionTypes.Prepend
                                                  ? categories.First().Id
                                                  : categories.Last().Id;
                    var referenceType = parameters.PositionType == CategoryPositionTypes.Prepend
                                            ? CategoryPositionTypes.Before
                                            : CategoryPositionTypes.After;
                    var param = new MoveCategoryParameters
                                    {
                                        CategoryId = parameters.CategoryId,
                                        ReferenceCategoryId = referenceCategoryId,
                                        UserId = parameters.UserId,
                                        PositionType = referenceType
                                    };

                    return this.productCategoriesRepository.MoveCategory(param);
                }

                if (!parameters.ReferenceCategoryId.HasValue)
                {
                    var categories = this.GetRootCategories();

                    if (categories.Length == 0)
                    {
                        // Обновить родителя
                        this.productCategoriesRepository.UpdateParent(parameters.UserId, parameters.CategoryId, parameters.ReferenceCategoryId);
                        return ResultBase.BuildSuccess();
                    }

                    parameters.ReferenceCategoryId = parameters.PositionType == CategoryPositionTypes.Prepend
                                                      ? categories.First().Id
                                                      : categories.Last().Id;
                }

                return this.productCategoriesRepository.MoveCategory(parameters);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка удаления категории", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        public ResultBase ChangeCategoriesStatus(string userId, int[] categoryIds, ProductCategoryStatuses status)
        {
            try
            {
                Utils.CheckArgument(string.IsNullOrEmpty, userId, "userId");
                securityChecker.CheckPermissions(
                    userId, ArmPermissions.ProductCategories, ArmPermissions.ProductCategoriesManage);
                Utils.CheckArgument(c => c == null || !c.Any(), categoryIds, "categoryIds");

                return this.productCategoriesRepository.ChangeCategoriesStatus(userId, categoryIds, status);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка изменения статуса категорий", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        public CreatePartnerProductCategoryLinkResult SetPartnerProductCategoryLink(
            CreatePartnerProductCateroryLinkParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.UserId.ThrowIfNull("parameters.userId");
                parameters.Link.ThrowIfNull("parameters.Link");

                securityChecker.CheckPermissions(parameters.UserId, parameters.Link.PartnerId, ArmPermissions.ProductCategoryLinks);

                if (parameters.Link.Paths == null)
                {
                    var error = string.Format("Не указаны пути привязки категории");
                    return CreatePartnerProductCategoryLinkResult.BuildFail(ResultCodes.INVALID_PARAMETER_VALUE, error);
                }

                var inputLink = parameters.Link;
                var userId = parameters.UserId;

                var partnerId = inputLink.PartnerId;

                var partner = this.partnerRepository.GetById(partnerId);
                if (partner == null)
                {
                    var error = string.Format("Партнер с идентификатором {0} не существует", partnerId);
                    return CreatePartnerProductCategoryLinkResult.BuildFail(ResultCodes.INVALID_PARAMETER_VALUE, error);
                }

                var categoryId = parameters.Link.ProductCategoryId;

                var category = this.productCategoriesDataSource.GetProductCategoryById(categoryId);
                if (category == null)
                {
                    var error = string.Format("Категории с идентификатором {0} не существует", categoryId);
                    return CreatePartnerProductCategoryLinkResult.BuildFail(ResultCodes.INVALID_PARAMETER_VALUE, error);
                }

                var paths = inputLink.Paths.Select(p => p.NamePath).ToList();
                foreach (var path in paths)
                {
                    if (partnerProductCateroryLinkRepository.IsUniqueLinks(inputLink.PartnerId, categoryId, path) 
                        && paths.Count(p => p.Equals(path, StringComparison.InvariantCulture)) == 1)
                    {
                        continue;
                    }

                    var error = string.Format("Привязка к категории для пути \"{0}\" уже существует", path);
                    return CreatePartnerProductCategoryLinkResult.BuildFail(ResultCodes.ALLREADY_EXISTS, error);
                }

                var link = this.SetPartnerProductCateroryLinkInternal(inputLink, userId);

                var result = new CreatePartnerProductCategoryLinkResult
                {
                    Link = link,
                    ResultCode = ResultCodes.SUCCESS
                };

                return result;
            }
            catch (Exception ex)
            {
                log.Error("Ошибка при создании привязки", ex);
                return ServiceOperationResult.BuildErrorResult<CreatePartnerProductCategoryLinkResult>(ex);
            }
        }

        public GetPartnerProductCategoryLinksResult GetPartnerProductCategoryLinks(string userId, int partnerId, int[] categoryIds)
        {
            try
            {
                userId.ThrowIfNull("userId");

                securityChecker.CheckPermissions(userId, partnerId, ArmPermissions.ProductCategoryLinks);

                var links = this.partnerProductCateroryLinkRepository.GetPartnerProductCateroryLinks(
                    partnerId, categoryIds);

                var mapped = links.GroupBy(
                    x => new
                    {
                        x.PartnerId,
                        x.ProductCategoryId
                    },
                    (key, list) =>
                        new PartnerProductCategoryLink(key.PartnerId, key.ProductCategoryId, ToCaterogyPaths(list)))
                                  .ToArray();

                var result = GetPartnerProductCategoryLinksResult.BuildSuccess(mapped);

                return result;
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения списка связок", ex);
                return ServiceOperationResult.BuildErrorResult<GetPartnerProductCategoryLinksResult>(ex);
            }
        }

        public ResultBase SetProductsTargetAudiences(SetProductsTargetAudiencesParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.UserId.ThrowIfNull("parameters.UserId");

                if (parameters.ProductIds != null && parameters.ProductIds.Any(x => x == null))
                {
                    throw new ArgumentException("Один из элементов списка иденктификаторов товаров имеет значение null");
                }

                if (parameters.TargetAudienceIds == null || parameters.TargetAudienceIds.Any(string.IsNullOrWhiteSpace))
                {
                    throw new ArgumentException(
                        "parameters.TargetAudienceIds равен null или один из элементов списка пустой");
                }

                securityChecker.CheckPermissions(parameters.UserId, ArmPermissions.Products, ArmPermissions.ProductsAssignAudience);

                Utils.CheckArgument(parameters.ProductIds == null || !parameters.ProductIds.Any(), "parameters.ProductIds");

                productsRepository.RemoveProductTargetAudiences(parameters.UserId, parameters.ProductIds);
                var targetAudienceIds = parameters.TargetAudienceIds.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                productsRepository.AddProductTargetAudiences(parameters.UserId, parameters.ProductIds, targetAudienceIds);

                return ResultBase.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.Error("Ошибка сохранения разрешений", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        public ResultBase SetCategoriesPermissions(SetCategoriesPermissionsParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");

                securityChecker.CheckPermissions(
                    parameters.UserId, parameters.PartnerId, ArmPermissions.ProductCategoryLinks);

                if (parameters.AddCategoriesId != null && parameters.RemoveCategoriesId != null)
                {
                    var intersect = parameters.AddCategoriesId.Intersect(parameters.RemoveCategoriesId).ToArray();
                    if (intersect.Any())
                    {
                        var error =
                            string.Concat(
                                "Наборы идентификаторов для добавления и удаления разрешений содержат пересечения: ",
                                string.Join(", ", intersect));

                        return ResultBase.BuildFail(ResultCodes.CATEGORIES_INTERSECT, error);
                    }
                }

                var partner = this.partnerRepository.GetById(parameters.PartnerId);
                if (partner == null)
                {
                    var error = string.Format("Партнер с идентификатором {0} не существует", parameters.PartnerId);
                    return ResultBase.BuildFail(ResultCodes.PARTNER_NOT_FOUND, error);
                }

                if (parameters.AddCategoriesId != null && parameters.AddCategoriesId.Length > 0)
                {
                    var checkResult = this.ValidateCategoriesExists(parameters.AddCategoriesId);
                    if (!checkResult.Success)
                    {
                        return checkResult;
                    }
                }

                if (parameters.RemoveCategoriesId != null && parameters.RemoveCategoriesId.Length > 0)
                {
                    var checkResult = this.ValidateCategoriesExists(parameters.RemoveCategoriesId);
                    if (!checkResult.Success)
                    {
                        return checkResult;
                    }
                }

                if (parameters.AddCategoriesId != null && parameters.AddCategoriesId.Length > 0)
                {
                    var create =
                        parameters.AddCategoriesId.Select(
                            x => new CategoryPermission(parameters.PartnerId, x, parameters.UserId)).ToArray();

                    this.categoryPermissionRepository.Save(parameters.UserId, create);
                }

                if (parameters.RemoveCategoriesId != null && parameters.RemoveCategoriesId.Length > 0)
                {
                    this.categoryPermissionRepository.Delete(parameters.UserId, parameters.PartnerId, parameters.RemoveCategoriesId);
                }

                return ResultBase.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.Error("Ошибка сохранения разрешений", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        public GetCategoriesPermissionsResult GetCategoriesPermissions(string userId, int partnerId)
        {
            try
            {
                userId.ThrowIfNull("userId");

                var list = this.categoryPermissionRepository.GetByPartner(partnerId);

                if (list == null)
                {
                    return GetCategoriesPermissionsResult.BuildSuccess();
                }

                var ids = list.Select(x => x.CategoryId).ToArray();
                return GetCategoriesPermissionsResult.BuildSuccess(ids);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения ", ex);
                return ServiceOperationResult.BuildErrorResult<GetCategoriesPermissionsResult>(ex);
            }
        }

        public ResultBase ChangeProductsStatus(ChangeStatusParameters parameters)
        {
            try
            {
                Utils.CheckArgument(parameters == null, "parameters");
                Utils.CheckArgument(parameters.UserId == null, "parameters.UserId");

                securityChecker.CheckPermissions(
                    parameters.UserId, ArmPermissions.Products, ArmPermissions.ProductsActivateDeactivate);

                Utils.CheckArgument(!parameters.ProductIds.Any(), "parameters.ProductIds");

                return new ProductsDataSource().ChangeProductsStatus(parameters);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка изменения статуса", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        public ResultBase ChangeProductsStatusByPartner(ChangeStatusByPartnerParameters parameters)
        {
            try
            {
                Utils.CheckArgument(parameters == null, "parameters");
                Utils.CheckArgument(parameters.UserId == null, "parameters.UserId");

                securityChecker.CheckPermissions(
                    parameters.UserId, ArmPermissions.Products, ArmPermissions.ProductsActivateDeactivate);

                Utils.CheckArgument(!parameters.PartnerProductIds.Any(), "parameters.ProductIds");

                return productsRepository.ChangeStatusesByPartner(parameters);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка изменения статуса", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        public ResultBase ChangeProductsModerationStatus(ChangeModerationStatusParameters parameters)
        {
            try
            {
                Utils.CheckArgument(parameters == null, "parameters");
                Utils.CheckArgument(parameters.UserId == null, "parameters.UserId");

                securityChecker.CheckPermissions(
                    parameters.UserId, ArmPermissions.Products, ArmPermissions.ProductsModerate);

                Utils.CheckArgument(!parameters.ProductIds.Any(), "parameters.Products");

                return new ProductsDataSource().ChangeProductsModerationStatus(parameters);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка изменения статуса ", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        public ResultBase DeleteCache(int seconds, string userId)
        {
            try
            {
                securityChecker.CheckPermissions(userId, ArmPermissions.Products);

                this.productsDataSource.DeleteCache(seconds);
                new MechanicsProvider().ClearCache();
                
                return ResultBase.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.Error("Ошибка отчистки кэша", ex);
                return ServiceOperationResult.BuildErrorResult<CreateProductResult>(ex);
            }
        }

        public CreateProductResult CreateProduct(CreateProductParameters parameters)
        {
            try
            {
                Utils.CheckArgument(parameters == null, "parameters");
                Utils.CheckArgument(string.IsNullOrEmpty(parameters.UserId), "parameters.UserId");
                Utils.CheckArgument(parameters.PartnerId <= 0, "parameters.PartnerId");

                securityChecker.CheckPermissions(
                    parameters.UserId,
                    parameters.PartnerId,
                    ArmPermissions.Products,
                    ArmPermissions.ProductsCreateUpdate);

                Utils.CheckArgument(string.IsNullOrEmpty(parameters.PartnerProductId), "parameters.PartnerProductId");
                Utils.CheckArgument(parameters.CategoryId <= 0, "parameters.CategoryId");
                Utils.CheckArgument(string.IsNullOrEmpty(parameters.Name), "parameters.Name");
                Utils.CheckArgument(parameters.PriceRUR < 0, "parameters.PriceRUR");
                Utils.CheckParameters(parameters);

                return productsDataSource.CreateProduct(parameters);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка вставки ", ex);
                return ServiceOperationResult.BuildErrorResult<CreateProductResult>(ex);
            }
        }

        public ResultBase UpdateProduct(UpdateProductParameters parameters)
        {
            try
            {
                Utils.CheckArgument(parameters == null, "parameters");
                Utils.CheckArgument(string.IsNullOrEmpty(parameters.UserId), "parameters.UserId");
                Utils.CheckArgument(parameters.PartnerId <= 0, "parameters.PartnerId");

                securityChecker.CheckPermissions(
                    parameters.UserId,
                    parameters.PartnerId,
                    ArmPermissions.Products,
                    ArmPermissions.ProductsCreateUpdate);

                Utils.CheckArgument(string.IsNullOrEmpty(parameters.ProductId), "parameters.ProductId");
                Utils.CheckArgument(parameters.CategoryId <= 0, "parameters.CategoryId");
                Utils.CheckArgument(string.IsNullOrEmpty(parameters.Name), "parameters.Name");
                Utils.CheckArgument(parameters.PriceRUR < 0, "parameters.PriceRUR");
                Utils.CheckParameters(parameters);

                return productsDataSource.UpdateProduct(parameters);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка обновления ", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        public ResultBase MoveProducts(MoveProductsParameters parameters)
        {
            try
            {
                Utils.CheckArgument(parameters == null, "parameters");
                Utils.CheckArgument(string.IsNullOrEmpty(parameters.UserId), "parameters.UserId");

                // NOTE: Нет никакого спец разрешения на перемещение, поэтому используем ArmPermissions.ProductsCreateUpdate
                securityChecker.CheckPermissions(
                    parameters.UserId,
                    ArmPermissions.Products,
                    ArmPermissions.ProductsChangeProductCategory);

                Utils.CheckArgument(parameters.TargetCategoryId <= 0, "parameters.TargetCategoryId");
                Utils.CheckArgument(!parameters.ProductIds.Any(), "parameters.ProductIds");

                if (productCategoriesRepository.GetById(parameters.TargetCategoryId) == null)
                {
                    return ResultBase.BuildNotFound("Target category not found");
                }

                return productsRepository.MoveProducts(parameters);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка перемещения ", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        public ResultBase DeleteProducts(DeleteProductParameters parameters)
        {
            try
            {
                Utils.CheckArgument(parameters == null, "parameters");
                Utils.CheckArgument(string.IsNullOrEmpty(parameters.UserId), "parameters.UserId");

                Utils.CheckArgument(!parameters.ProductIds.Any(), "parameters.ProductIds");

                productsRepository.DeleteProducts(parameters);

                return ResultBase.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.Error("Ошибка удаления ", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        public ResultBase RecommendProducts(RecommendParameters parameters)
        {
            try
            {
                Utils.CheckArgument(parameters == null, "parameters");
                Utils.CheckArgument(string.IsNullOrEmpty(parameters.UserId), "parameters.UserId");

                securityChecker.CheckPermissions(
                    parameters.UserId,
                    ArmPermissions.Products,
                    ArmPermissions.ProductsRecommend);

                Utils.CheckArgument(!parameters.ProductIds.Any(), "parameters.ProductIds");

                return productsRepository.ChangeStatuses(parameters);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка изменения признака \"рекомендованный\"", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        public SearchOrdersResult SearchOrders(SearchOrdersParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.UserId.ThrowIfNull("parameters.UserId");

                if (parameters.PartnerIds != null)
                {
                    foreach (var partnerId in parameters.PartnerIds)
                    {
                        securityChecker.CheckPermissions(parameters.UserId, partnerId, ArmPermissions.Orders);
                    }
                }

                if (parameters.CarrierIds != null)
                {
                    foreach (var carrierId in parameters.CarrierIds)
                    {
                        securityChecker.CheckPermissions(parameters.UserId, carrierId, ArmPermissions.Orders);
                    }
                }

                var now = DateTime.Now;

                // NOTE: Считаем что если дата не указана, то берем с начала текущего месяца!
                var internalStartDate = parameters.StartDate ?? now.AddYears(-10);
                var internalEndDate = parameters.EndDate ?? now;
                var internalCountToSkip = parameters.CountToSkip ?? 0;
                var internalCountToTake = parameters.CountToTake.NormalizeByHeight(ApiSettings.MaxResultsCountOrders);

                var internalCalcTotalCount = parameters.CalcTotalCount.HasValue && parameters.CalcTotalCount.Value;

                var orders = ordersDataSource.SearchOrders(
                   internalStartDate,
                   internalEndDate,
                   parameters.Statuses,
                   parameters.SkipStatuses,
                   parameters.OrderPaymentStatuses,
                   parameters.OrderDeliveryPaymentStatus,
                   parameters.PartnerIds,
                   internalCountToSkip,
                   internalCountToTake,
                   internalCalcTotalCount,
                   parameters.OrderIds,
                   parameters.CarrierIds);

                return new SearchOrdersResult
                {
                    ResultCode = ResultCodes.SUCCESS,
                    Orders = orders.ToArray(),
                    TotalCount = internalCalcTotalCount ? orders.TotalCount : null
                };
            }
            catch (Exception ex)
            {
                log.Error("Ошибка поиска заказов", ex);
                return ServiceOperationResult.BuildErrorResult<SearchOrdersResult>(ex);
            }
        }

        public ChangeOrdersStatusesResult ChangeOrdersStatuses(string userId, OrdersStatus[] ordersStatuses)
        {
            try
            {
                Utils.CheckArgument(string.IsNullOrEmpty, userId, "userId");
                Utils.CheckArgument(o => o == null || !o.Any(), ordersStatuses, "ordersStatuses");

                securityChecker.CheckPermissions(userId, ArmPermissions.Orders, ArmPermissions.OrdersChangeOrderStatus);

                var results = UpdateOrdersStatuses(ordersStatuses, userId);
                
                return ChangeOrdersStatusesResult.BuildSuccess(results);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка изменения статуса заказов", ex);
                return ServiceOperationResult.BuildErrorResult<ChangeOrdersStatusesResult>(ex);
            }
        }

        public ResultBase ChangeOrdersStatusDescription(string userId, int orderId, string orderStatusDescription)
        {
            try
            {
                Utils.CheckArgument(string.IsNullOrEmpty, userId, "userId");
                Utils.CheckArgument(x => x == null, orderStatusDescription, "userId");

                securityChecker.CheckPermissions(userId, ArmPermissions.Orders, ArmPermissions.OrdersChangeOrderStatus);

                var ordersStatus = new OrdersStatus
                {
                    OrderId = orderId,
                    OrderStatusDescription = orderStatusDescription
                };

                var result = ordersStatusChanger.UpdateOrderStatus(ordersStatus, userId);
                return result;
            }
            catch (Exception ex)
            {
                log.Error("Ошибка изменения статуса заказов", ex);
                return ServiceOperationResult.BuildErrorResult<ChangeOrdersStatusesResult>(ex);
            }
        }

        public ChangeOrdersStatusesResult ChangeOrdersPaymentStatuses(string userId, OrdersPaymentStatus[] statuses)
        {
            try
            {
                Utils.CheckArgument(string.IsNullOrEmpty, userId, "userId");
                Utils.CheckArgument(o => o == null || !o.Any(), statuses, "statuses");

                securityChecker.CheckPermissions(userId, ArmPermissions.Orders, ArmPermissions.OrdersChangeOrderStatus);

                var results = ordersDataSource.UpdateOrdersPaymentStatuses(statuses, userId);
                return ChangeOrdersStatusesResult.BuildSuccess(results);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка изменения статуса оплаты заказов", ex);
                return ServiceOperationResult.BuildErrorResult<ChangeOrdersStatusesResult>(ex);
            }
        }

        public ChangeOrdersStatusesResult ChangeOrdersDeliveryStatuses(string userId, OrdersDeliveryStatus[] statuses)
        {
            try
            {
                Utils.CheckArgument(string.IsNullOrEmpty, userId, "userId");
                Utils.CheckArgument(o => o == null || !o.Any(), statuses, "statuses");

                securityChecker.CheckPermissions(userId, ArmPermissions.Orders, ArmPermissions.OrdersChangeOrderStatus);

                var results = ordersDataSource.UpdateOrdersDeliveryStatuses(statuses, userId);
                return ChangeOrdersStatusesResult.BuildSuccess(results);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка изменения статуса доставки заказов", ex);
                return ServiceOperationResult.BuildErrorResult<ChangeOrdersStatusesResult>(ex);
            }
        }

        public GetOrderResult GetOrderById(string userId, int orderId)
        {
            try
            {
                userId.ThrowIfNull("userId");

                securityChecker.CheckPermissions(userId, ArmPermissions.Orders);

                if (orderId <= 0)
                {
                    return
                        ServiceOperationResult.BuildErrorResult<GetOrderResult>(
                            new ArgumentException("Не валидное поле OrderId"));
                }

                var order = ordersDataSource.GetOrder(orderId);

                if (order == null)
                {
                    return GetOrderResult.BuildFail(ResultCodes.NOT_FOUND, "Заказ не найден");
                }

                var flow = this.ordersRepository.GetNextOrderStatuses(order.Status);
                return GetOrderResult.BuildSuccess(order, flow);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения заказа по идентификатору заказа", ex);
                return ServiceOperationResult.BuildErrorResult<GetOrderResult>(ex);
            }
        }

        public GetOrderStatusesHistoryResult GetOrderStatusesHistory(GetOrderStatusesHistoryParameters parameters)
        {
            if (parameters.OrderId <= 0)
            {
                return
                    ServiceOperationResult.BuildErrorResult<GetOrderStatusesHistoryResult>(
                        new ArgumentException("Не валидное поле OrderId"));
            }

            try
            {
                var items = ordersHistoryRepository.GetOrderHistory(
                    parameters.OrderId, parameters.CountToSkip, parameters.CountToTake, parameters.CalcTotalCount);

                if (items == null || items.Count == 0)
                {
                    return GetOrderStatusesHistoryResult.BuildSuccess(null, 0, "По заданным параметрам история заказов не найдена");
                }

                return GetOrderStatusesHistoryResult.BuildSuccess(items.ToArray(), items.TotalCount);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения заказа по идентификатору заказа", ex);
                return ServiceOperationResult.BuildErrorResult<GetOrderStatusesHistoryResult>(ex);
            }
        }

        public PartnerCommitOrdersResult PartnerCommitOrder(
            string userId, int partnerId, PartnerOrderCommitment[] partnerOrderCommitment)
        {
            try
            {
                securityChecker.CheckPermissions(userId, ArmPermissions.Orders, ArmPermissions.OrdersChangeOrderStatus);

                return PartnerCommitOrderInternal(partnerId, partnerOrderCommitment);
            }
            catch (Exception e)
            {
                log.Error("Ошибка обработки ответа партнера о подтверждении заказа", e);
                return ServiceOperationResult.BuildErrorResult<PartnerCommitOrdersResult>(e);
            }
        }

        public ResultBase ChangeOrderDeliveryInstructions(string userId, int orderId, string instructions)
        {
            try
            {
                securityChecker.CheckPermissions(userId, ArmPermissions.Orders, ArmPermissions.OrdersChangeOrderStatus);

                ordersRepository.UpdateDeliveryInstructions(orderId, instructions, userId);

                return ResultBase.BuildSuccess();
            }
            catch (Exception e)
            {
                log.Error("Ошибка обновления инструкций по получению заказа", e);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(e);
            }
        }

        #region IDeliveryAdminService
        public DeliveryLocationsResult GetDeliveryLocations(GetDeliveryLocationsParameters parameters, string userId)
        {
            try
            {
                userId.ThrowIfNull("userId");
                securityChecker.CheckPermissions(userId, ArmPermissions.PartnersDeliveryMatrix);
                
                var inner = parameters ?? new GetDeliveryLocationsParameters();

                var countToTake = inner.CountToTake.NormalizeByHeight(ApiSettings.MaxResultsCountDeliveryBindings);

                var bindings = this.deliveryRatesRepository.GetDeliveryLocations(
                    inner.PartnerId,
                    inner.StatusFilters,
                    inner.CountToSkip,
                    countToTake,
                    inner.CalcTotalCount,
                    inner.HasRates,
                    inner.SearchTerm);

                return DeliveryLocationsResult.BuildSuccess(bindings, bindings.TotalCount);
            }
            catch (Exception e)
            {
                log.Error("Ошибка обработки получении списка привязок", e);
                return ServiceOperationResult.BuildErrorResult<DeliveryLocationsResult>(e);
            }
        }

        public ResultBase SetDeliveryLocationKladr(int locationId, string kladr, string userId)
        {
            try
            {
                userId.ThrowIfNull("userId");
                kladr.ThrowIfNull("kladr");

                securityChecker.CheckPermissions(userId, ArmPermissions.PartnersDeliveryMatrix);

                CheckKladr(kladr);

                var binding = this.deliveryRatesRepository.GetDeliveryLocation(locationId);
                if (binding == null)
                {
                    return ResultBase.BuildNotFound("Привязка не найдена");
                }

                var removedKladr = binding.Kladr;
                var removedStatus = binding.Status;

                // NOTE: Нельзя привязать КЛАДР уже занятый другим.
                var other = this.deliveryRatesRepository.GetDeliveryLocationByPartnerAndKladr(
                    binding.PartnerId,
                    kladr,
                    DeliveryLocationStatus.CorrectBinded.MakeArray(),
                    binding.Id.MakeArray(),
                    true);

                if (other != null)
                {
                    const string MessageFormat = "Код {0} уже привязан к другому населенному пункту {1}";
                    var mess = string.Format(MessageFormat, kladr, other.LocationName);
                    return ResultBase.BuildFail(ResultCodes.KLADR_USED_BY_ANOTHER_Location, mess);
                }

                binding.Kladr = kladr;
                binding.Status = DeliveryLocationStatus.CorrectBinded;
                binding.UpdateUserId = userId;
                binding.UpdateDateTime = DateTime.Now;
                binding.UpdateSource = UpdateSources.Arm;

                this.deliveryRatesRepository.SaveDeliveryLocation(binding);

                if (removedStatus == DeliveryLocationStatus.KladrDuplication)
                {
                    this.ChangeStatusOtherDublicatedDeliveryLocation(binding.PartnerId, removedKladr, userId);
                }

                return ResultBase.BuildSuccess();
            }
            catch (Exception e)
            {
                log.Error("Ошибка изменения привязки привязки", e);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(e);
            }
        }

        public ResultBase ResetDeliveryLocation(int locationId, string userId)
        {
            try
            {
                userId.ThrowIfNull("userId");
                securityChecker.CheckPermissions(userId, ArmPermissions.PartnersDeliveryMatrix);

                var binding = this.deliveryRatesRepository.GetDeliveryLocation(locationId);
                if (binding == null)
                {
                    return ResultBase.BuildNotFound("Привязка не найдена");
                }

                var removedKladr = binding.Kladr;
                var removedStatus = binding.Status;

                binding.Kladr = null;
                binding.Status = DeliveryLocationStatus.NotBinded;
                binding.UpdateUserId = userId;
                binding.UpdateDateTime = DateTime.Now;
                binding.UpdateSource = UpdateSources.Arm;

                this.deliveryRatesRepository.SaveDeliveryLocation(binding);

                if (removedStatus == DeliveryLocationStatus.KladrDuplication)
                {
                    this.ChangeStatusOtherDublicatedDeliveryLocation(binding.PartnerId, removedKladr, userId);
                }

                return ResultBase.BuildSuccess();
            }
            catch (Exception e)
            {
                log.Error("Ошибка сброса привязки привязки", e);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(e);
            }
        }

        public DeliveryLocationHistoryResult GetDeliveryLocationHistory(GetDeliveryLocationHistoryParameters parameters, string userId)
        {
            try
            {
                userId.ThrowIfNull("userId");

                securityChecker.CheckPermissions(userId, ArmPermissions.PartnersDeliveryMatrix);

                var inner = parameters ?? new GetDeliveryLocationHistoryParameters();

                var countToTake = inner.CountToTake.NormalizeByHeight(ApiSettings.MaxResultsCountDeliveryBindingHistory);

                var bindings = this.deliveryRatesRepository.GetDeliveryLocationHistory(
                    inner.LocationId, inner.PartnerId, inner.CountToSkip, countToTake, inner.CalcTotalCount);

                return DeliveryLocationHistoryResult.BuildSuccess(bindings, bindings.TotalCount);
            }
            catch (Exception e)
            {
                log.Error("Ошибка получения истории привязок", e);
                return ServiceOperationResult.BuildErrorResult<DeliveryLocationHistoryResult>(e);
            }
        }

        public ResultBase SaveProductViewsForDay(DateTime date, KeyValuePair<string, int>[] views, string userId)
        {
            try
            {
                views.ThrowIfNull("views");

                securityChecker.CheckPermissions(userId, ArmPermissions.Products);

                productViewsByDayRepository.SaveViews(date, views);

                return ResultBase.BuildSuccess();
            }
            catch (Exception ex)
            {
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }

        #endregion

        private void ChangeStatusOtherDublicatedDeliveryLocation(int partnerId, string removedKladr, string userId)
        {
            /* NOTE: Есть только одна локация с этим же КЛАДР и при этом ее статус DeliveryLocationStatus.KladrDuplication, 
             * то переводим ее в статус OK */
            var others = this.deliveryRatesRepository.GetDeliveryLocationsByPartnerAndKladr(
                partnerId, removedKladr, 0, 2);
            if (others != null && others.Count == 1)
            {
                var other = others.First();
                if (other.Status == DeliveryLocationStatus.KladrDuplication)
                {
                    other.Status = DeliveryLocationStatus.CorrectBinded;
                    other.UpdateUserId = userId;
                    other.UpdateDateTime = DateTime.Now;
                    other.UpdateSource = UpdateSources.Arm;

                    this.deliveryRatesRepository.SaveDeliveryLocation(other);
                }
            }
        }

        private PartnerCommitOrdersResult PartnerCommitOrderInternal(int partnerId, IEnumerable<PartnerOrderCommitment> partnerOrderCommitment)
        {
            if (partnerOrderCommitment == null)
            {
                throw new ArgumentNullException("partnerOrderCommitment");
            }

            var orderStatuses = new List<ExternalOrdersStatus>();

            foreach (var orderCommitment in partnerOrderCommitment)
            {
                if (!this.ordersRepository.Exists(orderCommitment.OrderId, orderCommitment.ExternalOrderId))
                {
                    var message = string.Format(
                        "Заказ с id '{0}' или ExternalId '{1}' не найден",
                        orderCommitment.OrderId,
                        orderCommitment.ExternalOrderId);

                    return PartnerCommitOrdersResult.BuildFail(ResultCodes.NOT_FOUND, message);
                }

                var exists = this.ordersRepository.Get(partnerId, orderCommitment.ExternalOrderId);
                if (exists != null && exists.Id != orderCommitment.OrderId)
                {
                    var message =
                        string.Format(
                            "Идентификатор заказа \"{0}\" (заказ id={1}) в системе поставщика/партнера должен быть уникален в рамках парнера, тем не менее в БД найден заказ id={2} c идентификатором заказа \"{3}\" в системе поставщика/партнера.",
                            orderCommitment.ExternalOrderId,
                            orderCommitment.OrderId,
                            exists.Id,
                            exists.ExternalOrderId);
                    log.ErrorFormat(message);

                    this.SendErrorByEmail(partnerId, message);

                    return PartnerCommitOrdersResult.BuildFail(ResultCodes.NOT_UNIQUE_EXTERNAL_ORDER_ID, message);
                }

                var existsInBatch = orderStatuses.FirstOrDefault(x => x.ExternalOrderId == orderCommitment.ExternalOrderId);

                if (existsInBatch != null)
                {
                    var message =
                        string.Format(
                            "Идентификатор заказа \"{0}\" (заказ id={1}) в системе поставщика/партнера должен быть уникален в рамках парнера, тем не менее в пакете подтверждений найден заказ id={2} c идентификатором заказа \"{3}\" в системе поставщика/партнера.",
                            orderCommitment.ExternalOrderId,
                            orderCommitment.OrderId,
                            existsInBatch.OrderId,
                            existsInBatch.ExternalOrderId);
                    log.ErrorFormat(message);

                    this.SendErrorByEmail(partnerId, message);

                    return PartnerCommitOrdersResult.BuildFail(ResultCodes.NOT_UNIQUE_EXTERNAL_ORDER_ID, message);
                }

                var orderStatus = new ExternalOrdersStatus
                {
                    OrderId = orderCommitment.OrderId,
                    ClientId = null,
                    ExternalOrderId = orderCommitment.ExternalOrderId,
                    ExternalOrderStatusCode = orderCommitment.ReasonCode,
                    OrderStatusDescription = orderCommitment.Reason,
                    ExternalOrderStatusDateTime = DateTime.Now,
                    OrderStatus = orderCommitment.IsConfirmed
                                      ? OrderStatuses.DeliveryWaiting
                                      : OrderStatuses.CancelledByPartner
                };

                orderStatuses.Add(orderStatus);
            }

            var results = UpdateExternalOrdersStatuses(orderStatuses.ToArray(), ApiSettings.ClientSiteUserName);

            var firstInvalid = results.FirstOrDefault(x => x.ResultCode != ResultCodes.SUCCESS);

            return firstInvalid != null
                ? PartnerCommitOrdersResult.BuildFail(firstInvalid.ResultCode, firstInvalid.ResultDescription)
                : PartnerCommitOrdersResult.BuildSuccess(results);
        }

        private void SendErrorByEmail(int partnerId, string body)
        {
            var partner = this.partnerRepository.GetById(partnerId);

            if (partner == null)
            {
                var message =
                    string.Format(
                        "Не возможно отправить письмо с ошибкой партнеру, партнер с ид {0} не найден", partnerId);
                log.ErrorFormat(message);

                throw new OperationException(message);
            }

            var partnerRecipients = partner.Settings.GetReportRecipients(string.Empty);
            var internalRecipients = ConfigurationManager.AppSettings["reportRecipients"] ?? string.Empty;

            log.InfoFormat("Отправка отчета, получатели партнера: {0}, внутрение получатели: {1}", partnerRecipients, internalRecipients);

            var separator = new[] { ';' };

            var recipients =
                partnerRecipients.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                                 .Union(internalRecipients.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                                 .Where(x => x != null)
                                 .Select(x => x.Trim())
                                 .Where(x => x.Length > 0)
                                 .ToArray();

            if (recipients.Length == 0)
            {
                return;
            }

            var subject = string.Format("Уведомление о нарушении уникальности номера заказа в системе поставщика/партнера \"{0}\"", partner.Name);

            logEmailSender.SendMail(subject, recipients, body);
        }

        private ResultBase ValidateCategoriesExists(ICollection<int> ids)
        {
            var categories = this.productCategoriesDataSource.GetProductCategoriesByIds(ids);

            if (ids.Count == categories.Length)
            {
                return ResultBase.BuildSuccess();
            }

            var notExists = ids.Except(categories.Select(x => x.Id));

            var error = string.Format(
                "Категории с идентификатором(ами) {0} не существует(ют)", string.Join(", ", notExists));
            return ResultBase.BuildFail(ResultCodes.CATEGORY_NOT_FOUND, error);
        }

        private PartnerProductCategoryLink SetPartnerProductCateroryLinkInternal(PartnerProductCategoryLink inputLink, string userId)
        {
            var links = this.partnerProductCateroryLinkRepository.SetPartnerProductCateroryLink(inputLink, userId);

            if (links == null || links.Count == 0)
            {
                return null;
            }

            var link = links.First();

            var paths = links.Select(l => new CategoryPath(l.IncludeSubcategories, l.NamePath)).ToArray();

            return new PartnerProductCategoryLink
            {
                PartnerId = link.PartnerId,
                ProductCategoryId = link.ProductCategoryId,
                Paths = paths
            };
        }

        private ProductCategory[] GetSubCategories(int categoryId, int? nestingLevel = null)
        {
            /* ReSharper disable RedundantArgumentName */
            var result = this.productCategoriesDataSource.AdminGetCategories(
                categoriesStatus: null,
                parentId: categoryId,
                nestingLevel: nestingLevel,
                countToTake: int.MaxValue,
                countToSkip: 0,
                calcTotalCount: false);
            /* ReSharper restore RedundantArgumentName */

            return result.Categories;
        }

        private ProductCategory[] GetRootCategories()
        {
            /* ReSharper disable RedundantArgumentName */
            var result = this.productCategoriesDataSource.AdminGetCategories(
                    categoriesStatus: ProductCategoryStatuses.Active,
                    parentId: null,
                    nestingLevel: 1,
                    countToTake: int.MaxValue,
                    countToSkip: 0,
                    calcTotalCount: false,
                    includeParent: true);
            /* ReSharper restore RedundantArgumentName */

            if (result == null || result.Categories == null)
            {
                const string Message = "Не удалось выполнить поиск корневых категорий";
                throw new OperationException(Message) { ResultCode = ResultCodes.UNKNOWN_ERROR };
            }

            return result.Categories;
        }

        private CategoryPath[] ToCaterogyPaths(IEnumerable<Entities.PartnerProductCategoryLink> list)
        {
            return list.Select(y => new CategoryPath(y.IncludeSubcategories, y.NamePath)).ToArray();
        }

        private void CheckKladr(string kladr)
        {
            kladr.ThrowIfNull("kladr");

            if (kladr.Length != 13)
            {
                throw new OperationException(ResultCodes.INVALID_DELIVARY_KLADR, "Код КЛАДР должен содержать 13 цифр");
            }

            if (!kladr.All(char.IsDigit))
            {
                throw new OperationException(ResultCodes.INVALID_DELIVARY_KLADR, "Код КЛАДР должен содержать 13 цифр");
            }

            if (!this.geoPointProvider.IsKladrCodeExists(kladr))
            {
                throw new OperationException(ResultCodes.KLADR_NOT_FOUND_BY_GEOBASE, "Код КЛАДР не найден в геобазе, код ошибочен");
            }
        }

        private Partner[] InternalGetPartners(int[] ids)
        {
            if (ids == null || ids.Count() == 0)
            {
                return this.partnerRepository.GetAllPartners();
            }

            return this.partnerRepository.GetByIds(ids);
        }

        private ChangeOrderStatusResult[] UpdateOrdersStatuses(OrdersStatus[] ordersStatuses, string userId)
        {
            var results = ordersStatusChanger.UpdateOrdersStatuses(ordersStatuses, userId);

            return results;
        }

        private ChangeExternalOrderStatusResult[] UpdateExternalOrdersStatuses(ExternalOrdersStatus[] orderStatuses, string userId)
        {
            var results = ordersStatusChanger.UpdateExternalOrdersStatuses(orderStatuses, userId);

            return results;
        }
    }
}