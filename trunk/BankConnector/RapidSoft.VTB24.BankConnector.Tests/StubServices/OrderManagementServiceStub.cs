namespace RapidSoft.VTB24.BankConnector.Tests.StubServices
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService;

	public class OrderManagementServiceStub : StubBase, IOrderManagementService
	{
		public static List<Order> TestOrders = new List<Order>();

		public static void InitTestOrders()
		{
			TestOrders = new List<Order>
				             {
					             new Order
						             {
							             Id = 315,
							             BonusDeliveryCost = 10,
							             BonusItemsCost = 11,
							             BonusTotalCost = 12,
							             DeliveryCost = 13,
							             DeliveryInfo = CreateDeliveryInfoForDeliveryStub(),
							             Items =
								             new[]
									             {
										             new OrderItem
											             {
												             Amount = 17,
                                                             AmountPriceBonus = 18,
                                                             AmountPriceRur = 19,
												             Product =
													             new Product
														             {
															             ProductId = "ProductId",
															             Name = "ItemName"
														             }
											             }
									             },
							             PartnerId = 1,
							             ExternalOrderId = "externalOrderId",
                                         ItemsCost = 11,
							             TotalCost = 15,
							             InsertedDate = DateTime.Now,
							             ClientId = "vtb_2"
						             },
					             new Order
						             {
							             Id = 316,
							             BonusDeliveryCost = 10,
							             BonusItemsCost = 11,
							             BonusTotalCost = 12,
							             DeliveryCost = 13,
							             DeliveryInfo = CreateDeliveryInfoForPickUpStub(),
							             Items =
								             new[]
									             {
										             new OrderItem
											             {
												             Amount = 17,
                                                             AmountPriceBonus = 18,
                                                             AmountPriceRur = 19,
												             Product =
													             new Product
														             {
															             ProductId = "ProductId",
															             Name = "ItemName"
														             }
											             }
									             },
							             PartnerId = 1,
							             ExternalOrderId = "externalOrderId",
                                         ItemsCost = 11,
							             TotalCost = 15,
							             InsertedDate = DateTime.Now,
							             ClientId = "vtb_2"
						             }

				             };
		}

	    public string Echo(string message)
	    {
	        throw new NotImplementedException();
	    }

	    public Task<string> EchoAsync(string message)
	    {
	        throw new NotImplementedException();
	    }

	    public CreateOrderResult CreateOrderFromBasketItems(
			CreateOrderFromBasketItemsParameters parameters)
		{
			throw new NotImplementedException();
		}

	    public Task<CreateOrderResult> CreateOrderFromBasketItemsAsync(CreateOrderFromBasketItemsParameters parameters)
	    {
	        throw new NotImplementedException();
	    }

	    public CreateOrderResult CreateOnlinePartnerOrder(CreateOrderFromOnlinePartnerParameters parameters)
		{
			throw new NotImplementedException();
		}

	    public Task<CreateOrderResult> CreateOnlinePartnerOrderAsync(CreateOrderFromOnlinePartnerParameters parameters)
	    {
	        throw new NotImplementedException();
	    }

	    public ClientCommitOrderResult ClientCommitOrder(string userId, int orderId)
		{
			throw new NotImplementedException();
		}

	    public Task<ClientCommitOrderResult> ClientCommitOrderAsync(string clientId, int orderId)
	    {
	        throw new NotImplementedException();
	    }

	    public GetOrdersHistoryResult GetOrdersHistory(GetOrdersHistoryParameters parameters)
		{
			throw new NotImplementedException();
		}

	    public Task<GetOrdersHistoryResult> GetOrdersHistoryAsync(GetOrdersHistoryParameters parameters)
	    {
	        throw new NotImplementedException();
	    }

	    public HasNonterminatedOrdersResult HasNonterminatedOrders(string clientId)
		{
			var result = new HasNonterminatedOrdersResult();
			result.HasOrders = false;
			result.Success = true;
			result.ResultCode = 0;
			return result;
		}

	    public Task<HasNonterminatedOrdersResult> HasNonterminatedOrdersAsync(string clientId)
	    {
	        throw new NotImplementedException();
	    }

	    public GetOrdersHistoryResult GetOrdersForPayment(GetOrdersForPaymentParameters parameters)
		{
			var result = new GetOrdersHistoryResult();
			result.ResultCode = 0;
			result.ResultDescription = this.GetStubDescription();
			result.Success = true;
			result.Orders = TestOrders.OrderBy(x => x.Id).Skip(parameters.CountToSkip).Take(parameters.CountToTake).ToArray();
			return result;
		}

	    public Task<GetOrdersHistoryResult> GetOrdersForPaymentAsync(GetOrdersForPaymentParameters parameters)
	    {
	        throw new NotImplementedException();
	    }

	    public GetOrderResult GetOrderById(int orderId, string clientId)
		{
			throw new NotImplementedException();
		}

	    public Task<GetOrderResult> GetOrderByIdAsync(int orderId, string clientId)
	    {
	        throw new NotImplementedException();
	    }

	    public GetOrderResult GetOrderByExternalId(GetOrderByExternalIdParameters parameters)
		{
			throw new NotImplementedException();
		}

	    public Task<GetOrderResult> GetOrderByExternalIdAsync(GetOrderByExternalIdParameters parameters)
	    {
	        throw new NotImplementedException();
	    }

	    public GetOrderPaymentStatusesResult GetOrderPaymentStatuses(int[] orderIds)
	    {
	        var r =
	            orderIds.Select(
	                x =>
	                new OrderPayments()
	                    {
	                        DeliveryPaymentStatus = OrderDeliveryPaymentStatus.No,
	                        PaymentStatus = OrderPaymentStatuses.No,
	                        Id = x
	                    });

	        return new GetOrderPaymentStatusesResult
	                   {
	                       OrderPaymentStatuses = r.ToArray(),
	                       ResultCode = 0,
	                       Success = true,
	                       ResultDescription = null
	                   };
	    }

	    public Task<GetOrderPaymentStatusesResult> GetOrderPaymentStatusesAsync(int[] orderIds)
	    {
	        throw new NotImplementedException();
	    }

	    public GetLastDeliveryAddressesResult GetLastDeliveryAddresses(string userId, bool excludeAddressesWithoutKladr, int? countToTake)
		{
			throw new NotImplementedException();
		}

	    public Task<GetLastDeliveryAddressesResult> GetLastDeliveryAddressesAsync(string clientId, bool excludeAddressesWithoutKladr, int? countToTake)
	    {
	        throw new NotImplementedException();
	    }

	    public ChangeExternalOrdersStatusesResult ChangeExternalOrdersStatuses(ExternalOrdersStatus[] externalOrdersStatuses)
		{
			throw new NotImplementedException();
		}

	    public Task<ChangeExternalOrdersStatusesResult> ChangeExternalOrdersStatusesAsync(ExternalOrdersStatus[] externalOrdersStatuses)
	    {
	        throw new NotImplementedException();
	    }

	    public ChangeOrdersStatusesResult ChangeOrdersStatuses(OrdersStatus[] ordersStatuses)
		{
			var result = new ChangeOrdersStatusesResult
			{
				Success = true,
				ResultCode = 0,
				ResultDescription = this.GetStubDescription(),
				ChangeOrderStatusResults =
					GetChangeStatusResult(
						ordersStatuses.Select(x => x.OrderId.Value).ToList()),
			};
			return result;
		}

	    public Task<ChangeOrdersStatusesResult> ChangeOrdersStatusesAsync(OrdersStatus[] ordersStatuses)
	    {
	        throw new NotImplementedException();
	    }

	    public ResultBase ChangeOrderStatusDescription(int orderId, string orderStatusDescription)
	    {
	        throw new NotImplementedException();
	    }

	    public Task<ResultBase> ChangeOrderStatusDescriptionAsync(int orderId, string orderStatusDescription)
	    {
	        throw new NotImplementedException();
	    }

	    public ChangeOrdersStatusesResult ChangeOrdersStatusesBeforePayment()
		{
			return new ChangeOrdersStatusesResult { ChangeOrderStatusResults = null, ResultCode = 0, Success = true };
		}

	    public Task<ChangeOrdersStatusesResult> ChangeOrdersStatusesBeforePaymentAsync()
	    {
	        throw new NotImplementedException();
	    }

	    public ChangeOrdersStatusesResult ChangeOrdersPaymentStatuses(OrdersPaymentStatus[] statuses)
		{
			var result = new ChangeOrdersStatusesResult();
			result.Success = true;
			result.ResultCode = 0;
			result.ResultDescription = this.GetStubDescription();
			result.ChangeOrderStatusResults = GetChangeStatusResult(statuses.Select(x => x.OrderId).ToList());
			return result;
		}

	    public Task<ChangeOrdersStatusesResult> ChangeOrdersPaymentStatusesAsync(OrdersPaymentStatus[] statuses)
	    {
	        throw new NotImplementedException();
	    }

	    public ChangeOrdersStatusesResult ChangeOrdersDeliveryStatuses(OrdersDeliveryStatus[] statuses)
		{
			var result = new ChangeOrdersStatusesResult();
			result.Success = true;
			result.ResultCode = 0;
			result.ResultDescription = this.GetStubDescription();
			result.ChangeOrderStatusResults = GetChangeStatusResult(statuses.Select(x => x.OrderId).ToList());
			return result;
		}

	    public Task<ChangeOrdersStatusesResult> ChangeOrdersDeliveryStatusesAsync(OrdersDeliveryStatus[] statuses)
	    {
	        throw new NotImplementedException();
	    }

	    public GetDeliveryVariantsResult GetDeliveryVariants(GetDeliveryVariantsParameters parameters)
	    {
	        return new GetDeliveryVariantsResult()
	               {
	                   ResultCode = 0
	               };
	    }

	    public Task<GetDeliveryVariantsResult> GetDeliveryVariantsAsync(GetDeliveryVariantsParameters parameters)
	    {
	        throw new NotImplementedException();
	    }

	    private static DeliveryInfo CreateDeliveryInfoForDeliveryStub()
		{
			return new DeliveryInfo
			{
                DeliveryType = DeliveryTypes.Delivery,
                DeliveryVariantName = "Курьерская доставка",
                Address = new DeliveryAddress()
                          {
                              CityKladrCode = "_CityKladrCode",
                              RegionKladrCode = "_RegionKladrCode",
                              AddressText = "_AddressText",
                              RegionTitle = "_RegionTitle",
                              CityTitle = "_CityTitle"                              
                          },
				Contact =
				    new Contact
					    {
						    Email = "email",
						    FirstName = "FirstName",
						    LastName = "LastName",
						    MiddleName = "MiddleName",
						    Phone =
							    new PhoneNumber
								    {
									    CityCode = "927",
									    CountryCode = "7",
									    LocalNumber = "1234567"
								    }
					    }
			};
		}

        private static DeliveryInfo CreateDeliveryInfoForPickUpStub()
        {
            return new DeliveryInfo
            {
                DeliveryType = DeliveryTypes.Pickup,
                DeliveryVariantName = "Самовывоз",
                PickupPoint = new PickupPoint1()
                              {                                  
                                  Address = "Address"
                              },
                Contact =
                    new Contact
                    {
                        Email = "email",
                        FirstName = "FirstName",
                        LastName = "LastName",
                        MiddleName = "MiddleName",
                        Phone =
                            new PhoneNumber
                            {
                                CityCode = "927",
                                CountryCode = "7",
                                LocalNumber = "1234567"
                            }
                    }
            };
        }

		private ChangeOrderStatusResult[] GetChangeStatusResult(List<int> orderIds)
		{
			return
				orderIds.Select(
					x =>
					new ChangeOrderStatusResult
						{
							OrderId = x,
							ResultCode = 0,
							Success = true,
							ResultDescription = this.GetStubDescription(),
						}).ToArray();
		}
	}
}
