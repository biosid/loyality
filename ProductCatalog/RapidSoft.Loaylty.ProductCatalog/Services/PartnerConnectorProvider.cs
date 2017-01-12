namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;

    using System.Linq;

    using API.OutputResults;

    using AutoMapper;

    using Interfaces;

    using PartnersConnector.WsClients.PartnersOrderManagementService;

    using Contact = API.Entities.Contact;
    using DeliveryInfo = API.Entities.DeliveryInfo;
    using GetDeliveryVariantsResult = PartnersConnector.WsClients.PartnersOrderManagementService.GetDeliveryVariantsResult;
    using Location = API.InputParameters.Location;
    using Order = API.Entities.Order;
    using OrderItem = API.Entities.OrderItem;

    internal class PartnerConnectorProvider : IPartnerConnectorProvider
    {
        #region Constructors

        static PartnerConnectorProvider()
        {
            CreateMap();
        }

        #endregion

        #region IPartnerConnectorProvider Members

        public CheckOrderResult CheckOrder(Order order)
        {
            try
            {
                using (var client = new OrderManagementServiceClient())
                {
                    var providerOrder = CreateProviderOrder(order);

                    return client.CheckOrder(providerOrder);
                }
            }
            catch (Exception e)
            {
                var message = "Не удалось получить ответ сервиса проверки заказа у провайдера";
                throw new OperationException(message, e)
                {
                    ResultCode = ResultCodes.PROVIDER_CONNECTOR_CALL_ERROR
                };
            }
        }

        public CommitOrderResult CommitOrder(Order order)
        {
            try
            {
                using (var client = new OrderManagementServiceClient())
                {
                    var providerOrder = CreateProviderOrder(order);

                    return client.CommitOrder(providerOrder);
                }
            }
            catch (Exception e)
            {
                throw new OperationException("Не удалось поставить заказ в очередь на проверку", e)
                {
                    ResultCode = ResultCodes.PROVIDER_CONNECTOR_CALL_ERROR
                };
            }
        }

        public PartnersConnector.WsClients.PartnersOrderManagementService.ResultBase CustomCommitOrder(Order order, string methodName)
        {
            try
            {
                using (var client = new OrderManagementServiceClient())
                {
                    var providerOrder = CreateProviderOrder(order);

                    return client.CustomCommitOrder(providerOrder, methodName);
                }
            }
            catch (Exception e)
            {
                throw new OperationException("Не удалось подтвердить заказ", e)
                {
                    ResultCode = ResultCodes.PROVIDER_CONNECTOR_CALL_ERROR
                };
            }
        }

        public FixBasketItemPriceResult FixBasketItemPrice(FixBasketItemPriceParam param)
        {
            try
            {
                using (var client = new OrderManagementServiceClient())
                {
                    return client.FixBasketItemPrice(param);
                }
            }
            catch (Exception e)
            {
                var message = "Ошибка обращения к коннектору партнёров при фиксации цены";
                throw new OperationException(message, e)
                {
                    ResultCode = ResultCodes.PROVIDER_CONNECTOR_CALL_ERROR
                };
            }
        }

        public GetDeliveryVariantsResult GetDeliveryVariants(Location location, OrderItem orderItem, string clientId, int partnerId)
        {
            return GetDeliveryVariants(location, new[] { orderItem }, clientId, partnerId);
        }

        public GetDeliveryVariantsResult GetDeliveryVariants(Location location, OrderItem[] orderItems, string clientId, int partnerId)
        {
            var providerLocation = Mapper.Map<Location, PartnersConnector.WsClients.PartnersOrderManagementService.Location>(location);
            var providerOrderItems = orderItems.Select(Mapper.Map<OrderItem, PartnersConnector.WsClients.PartnersOrderManagementService.OrderItem>).ToArray();

            var param = new GetDeliveryVariantsParam()
            {
                PartnerId = partnerId,
                ClientId = clientId,
                Location = providerLocation,
                Items = providerOrderItems
            };

            using (var client = new OrderManagementServiceClient())
            {
                var result = client.GetDeliveryVariants(param);

                return result;
            }
        }

        #endregion

        #region Methods

        private static long PhoneToLong(Contact src)
        {
            if (src == null)
            {
                throw new ArgumentNullException("Contact is null");
            }

            if (src.Phone == null)
            {
                throw new ArgumentException("Contact.Phone is null");
            }

            var phoneToLong = string.Format("{0}{1}{2}", src.Phone.CountryCode, src.Phone.CityCode, src.Phone.LocalNumber);
            return long.Parse(phoneToLong);
        }

        private static PartnersConnector.WsClients.PartnersOrderManagementService.Order CreateProviderOrder(Order order)
        {
            var providerOrder = Mapper.Map<Order, PartnersConnector.WsClients.PartnersOrderManagementService.Order>(order);
            return providerOrder;
        }

        private static void CreateMap()
        {
            Mapper.CreateMap<DeliveryInfo, PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryInfo>();
            Mapper.CreateMap<API.Entities.DeliveryAddress, PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryAddress>();
            Mapper.CreateMap<API.Entities.VariantsLocation, PartnersConnector.WsClients.PartnersOrderManagementService.VariantsLocation>();
            Mapper.CreateMap<API.Entities.PickupPoint, PartnersConnector.WsClients.PartnersOrderManagementService.PickupPoint>();

            Mapper.CreateMap<Contact, PartnersConnector.WsClients.PartnersOrderManagementService.Contact>()
                  .ForMember(dest => dest.Phone, opt => opt.ResolveUsing(src => PhoneToLong(src)));

            Mapper.CreateMap<Order, PartnersConnector.WsClients.PartnersOrderManagementService.Order>();

            Mapper.CreateMap<OrderItem, PartnersConnector.WsClients.PartnersOrderManagementService.OrderItem>()
                  .ForMember(dest => dest.OfferId, opt => opt.MapFrom(src => src.Product.PartnerProductId))
                  .ForMember(dest => dest.OfferName, opt => opt.MapFrom(src => src.Product.Name))
                  .ForMember(dest => dest.Price, opt => opt.ResolveUsing(src => src.FixedPrice != null ? src.FixedPrice.PriceRUR : src.Product.PriceRUR))
                  .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Product.Weight ?? 0))
                  .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

            Mapper.CreateMap<Location, PartnersConnector.WsClients.PartnersOrderManagementService.Location>();
        }

        #endregion
    }
}