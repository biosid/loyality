namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using API;
    using API.Entities;
    using API.InputParameters;
    using API.OutputResults;

    using Configuration;

    using DataSources;
    using DataSources.Interfaces;
    using DataSources.Repositories;
    using Delivery;
    using Extensions;

    using Interfaces;

    using Logging;
    using Logging.Wcf;

    using Monitoring;

    using PartnersConnector.WsClients.PartnersOrderManagementService;

    using PromoAction.WsClients.MechanicsService;

    using Settings;

    using VTB24.BankConnector.WsClients.BankConnectorService;

    using DeliveryGroup = API.Entities.DeliveryGroup;
    using DeliveryInfo = API.Entities.DeliveryInfo;
    using DeliveryTypes = API.Entities.DeliveryTypes;
    using GetDeliveryVariantsResult = API.OutputResults.GetDeliveryVariantsResult;
    using IOrderManagementService = API.IOrderManagementService;
    using Order = API.Entities.Order;
    using OrderItem = API.Entities.OrderItem;
    using OrderStatuses = API.Entities.OrderStatuses;
    using ResultBase = API.OutputResults.ResultBase;

    [LoggingBehavior]
    public class OrderManagementService : SupportService, IOrderManagementService
    {
        #region Constants

        public const string RussiaCountryCode = "RU";
        private const int ProviderCheckOrderOk = 1;

        #endregion

        #region Fields

        private static readonly int ClearOrderAttemptMinHour = ConfigHelper.ClearOrderAttemptMinHour;
        private static readonly int ClearOrderAttemptMaxHour = ConfigHelper.ClearOrderAttemptMaxHour;

        private readonly ILog log = LogManager.GetLogger(typeof(OrderManagementService));
        
        private readonly IBasketService basketService;
        private readonly IOrdersDataSource ordersDataSource;
        private readonly IPartnerConnectorProvider partnerConnectorProvider;
        private readonly IOrdersRepository ordersRepository;
        private readonly IPartnerRepository partnerRepository;
        private readonly IPriceSpecification priceSpecification;
        private readonly IMechanicsProvider mechanicsProvider;
        private readonly IGeoPointProvider geoPointProvider;

        private readonly OrdersStatusChanger orderStatusChanger;

	    private readonly DeliveryVariantsProviderFactory deliveryVariantsProviderFactory;

        #endregion

        #region Constructors

        public OrderManagementService() : this(null)
        {
        }

        public OrderManagementService(
            IOrdersDataSource ordersDataSource = null,
            IBasketService basketService = null,
            IPartnerConnectorProvider partnerConnectorProvider = null,
            IOrdersRepository ordersRepository = null,
            IPartnerRepository partnerRepository = null,
            IBonusGatewayProvider bonusGatewayProvider = null,
            IAdvancePaymentProvider advancePaymentProvider = null,
            IDeliveryVariantsProvider deliveryVariantsProvider = null,
            IPriceSpecification priceSpecification = null,
            IMechanicsProvider mechanicsProvider = null, 
            IGeoPointProvider geoPointProvider = null)
        {
            this.geoPointProvider = geoPointProvider ?? new GeoPointProvider();
            this.partnerConnectorProvider = partnerConnectorProvider ?? new PartnerConnectorProvider();
            this.mechanicsProvider = mechanicsProvider ?? new MechanicsProvider();
            this.priceSpecification = priceSpecification ?? new PriceSpecification();
            this.ordersDataSource = ordersDataSource ?? new OrdersDataSource();
            this.basketService = basketService ?? new BasketService();
            this.ordersRepository = ordersRepository ?? new OrdersRepository();
            this.partnerRepository = partnerRepository ?? new PartnerRepository();

			var defaultDeliveryVariantsProvider = deliveryVariantsProvider ?? 
				new DeliveryVariantsProvider(this.partnerConnectorProvider, this.partnerRepository, new DeliveryRatesRepository());
			deliveryVariantsProviderFactory = new DeliveryVariantsProviderFactory(this.partnerRepository, defaultDeliveryVariantsProvider);

            orderStatusChanger = new OrdersStatusChanger(
                this.ordersDataSource,
                this.ordersRepository,
                bonusGatewayProvider ?? new BonusGatewayProvider(),
                advancePaymentProvider ?? new AdvancePaymentProvider());
        }

        #endregion

        #region IOrderManagementService Members

        public GetDeliveryVariantsResult GetDeliveryVariants(GetDeliveryVariantsParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                
                parameters.Location.ThrowIfNull("parameters.Location");
                parameters.BasketItems.ThrowIfNull("parameters.BasketItems");

                var basketItems = GetBasketItems(parameters.BasketItems, parameters.ClientContext);

                if (basketItems == null || basketItems.Length == 0)
                {
                    return new GetDeliveryVariantsResult
                    {
                        ResultCode = ResultCodes.BASKET_ITEM_NOT_FOUND,
                        ResultDescription = "Элемент корзины не найден"
                    };
                }

                var orderItems = CreateOrderItems(basketItems);

				var deliveryVariantsProvider = deliveryVariantsProviderFactory.CreateDeliveryVariantsProvider(orderItems[0].Product.PartnerId, orderItems);

                var deliveryVariants = deliveryVariantsProvider.GetSiteDeliveryVariants(orderItems[0].Product.PartnerId, parameters.Location, orderItems, parameters.ClientId);

                ConvertRurToBonus(parameters.ClientContext, deliveryVariants.DeliveryGroups, orderItems.Select(i => i.Product).ToArray());

                return deliveryVariants;
            }
            catch (Exception e)
            {
                log.Error("Ошибка получения вариантов доставки", e);
                return ServiceOperationResult.BuildErrorResult<GetDeliveryVariantsResult>(e);
            }            
        }

        public CreateOrderResult CreateOrderFromBasketItems(CreateOrderFromBasketItemsParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters is null");
                Utils.CheckArgument(string.IsNullOrEmpty, parameters.ClientId, "parameters.ClientId");
                Utils.CheckArgument(o => o == null, parameters.ClientContext, "parameters.ClientContext");
                Utils.CheckArgument(o => o == null, parameters.BasketItems, "parameters.BasketItems");

                return CreateOrderInternal(parameters.ClientId, parameters.BasketItems, parameters.ClientContext, parameters.Delivery, parameters.TotalAdvance);
            }
            catch (Exception e)
            {
                log.Error("Ошибка создания заказа", e);
                return ServiceOperationResult.BuildErrorResult<CreateOrderResult>(e);
            }
        }

        public CreateOrderResult CreateOnlinePartnerOrder(CreateOrderFromOnlinePartnerParameters parameters)
        {
            try
            {
                Utils.CheckArgument(parameters == null, "parameters");
                Utils.CheckArgument(string.IsNullOrEmpty(parameters.ClientId), "parameters.UserId");
                Utils.CheckArgument(parameters.PartnerId <= 0, "parameters.PartnerId");
                Utils.CheckArgument(string.IsNullOrEmpty(parameters.ExternalOrderId), "parameters.ExternalOrderId");

                var partner = this.partnerRepository.GetById(parameters.PartnerId);

                if (partner.Type != PartnerType.Online)
                {
                    var mess =
                        string.Format(
                            "Партнер с идентификатором {0} не имеет тип {1}. Только партнеры с типом {1} могут создать заказ с помощью CreateOnlinePartnerOrder",
                            parameters.PartnerId,
                            PartnerType.Online);
                    throw new ArgumentException(mess);
                }

                var clientContext = new Dictionary<string, string>();

                clientContext.Add(ClientContextParser.ClientIdKey, parameters.ClientId);

                var res = mechanicsProvider.GetOnlineProductFactors(clientContext);

                foreach (var item in parameters.Items)
                {
                    var itemBonusPrice = PriceSpecification.CalcBonusPrice(res, item.Price);

                    if (itemBonusPrice != item.BonusPrice)
                    {
                        var description = string.Format(
                            "Не верная цена в бонусах price:{0} bonusPrice:{1} но расчитано:{2} по коэффициентам:{3}", 
                            item.Price, 
                            item.BonusPrice, 
                            itemBonusPrice, 
                            GetFactorsString(res));

                        throw new OperationException(
                            ResultCodes.PRODUCT_WRONG_BONUS_PRICE, 
                            description);
                    }
                }

                var order = BuildOnlineOrder(parameters);

                var result = CreateOrderResult.BuildSuccess(order);

                return result;
            }
            catch (Exception e)
            {
                return ServiceOperationResult.BuildErrorResult<CreateOrderResult>(e);
            }
        }

        public CreateOrderResult CreateCustomOrder(CreateCustomOrderParameters parameters)
        {
            try
            {
                if (parameters == null)
                {
                    throw new ArgumentNullException("parameters");
                }

                if (string.IsNullOrEmpty(parameters.ClientId))
                {
                    throw new ArgumentException("parameters.ClientId");
                }

                if (string.IsNullOrWhiteSpace(parameters.ExternalOrderId))
                {
                    throw new ArgumentException("parameters.ExternalOrderId");
                }

                var partner = this.partnerRepository.GetById(parameters.PartnerId);

                if (partner == null)
                {
                    throw new ArgumentException("parameters.PartnerId");
                }

                if (partner.Type != PartnerType.Direct)
                {
                    throw new ArgumentException(string.Format("Партнер {0} должен иметь тип {1}", parameters.PartnerId, PartnerType.Direct));
                }

                var now = DateTime.Now;
                var utcNow = now.ToUniversalTime();

                var orderItems = parameters.Items != null
                                     ? parameters.Items
                                                 .Select(i => new OrderItem
                                                 {
                                                     Amount = i.Amount,
                                                     PriceRur = i.PriceRur,
                                                     AmountPriceRur = i.Amount * i.PriceRur,
                                                     PriceBonus = i.PriceBonus,
                                                     AmountPriceBonus = i.Amount * i.PriceBonus,
                                                     Product = new Product
                                                     {
                                                         ProductId = null,
                                                         PartnerProductId = i.ArticleId,
                                                         Name = i.ArticleName,
                                                         PriceRUR = i.PriceRur,
                                                         Price = i.PriceBonus
                                                     }
                                                 })
                                                 .ToArray()
                                     : new OrderItem[0];

                var costRur = orderItems.Sum(i => i.AmountPriceRur);
                var costBonus = orderItems.Sum(i => i.AmountPriceBonus);

                var order = new Order
                {
                    ClientId = parameters.ClientId,
                    ExternalOrderId = parameters.ExternalOrderId,
                    PartnerId = parameters.PartnerId,
                    CarrierId = parameters.PartnerId,
                    Status = OrderStatuses.Registration,
                    StatusChangedDate = now,
                    StatusUtcChangedDate = utcNow,
                    InsertedDate = now,
                    InsertedUtcDate = utcNow,
                    UpdatedDate = now,
                    UpdatedUtcDate = utcNow,
                    UpdatedUserId = ApiSettings.ClientSiteUserName,
                    TotalWeight = 0,
                    PaymentStatus = OrderPaymentStatuses.No,
                    DeliveryPaymentStatus = OrderDeliveryPaymentStatus.No,
                    Items = orderItems,
                    ItemsCost = costRur,
                    BonusItemsCost = costBonus,
                    TotalCost = costRur,
                    BonusTotalCost = costBonus
                };

                var orderId = ordersDataSource.Insert(order);

                order.Id = orderId;

                var result = CreateOrderResult.BuildSuccess(order);

                return result;
            }
            catch (Exception e)
            {
                return ServiceOperationResult.BuildErrorResult<CreateOrderResult>(e);
            }
        }

        public ClientCommitOrderResult ClientCommitOrder(string clientId, int orderId)
        {
            try
            {
                return ClientCommitOrderInternal(clientId, orderId);
            }
            catch (Exception e)
            {
                log.Error("Ошибка подтверждения заказа", e);
                return ServiceOperationResult.BuildErrorResult<ClientCommitOrderResult>(e);
            }
        }

        public GetOrdersHistoryResult GetOrdersHistory(GetOrdersHistoryParameters historyParameters)
        {
            try
            {
                return SearchOrdersInternal(historyParameters);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения списка заказов пользователя/клиента", ex);
                return ServiceOperationResult.BuildErrorResult<GetOrdersHistoryResult>(ex);
            }
        }

        public HasNonterminatedOrdersResult HasNonterminatedOrders(string clientId)
        {
            try
            {
                var has = this.ordersRepository.HasNonterminatedOrders(clientId);
                return HasNonterminatedOrdersResult.BuildSuccess(has);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка проверки наличия заказов в нетерминальных статусах", ex);
                return ServiceOperationResult.BuildErrorResult<HasNonterminatedOrdersResult>(ex);
            }
        }

        public GetOrdersHistoryResult GetOrdersForPayment(GetOrdersForPaymentParameters parameters)
        {
            try
            {
                var page = ordersRepository.GetForPayment(parameters.CountToSkip, parameters.CountToTake);

                if (page == null || page.Count == 0)
                {
                    log.Debug("Найдено 0 заказов пользователя/клиента");
                    return new GetOrdersHistoryResult
                    {
                        ResultCode = ResultCodes.SUCCESS,
                        ResultDescription = "По заданным параметрам заказы не найдены",
                        Orders = new Order[0],
                        TotalCount = 0
                    };
                }

                return new GetOrdersHistoryResult
                {
                    ResultCode = ResultCodes.SUCCESS,
                    ResultDescription = null,
                    Orders = page.ToArray(),
                    TotalCount = page.TotalCount
                };
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения списка заказов пользователя/клиента", ex);
                return ServiceOperationResult.BuildErrorResult<GetOrdersHistoryResult>(ex);
            }
        }

        public GetOrderResult GetOrderById(int orderId, string clientId)
        {
            if (orderId <= 0)
            {
                var mess = string.Format("Не корректное значение идентификатора заказа {0}", orderId);

                return ServiceOperationResult.BuildErrorResult<GetOrderResult>(new ArgumentException(mess));
            }

            try
            {
                var order = this.ordersDataSource.GetOrder(orderId, clientId);

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

        public GetOrderResult GetOrderByExternalId(GetOrderByExternalIdParameters parameters)
        {
            try
            {
                Utils.CheckArgument(string.IsNullOrEmpty(parameters.ExternalOrderId), "ExternalOrderId is null or empty");

                var order = this.ordersDataSource.GetOrderByExternalId(parameters.ExternalOrderId, parameters.ClientId);

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

        public GetOrderPaymentStatusesResult GetOrderPaymentStatuses(int[] orderIds)
        {
            try
            {
                var orders = this.ordersRepository.GetOrders(orderIds);
                return GetOrderPaymentStatusesResult.BuildSuccess(orders);
            }
            catch (Exception e)
            {
                log.Error("Ошибка в GetOrderPaymentStatuses", e);
                return ServiceOperationResult.BuildErrorResult<GetOrderPaymentStatusesResult>(e);
            }
        }

        public GetLastDeliveryAddressesResult GetLastDeliveryAddresses(string clientId, bool excludeDeliveryAddressesWithoutKladr, int? countToTake)
        {
            try
            {
                if (string.IsNullOrEmpty(clientId))
                {
                    return ServiceOperationResult.BuildErrorResult<GetLastDeliveryAddressesResult>(new ArgumentNullException("clientId"));
                }

                countToTake = countToTake.NormalizeByHeight(ApiSettings.MaxResultsCountOrders);

                var addresses = this.ordersDataSource.GetLastDeliveryAddresses(clientId, excludeDeliveryAddressesWithoutKladr, countToTake);

                return addresses.Length > 0
                           ? new GetLastDeliveryAddressesResult
                           {
                               ResultCode = ResultCodes.SUCCESS,
                               Addresses = addresses
                           }
                           : new GetLastDeliveryAddressesResult
                           {
                               ResultCode = ResultCodes.SUCCESS,
                               ResultDescription = "Адреса доставки не найдены"
                           };
            }
            catch (Exception e)
            {
                log.Error("Ошибка в GetLastDeliveryAddresses", e);
                return ServiceOperationResult.BuildErrorResult<GetLastDeliveryAddressesResult>(e);
            }
        }

        public ChangeExternalOrderStatusResult ChangeExternalOrderStatus(ExternalOrdersStatus externalOrderStatus)
        {
            var asArray = new[]
                          {
                              externalOrderStatus
                          };

            var results = this.ChangeExternalOrdersStatuses(asArray);

            if (results.Success)
            {
                return results.ChangeExternalOrderStatusResults.Single();
            } 
            else
            {
                return ChangeExternalOrderStatusResult.BuildFail(results.ResultCode, results.ResultDescription);
            }
        }

        public ChangeExternalOrdersStatusesResult ChangeExternalOrdersStatuses(ExternalOrdersStatus[] externalOrdersStatuses)
        {
            ChangeExternalOrderStatusResult[] results;
            try
            {
                Utils.CheckArgument(o => o == null || !o.Any(), externalOrdersStatuses, "ordersStatuses");

                results = orderStatusChanger.UpdateExternalOrdersStatuses(externalOrdersStatuses, ApiSettings.ClientSiteUserName);
            }
            catch (InvalidOperationException e)
            {
                log.Error("Ошибка обработки обновления статуса заказа", e);
                var res = ServiceOperationResult.BuildErrorResult<ChangeExternalOrdersStatusesResult>(e);
                res.ResultCode = ResultCodes.INVALID_PARAMETER_VALUE;
                return res;
            }
            catch (Exception e)
            {
                log.Error("Ошибка обработки обновления статуса заказа", e);
                return ServiceOperationResult.BuildErrorResult<ChangeExternalOrdersStatusesResult>(e);
            }

            return new ChangeExternalOrdersStatusesResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           ChangeExternalOrderStatusResults = results
                       };
        }

        public ChangeOrdersStatusesResult ChangeOrdersStatuses(OrdersStatus[] ordersStatuses)
        {
            try
            {
                Utils.CheckArgument(o => o == null || !o.Any(), ordersStatuses, "ordersStatuses");

                var results = orderStatusChanger.UpdateOrdersStatuses(ordersStatuses, ApiSettings.ClientSiteUserName);

                return ChangeOrdersStatusesResult.BuildSuccess(results);
            }
            catch (Exception e)
            {
                log.Error("Ошибка обработки обновления статуса заказа", e);
                return ServiceOperationResult.BuildErrorResult<ChangeOrdersStatusesResult>(e);
            }
        }

        public ResultBase ChangeOrderStatusDescription(int orderId,  string orderStatusDescription)
        {
            try
            {
                Utils.CheckArgument(string.IsNullOrEmpty(orderStatusDescription), "orderStatusDescription is null or empty");
                
                var order = ordersRepository.GetById(orderId);

                if (order == null)
                {
                    var mess = string.Format("Order with id - {0} not found", orderId);
                    return ResultBase.BuildNotFound(mess);
                }

                ordersRepository.UpdateOrderStatusDescription(orderId, orderStatusDescription, ApiSettings.ClientSiteUserName);

                return ResultBase.BuildSuccess();
            }
            catch (Exception e)
            {
                log.Error("Ошибка обновления описания статуса заказа", e);
                return ServiceOperationResult.BuildErrorResult<ChangeOrdersStatusesResult>(e);
            }
        }

        public ChangeOrdersStatusesResult ChangeOrdersStatusesBeforePayment()
        {
            try
            {
                bool settingValue;

                var skipPartnerIds =
                    partnerRepository.GetSettingsByKey(PartnerSettingsExtension.DisableAutoOrderStatusChangeKey)
                                     .Where(s => s.Value != null && bool.TryParse(s.Value, out settingValue) && settingValue)
                                     .Select(s => s.Key)
                                     .ToArray();

                var ids = this.ordersRepository.GetIds(OrderStatuses.Processing, OrderPaymentStatuses.No, PartnerType.Direct, skipPartnerIds);

                var ordersStatuses = ids.Select(
                    x => new OrdersStatus
                         {
                             OrderId = x,
                             OrderStatus = OrderStatuses.DeliveryWaiting
                         }).ToArray();

                var results = orderStatusChanger.UpdateOrdersStatuses(ordersStatuses, ApiSettings.ClientSiteUserName);

                return ChangeOrdersStatusesResult.BuildSuccess(results);
            }
            catch (Exception e)
            {
                log.Error("Ошибка перевода заказов direct партнера перед оплатой", e);
                return ServiceOperationResult.BuildErrorResult<ChangeOrdersStatusesResult>(e);
            }
        }

        public ChangeOrdersStatusesResult ChangeOrdersPaymentStatuses(OrdersPaymentStatus[] statuses)
        {
            try
            {
                Utils.CheckArgument(o => o == null || !o.Any(), statuses, "statuses");

                ChangeOrderStatusResult[] results = ordersDataSource.UpdateOrdersPaymentStatuses(statuses, ApiSettings.ClientSiteUserName);
                return ChangeOrdersStatusesResult.BuildSuccess(results);
            }
            catch (Exception e)
            {
                log.Error("Ошибка обработки обновления статуса заказа", e);
                return ServiceOperationResult.BuildErrorResult<ChangeOrdersStatusesResult>(e);
            }
        }

        public ChangeOrdersStatusesResult ChangeOrdersDeliveryStatuses(OrdersDeliveryStatus[] statuses)
        {
            try
            {
                Utils.CheckArgument(o => o == null || !o.Any(), statuses, "statuses");

                ChangeOrderStatusResult[] results = ordersDataSource.UpdateOrdersDeliveryStatuses(statuses, ApiSettings.ClientSiteUserName);
                return ChangeOrdersStatusesResult.BuildSuccess(results);
            }
            catch (Exception e)
            {
                log.Error("Ошибка обработки обновления статуса заказа", e);
                return ServiceOperationResult.BuildErrorResult<ChangeOrdersStatusesResult>(e);
            }
        }
        
        #endregion

        #region Methods

        private CreateOrderResult CreateOrderInternal(string clientId, Guid[] basketItemIds, Dictionary<string, string> clientContext, DeliveryDto delivery, decimal totalAdvance)
        {
            delivery.ThrowIfNull("delivery");
            delivery.DeliveryVariantLocation.ThrowIfNull("delivery.DeliveryVariantLocation");

            var basketItems = this.GetBasketItems(basketItemIds, clientContext);

            var validateItemsResult = ValidateBasketItems(basketItems);

            if (validateItemsResult != null)
            {
                return validateItemsResult;
            }

            ValidateAvailability(delivery.DeliveryVariantLocation.KladrCode, basketItems);

            var orderItems = CreateOrderItems(basketItems);

            var variants = GetPartnerDeliveryVariants(clientId, delivery, orderItems);

            var deliveryTypeSpec = GetDeliveryTypeSpecification(delivery.DeliveryType, variants);

            var deliveryCost = deliveryTypeSpec.GetDeliveryCost(delivery.ExternalDeliveryVariantId, delivery.PickupPoint.Maybe(p => p.ExternalPickupPointId));
            
            Utils.CheckArgument(totalAdvance > 0 && deliveryCost > totalAdvance, "deliveryCost"); // оплатить картой можно не менее стоимости доставки
            
            decimal bonusDeliveryCost = 0, deliveryAdvance = 0;
            
            if (totalAdvance > 0)
            {
                deliveryAdvance = deliveryCost;
            }
            else
            {
                bonusDeliveryCost = GetDeliveryPrice(deliveryCost, orderItems.Select(i => i.Product).ToArray(), clientContext).Bonus;
            }

            var productPrices = orderItems.Select(i => new Price()
                {
                    Bonus = i.Product.Price,
                    Rur = i.Product.PriceRUR
                }).ToArray();
            
            var orderPrices = CreateOrderPrice(orderItems.Select(i => i.Product).ToArray(), orderItems.Select(i => i.FixedPrice).ToArray(), productPrices, clientContext);

            var deliveryInfo = deliveryTypeSpec.BuildDeliveryInfo(delivery, orderItems[0].Product.PartnerId);            

            var order = this.BuildDraftOrder(clientId, deliveryInfo, orderPrices, orderItems, variants.CarrierId, deliveryCost, bonusDeliveryCost, deliveryAdvance, totalAdvance - deliveryAdvance);

            order.Id = ordersDataSource.Insert(order);

            CheckOrder(clientId, order);

            order = this.ordersDataSource.GetOrder(order.Id);
            
            return CreateOrderResult.BuildSuccess(order);   
        }

        private PartnerDeliveryVariants GetPartnerDeliveryVariants(string clientId, DeliveryDto delivery, OrderItem[] orderItems)
        {
			var deliveryVariantsProvider = deliveryVariantsProviderFactory.CreateDeliveryVariantsProvider(orderItems[0].Product.PartnerId, orderItems);

            var variants = deliveryVariantsProvider.GetPartnerDeliveryVariants(orderItems[0].Product.PartnerId, delivery.DeliveryVariantLocation, orderItems, clientId);

            if (!variants.Variants.Success)
            {
                throw new OperationException(ResultCodes.UNKNOWN_ERROR, string.Format("Варианты доставки получить не удалось ResultCode:{0} ResultDescription:{1}", variants.Variants.ResultCode, variants.Variants.ResultDescription));
            }

            return variants;
        }

        private Price GetDeliveryPrice(decimal deliveryPriceRur, Product[] products, Dictionary<string, string> clientContext)
        {
            var deliveryPrice = mechanicsProvider.CalculateDeliveryPrice(clientContext, deliveryPriceRur, products.First().PartnerId);

            return new Price()
                {
                    Rur = deliveryPriceRur,
                    Bonus = deliveryPrice.Price.Bonus.Round()
                };
        }

        private IDeliveryTypeSpec GetDeliveryTypeSpecification(DeliveryTypes deliveryType, PartnerDeliveryVariants variants)
        {
	        switch (deliveryType)
	        {
		        case DeliveryTypes.Pickup:
		        {
			        return new PickupSpec(variants, partnerRepository);
		        }
		        case DeliveryTypes.Delivery:
		        {
			        return new DeliverySpec(variants, geoPointProvider, partnerRepository);
		        }
				case DeliveryTypes.Email:
		        {
                    return new EmailSpec();
		        }
				default:
		        {
					throw new NotSupportedException(string.Format("DeliveryType {0} not supported", deliveryType));
		        }
	        }
        }

	    private OrderItem[] CreateOrderItems(BasketItem[] basketItems)
        {
            return basketItems.Select(b =>
                                      new OrderItem
                                          {
                                              Amount = b.ProductsQuantity,
                                              Product = b.Product,
                                              BasketItemId = b.Id.ToString(),
                                              FixedPrice = GetFixedPrice(b.FixedPrice)
                                          }).ToArray();
        }

        private CreateOrderResult ValidateBasketItems(BasketItem[] basketItems)
        {
            if (basketItems == null || basketItems.Length == 0 || basketItems.Any(b => b == null))
            {
                return new CreateOrderResult
                {
                    ResultCode = ResultCodes.BASKET_ITEM_NOT_FOUND,
                    ResultDescription = "Элемент корзины не найден"
                };
            }

            if (basketItems.Any(b => b.ClientId != basketItems[0].ClientId))
            {
                return new CreateOrderResult
                {
                    ResultCode = ResultCodes.BASKET_ITEMS_FROM_DIFFERENT_CLIENTS,
                    ResultDescription = "Элементы корзины от разных клиентов"
                };
            }

            if (basketItems.Any(b => b.BasketItemGroupId != basketItems[0].BasketItemGroupId))
            {
                return new CreateOrderResult
                {
                    ResultCode = ResultCodes.BASKET_ITEMS_FROM_DIFFERENT_PARTNERS,
                    ResultDescription = "Элементы корзины от разных партнеров"
                };
            }

            var emptyProductItem = basketItems.FirstOrDefault(b => b.Product == null);
            if (emptyProductItem != null)
            {
                return new CreateOrderResult
                {
                    ResultCode = ResultCodes.NOT_FOUND,
                    ResultDescription = string.Format("Товар в элементе корзины {0} не заполнен", emptyProductItem.Id)
                };
            }

	        if (basketItems.Any(b => b.Product.IsDeliveredByEmail))
	        {
		        if (basketItems.Any(b => !b.Product.IsDeliveredByEmail))
		        {
					return new CreateOrderResult
					{
						ResultCode = ResultCodes.BASKET_ITEMS_WITH_EMAIL_AND_ANOTHER_DELIVERY_TYPE,
						ResultDescription = "Покрайней мере один из элементов корзины доставляется по E-mail, при этом не все элементы доставляются по E-mail"
					};
		        }
	        }

            return null;
        }

        private void ValidateAvailability(string kladrCode, BasketItem[] basketItems)
        {
            foreach (var basketItem in basketItems)
            {
                switch (basketItem.AvailabilityStatus)
                {
                    case ProductAvailabilityStatuses.DeliveryRateNotFound:
                        {
                            if (kladrCode != null)
                            {
                                var error = string.Format(
                                    "Товар id:{0} не доставляется КЛАДР:{1}", basketItem.ProductId, kladrCode);
                                throw new OperationException(ResultCodes.PRODUCT_NOT_DELIVERED, error);
                            }

                            break;
                        }

                    default:
                        {
                            if (basketItem.AvailabilityStatus != ProductAvailabilityStatuses.Available)
                            {
                                var error = string.Format(
                                    "Товар id:{0} имеет не допустимый статус {1}", basketItem.ProductId, basketItem.AvailabilityStatus);
                                throw new OperationException(ResultCodes.PRODUCT_INVALID_STATUS, error);
                            }

                            break;
                        }
                }
            }
        }

        private BasketItem[] GetBasketItems(Guid[] basketItemIds, Dictionary<string, string> clientContext)
        {
            if (basketItemIds.Length == 0)
            {
                throw new ArgumentException("basketItems.Length == 0");
            }

            if (basketItemIds.Any(b => b == Guid.Empty))
            {
                throw new Exception("BasketItemId is Guid.Empty");
            }

            var res = basketService.GetBasketItems(basketItemIds, clientContext);

            return res.Success ? res.Items : null;
        }

        private FixedPrice GetFixedPrice(string fixedPriceXml)
        {
            return string.IsNullOrEmpty(fixedPriceXml) ? null : XmlSerializer.Deserialize<FixedPrice>(fixedPriceXml);
        }

        private OrderItemPrice[] CreateOrderPrice(Product[] products, FixedPrice[] fixedPrices, Price[] productPrices, Dictionary<string, string> clientContext)
        {
            var orderItemPrices = new List<OrderItemPrice>();

            for (var i = 0; i < products.Length; i++)
            {
                Price productPricesValue;
                if (fixedPrices[i] == null)
                {
                    productPricesValue = productPrices[i];
                }
                else
                {
                    var actualPrice = mechanicsProvider.CalculateProductPrice(clientContext, fixedPrices[i].PriceRUR, products[i]);
                    productPricesValue = new Price()
                    {
                        Bonus = actualPrice.PromoResult.Round(),
                        Rur = fixedPrices[i].PriceRUR
                    };
                }

                orderItemPrices.Add(new OrderItemPrice()
                    {
                        ProductId = products[i].ProductId,
                        ProductPrice = productPricesValue.Bonus,
                        ProductPriceRur = productPricesValue.Rur
                    });
            }

            return orderItemPrices.ToArray();
        }

        private Order BuildDraftOrder(string clientId, DeliveryInfo delivery, OrderItemPrice[] orderPrices, OrderItem[] orderItems, int? carrierId, decimal deliveryCost, decimal bonusDeliveryCost, decimal deliveryAdvance, decimal itemsAdvance)
        {
            Utils.CheckArgument(o => o == null, delivery.Contact, "delivery.Contact");
            Utils.CheckArgument(o => string.IsNullOrEmpty(o.FirstName), delivery.Contact, "delivery.Contact.FirstName");
            Utils.CheckArgument(o => string.IsNullOrEmpty(o.LastName), delivery.Contact, "delivery.Contact.LastName");
            Utils.CheckArgument(c => c.Phone == null || string.IsNullOrEmpty(c.Phone.LocalNumber) || !c.Phone.IsWellFormed(), delivery.Contact, "delivery.Contact.Phone");

            var order = new Order
            {
                ClientId = clientId,
                Status = OrderStatuses.Draft,
                UpdatedUserId = ApiSettings.ClientSiteUserName,
                DeliveryInfo = delivery,
                PartnerId = orderItems[0].Product.PartnerId,
                CarrierId = carrierId,
                Items = orderItems,
                TotalWeight = orderItems.Sum(i => (i.Product.Weight ?? 0) * i.Amount)
            };

            priceSpecification.FillOrderPrice(order, orderPrices, deliveryCost, bonusDeliveryCost, deliveryAdvance, itemsAdvance);

            return order;
        }

        private void CheckOrder(string clientId, Order order)
        {
            var partner = partnerRepository.GetActivePartner(order.PartnerId);

            if (partner.Settings.GetCheckOrderSupported())
            {
                var res = this.partnerConnectorProvider.CheckOrder(order);

                if (!res.Success)
                {
                    var message = "Ошибка проверки заказа " + GetCheckOrderResultText(res);
                    throw new OperationException(message, ResultCodes.UNKNOWN_ERROR);
                }

                if (res.Checked == ProviderCheckOrderOk)
                {
                    // переводим подтвержденный заказ в статус Registration
                    orderStatusChanger.UpdateExternalOrderStatus(
                        new ExternalOrdersStatus
                        {
                            OrderId = order.Id,
                            ClientId = clientId,
                            OrderStatus = OrderStatuses.Registration,
                            ExternalOrderStatusCode = res.ReasonCode,
                            OrderStatusDescription = res.Reason,
                            ExternalOrderStatusDateTime = DateTime.Now
                        },
                        ApiSettings.ClientSiteUserName);
                }
                else
                {
                    var description = "Партнёр отказал при проверке заказа " + GetCheckOrderResultText(res);
                    throw new OperationException(ResultCodes.PROVIDER_CHECK_ORDER_DECLINED, description);
                }
            }
            else
            {
                // обновляем статус проверенного заказа
                orderStatusChanger.UpdateExternalOrderStatus(
                    new ExternalOrdersStatus
                    {
                        OrderId = order.Id,
                        ClientId = clientId,
                        OrderStatus = OrderStatuses.Registration,
                        ExternalOrderStatusCode = string.Empty,
                        OrderStatusDescription = string.Empty,
                        ExternalOrderStatusDateTime = DateTime.Now
                    },
                    ApiSettings.ClientSiteUserName);
            }
        }

        private string GetCheckOrderResultText(CheckOrderResult res)
        {
            return string.Format(
                "Checked:{0} Reason:{1} ReasonCode:{2} ResultCode:{3} ResultDescription:{4}", 
                res.Checked,
                res.Reason,
                res.ReasonCode,
                res.ResultCode,
                res.ResultDescription);
        }

        private ClientCommitOrderResult ClientCommitOrderInternal(string clientId, int orderId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (orderId == 0)
            {
                throw new ArgumentException("orderId is zero");
            }

            var order = ordersDataSource.GetOrder(orderId, clientId);

            if (order == null)
            {
                var mess = string.Format("Заказ с идентификатором {0} клиента {1} не найден", orderId, clientId);
                throw new OperationException(ResultCodes.NOT_FOUND, mess);
            }

            var partner = this.partnerRepository.GetById(order.PartnerId);

            if (partner == null)
            {
                var mess = string.Format("Партнер с идентификатором {0} не найден", order.PartnerId);
                throw new OperationException(ResultCodes.PARTNER_NOT_FOUND, mess);
            }

            ClientCommitOrderResult res;

            switch (partner.Type)
            {
                case PartnerType.Offline:
                    res = StartOrderProcessing(clientId, order);

                    if (res.Success)
                    {
                        order = res.Order;

                        // данная операция может бросить исключение, и тогда заказ останется в статусе Processing
                        // по таким заказам нужен ручной разбор, так как бонусы уже списаны, а что с заказом произошло на стороне партнера - неизвестно
                        res = this.ClientCommitOrderForOfflinePartner(order);
                    }
                    break;

                case PartnerType.Direct:
                    res = StartOrderProcessing(clientId, order);

                    if (res.Success)
                    {
                        order = res.Order;

                        res = this.ClientCommitOrderForDirectPartner(order, partner.Settings);
                    }
                    break;

                case PartnerType.Online:
                    res = this.StartOrderProcessing(clientId, order);
                    break;

                default:
                    throw new NotSupportedException(string.Format("Тип партнера {0} не поддерживается!", partner.Type));
            }

            if (partner.Type == PartnerType.Offline || partner.Type == PartnerType.Direct)
            {
                // NOTE: Удаляем элемент корзины после оформления заказа
                foreach (var orderItem in order.Items.Where(i => i.Product.ProductId != null))
                {
                    this.basketService.Remove(order.ClientId, orderItem.Product.ProductId);
                }   
            }

            ClearOrderAttempt(clientId);

            return res;
        }

        private ClientCommitOrderResult StartOrderProcessing(string clientId, Order order)
        {
            var externalOrdersStatus = new ExternalOrdersStatus
                                       {
                                           OrderId = order.Id,
                                           OrderStatus = OrderStatuses.Processing,
                                           ExternalOrderId = order.ExternalOrderId,
                                           ExternalOrderStatusCode = order.ExternalOrderStatusCode,
                                           OrderStatusDescription = order.OrderStatusDescription,
                                           ExternalOrderStatusDateTime = DateTime.Now
                                       };

            var updateResuls = orderStatusChanger.UpdateExternalOrderStatus(externalOrdersStatus, ApiSettings.ClientSiteUserName);

            if (!updateResuls.Success)
            {
                return ClientCommitOrderResult.BuildFail(updateResuls.ResultCode, updateResuls.ResultDescription);
            }

            var updatedOrder = ordersDataSource.GetOrder(order.Id, clientId);

            return ClientCommitOrderResult.BuildSuccess(updatedOrder);
        }

        private ClientCommitOrderResult ClientCommitOrderForOfflinePartner(Order order)
        {
            // поставить заказ в очередь на подтверждение через коннектор
            var commitRes = this.partnerConnectorProvider.CommitOrder(order);

            if (!commitRes.Success)
            {
                var message = "Ошибка подтверждения заказа " + GetCommitResText(commitRes);
                throw new OperationException(message, ResultCodes.UNKNOWN_ERROR);
            }

            // обновляем статус заказу
            switch (commitRes.Confirmed)
            {
                case ConfirmedStatuses.AddToQueue:
                {
                    order.Status = OrderStatuses.Processing;
                    break;
                }

                case ConfirmedStatuses.Committed:
                {
                    order.Status = OrderStatuses.DeliveryWaiting;
                    break;
                }

                case ConfirmedStatuses.Rejected:
                {
                    order.Status = OrderStatuses.CancelledByPartner;
                    break;
                }

                default:
                {
                    throw new NotSupportedException(
                        string.Format("Статус подтверждения {0} не поддреживается", commitRes.Confirmed));
                }
            }

            // к этому моменту заказ уже находится в статусе Processing
            if (order.Status != OrderStatuses.Processing)
            {
                order.StatusChangedDate = commitRes.Success ? DateTime.Now : order.StatusChangedDate;

                // todo проверять уникальность полученного от провайдера InternalOrderId
                var externalOrdersStatus = new ExternalOrdersStatus
                {
                    OrderId = order.Id,
                    OrderStatus = order.Status,
                    ExternalOrderId = commitRes.InternalOrderId,
                    ExternalOrderStatusCode = order.ExternalOrderStatusCode,
                    OrderStatusDescription = order.OrderStatusDescription,
                    ExternalOrderStatusDateTime = DateTime.Now
                };

                var updateResult = this.ChangeExternalOrderStatus(externalOrdersStatus);

                if (!updateResult.Success)
                {
                    return ClientCommitOrderResult.BuildFail(updateResult.ResultCode, updateResult.ResultDescription);
                }
            }

            var resultCode = order.Status != OrderStatuses.CancelledByPartner
                ? ResultCodes.SUCCESS
                : ResultCodes.PROVIDER_CONFIRM_ORDER_DECLINED;

            var result = ClientCommitOrderResult.BuildSuccess(order, resultCode, GetCommitResText(commitRes));

            return result;
        }

        private ClientCommitOrderResult ClientCommitOrderForDirectPartner(Order order, Dictionary<string, string> partnerSettings)
        {
            string methodName;
            if (partnerSettings == null ||
                !partnerSettings.TryGetValue(ConfigHelper.CustomCommitMethodSettingName, out methodName))
            {
                // если метод не задан, то ничего не делаем
                return ClientCommitOrderResult.BuildSuccess(order, ResultCodes.SUCCESS, "метод автоматического подтверждения не задан");
            }

            if (string.IsNullOrEmpty(methodName))
            {
                return ClientCommitOrderResult.BuildFail(ResultCodes.CUSTOM_COMMIT_METHOD_IS_INVALID, "некорректное значение имени метода автоматического подтверждения");
            }

            // вызываем метод автоматического подтверждения
            bool confirmed;
            string confirmDescription;

            try
            {
                var commitRes = partnerConnectorProvider.CustomCommitOrder(order, methodName);

                confirmed = commitRes.Success;
                confirmDescription = commitRes.ResultDescription;
            }
            catch (Exception e)
            {
                confirmed = false;
                confirmDescription = "ошибка вызова метода автоматического подтверждения заказа";

                log.Error(confirmDescription, e);
            }

            order.Status = confirmed ? OrderStatuses.Delivered : OrderStatuses.CancelledByPartner;

            var statusRes = ChangeOrdersStatuses(new[]
            {
                new OrdersStatus
                {
                    OrderId = order.Id,
                    OrderStatus = order.Status
                }
            });

            if (!statusRes.Success)
            {
                return ClientCommitOrderResult.BuildFail(statusRes.ResultCode, statusRes.ResultDescription);
            }

            var resultCode = order.Status != OrderStatuses.CancelledByPartner
                                 ? ResultCodes.SUCCESS
                                 : ResultCodes.PROVIDER_CONFIRM_ORDER_DECLINED;

            return ClientCommitOrderResult.BuildSuccess(order, resultCode, confirmDescription);
        }

        private string GetCommitResText(CommitOrderResult commitRes)
        {
            var message = string.Format(
                "Confirmed:{0} ReasonCode:{1} Reason:{2} ResultCode:{3} ResultDescription:{4}",
                commitRes.Confirmed,
                commitRes.ReasonCode,
                commitRes.Reason,
                commitRes.ResultCode, 
                commitRes.ResultDescription);
            return message;
        }

        private GetOrdersHistoryResult SearchOrdersInternal(GetOrdersHistoryParameters parameters)
        {
            parameters.ThrowIfNull("parameters");

            var now = DateTime.Now;

            var internalCountToSkip = parameters.CountToSkip ?? 0;
            var internalCountToTake = parameters.CountToTake.NormalizeByHeight(ApiSettings.MaxResultsCountOrders);

            var internalCalcTotalCount = parameters.CalcTotalCount.HasValue && parameters.CalcTotalCount.Value;
            var skipStatusesOrder = new[] { OrderStatuses.Draft };

            var statuses = parameters.Statuses == null
                ? null
                : parameters.Statuses.SelectMany(x => x.GetOrderStatuses()).Distinct().ToArray();

            var orders = ordersDataSource.GetOrders(
                parameters.ClientId,
                parameters.StartDate,
                parameters.EndDate,
                statuses,
                skipStatusesOrder,
                internalCountToSkip,
                internalCountToTake,
                internalCalcTotalCount);

            if (orders == null || orders.Count == 0)
            {
                log.Debug("Найдено 0 заказов пользователя/клиента");
                return new GetOrdersHistoryResult
                {
                    ResultCode = ResultCodes.SUCCESS,
                    ResultDescription = "По заданным параметрам заказы не найдены",
                    Orders = null,
                    TotalCount = internalCalcTotalCount ? (int?)0 : null
                };
            }

            var arrOrders = orders.ToArray();

            return new GetOrdersHistoryResult
            {
                ResultCode = ResultCodes.SUCCESS,
                ResultDescription = null,
                Orders = arrOrders,
                TotalCount = internalCalcTotalCount ? orders.TotalCount : null
            };
        }

        private Order BuildOnlineOrder(CreateOrderFromOnlinePartnerParameters parameters)
        {
            var now = DateTime.Now;
            var nowUtc = DateTime.UtcNow;

            // todo: нужно ли добавить проверку, что количество вознаграждений в заказе = 1?
            var items = parameters.Items;

            var order = new Order();

            order.ClientId = parameters.ClientId;
            order.ExternalOrderId = parameters.ExternalOrderId;
            order.PartnerId = parameters.PartnerId;
            order.Status = OrderStatuses.Registration;
            order.StatusChangedDate = now;
            order.StatusUtcChangedDate = nowUtc;
            order.InsertedDate = parameters.DateTime;
            order.InsertedUtcDate = parameters.UtcDateTime;
            order.UpdatedDate = now;
            order.UpdatedUtcDate = nowUtc;
            order.UpdatedUserId = ApiSettings.ClientSiteUserName;

            order.TotalWeight = parameters.Items == null ? 0 : parameters.Items.Sum(x => x.Amount * x.Weight);

            order.PaymentStatus = OrderPaymentStatuses.No;
            order.DeliveryPaymentStatus = OrderDeliveryPaymentStatus.No;

            OrderItem[] orderItems;

            if (items != null)
            {
                orderItems =
                    items.Select(
                        x =>
                            new OrderItem
                            {
                                Amount = x.Amount,
                                Product =
                                    new Product
                                    {
                                        // в заказах онлайн-партнёров не может быть ProductId
                                        // https://jira.rapidsoft.ru/browse/VTBPLK-1751
                                        ProductId = null,
                                        PartnerProductId = x.ArticleId,
                                        Name = x.ArticleName,
                                        Description = x.Comment,
                                        PriceRUR = x.Price,
                                        Price = x.BonusPrice,
                                        Weight = x.Weight
                                    }
                            }).ToArray();
            }
            else
            {
                orderItems = new OrderItem[0];
            }

            order.Items = orderItems;
            order.CarrierId = parameters.PartnerId;

            var prices = orderItems.Length > 0
                             ? new[]
                             {
                                 new OrderItemPrice
                                 {
                                     ProductId = null,
                                     ProductPriceRur = orderItems[0].Product.PriceRUR,
                                     ProductPrice = orderItems[0].Product.Price
                                 }
                             }
                             : new OrderItemPrice[0];

            priceSpecification.FillOrderPrice(order, prices, 0, 0, 0, 0);

            var orderId = ordersDataSource.Insert(order);

            order.Id = orderId;

            return order;
        }

        private void ConvertRurToBonus(Dictionary<string, string> clientContext, DeliveryGroup[] deliveryGroups, Product[] products)
        {
            if (deliveryGroups == null)
            {
                return;
            }

            var deliveryPricesRur = deliveryGroups.SelectMany(g => g.DeliveryVariants.Select(v => v.DeliveryCost)).ToList();
            deliveryPricesRur.AddRange(deliveryGroups.SelectMany(g => g.DeliveryVariants.SelectMany(v => v.PickupPoints).Select(p => p.DeliveryCost)));

            var res = mechanicsProvider.CalculateDeliveryPrices(clientContext, deliveryPricesRur.Distinct().ToArray(), products.First().PartnerId);

            EnumerateVariants(deliveryGroups, res, products.First().PartnerId);
        }

        private void EnumerateVariants(DeliveryGroup[] deliveryGroups, CalculatedPrice[] calculatedPrices, int partnerId)
        {
            foreach (var deliveryGroup in deliveryGroups)
            {
                if (deliveryGroup.DeliveryVariants == null)
                {
                    continue;
                }

                foreach (var variant in deliveryGroup.DeliveryVariants)
                {
                    var deliveryCost = GetCalculatedPrice(calculatedPrices, partnerId, variant.DeliveryCost);
                    
                    variant.BonusDeliveryCost = deliveryCost.Price.Bonus;

                    if (variant.PickupPoints == null)
                    {
                        continue;
                    }

                    foreach (var pickupVariant in variant.PickupPoints)
                    {
                        deliveryCost = GetCalculatedPrice(calculatedPrices, partnerId, pickupVariant.DeliveryCost);

                        pickupVariant.BonusDeliveryCost = deliveryCost.Price.Bonus;
                    }
                }
            }
        }

        private CalculatedPrice GetCalculatedPrice(CalculatedPrice[] calculatedPrices, int partnerId, decimal priceDeliveryRur)
        {
            var deliveryCost = calculatedPrices.FirstOrDefault(p => p.PartnerId == partnerId && p.Price.Rur == priceDeliveryRur);

            if (deliveryCost == null)
            {
                throw new Exception(string.Format("Не удалось найти расчитанную цену в баллах partnerId:{0} DeliveryCost(руб):{1}", partnerId, priceDeliveryRur));
            }

            return deliveryCost;
        }

        private string GetFactorsString(FactorsResult factors)
        {
            return string.Format("БМ:{0} БА:{1} М:{2} А:{3}", factors.BaseMultiplicationFactor, factors.BaseAdditionFactor, factors.MultiplicationFactor, factors.AdditionFactor);
        }

        private void ClearOrderAttempt(string clientId)
        {
            var currentHour = DateTime.Now.TimeOfDay.Hours;

            // сбрасываем только в определенное время суток
            if (currentHour < ClearOrderAttemptMinHour || currentHour > ClearOrderAttemptMaxHour)
            {
                return;
            }

            try
            {
                using (var service = new BankConnectorClient())
                {
                    var response = service.ClearOrderAttempt(clientId);

                    if (!response.Success)
                    {
                        log.WarnFormat(
                            "Не удалось сбросить попытку клиента оформить заказ (заказ был успешно оформлен): [{0}] - {1}",
                            response.ResultCode,
                            response.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Warn("Ошибка при сбросе попытки клиента оформить заказ (заказ был успешно оформлен), ClientID = " + clientId, ex);
            }
        }

        #endregion
    }
}
