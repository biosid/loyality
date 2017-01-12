using System;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using NSubstitute;
using RapidSoft.Etl.Logging;
using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource;
using RapidSoft.VTB24.BankConnector.DataSource.Repository;

namespace RapidSoft.VTB24.BankConnector.Tests.Helpers
{
    public class DataSourceHelper
	{
        public static IUnitOfWork CreateUow()
        {
            var ctx = new BankConnectorDBContext();
            return new UnitOfWork(
                ctx,
                new GenericRepository<StepsBuffer>(ctx),
                new GenericRepository<ClientAudienceRelation>(ctx),
                new GenericRepository<ClientCardRegStatus>(ctx),
                new GenericRepository<ClientForDeletion>(ctx),
                new GenericRepository<ClientForDeletionResponse>(ctx),
                new GenericRepository<ClientForBankRegistrationResponse>(ctx),
                new ClientPersonalMessageRepository(ctx),
                new ClientPersonalMessageResponseRepository(ctx),
                new PromoActionRepository(ctx),
                new PromoActionResponseRepository(ctx),
                new ClientForActivationRepository(ctx),
                new ClientForRegistrationRepository(ctx),
                new ClientForBankRegistrationRepository(ctx),
                new OrderPaymentResponseRepository(ctx),
                new OrderForPaymentRepository(ctx),
                new OrderItemsForPaymentRepository(ctx),
                new OrderPaymentResponse2Repository(ctx),
                new ClientForRegistrationResponseRepository(ctx),
                new GenericRepository<Accrual>(ctx),
                new ClientUpdatesRepository(ctx),
                new LoyaltyClientUpdateRepository(ctx),
                new ProfileCustomFieldsRepository(ctx),
                new ProfileCustomFieldsValuesRepository(ctx),
                new ClientLoginBankUpdatesRepository(ctx),
                new ClientLoginBankUpdatesResponseRepository(ctx),
                new ClientForBankPwdResetRepository(ctx),
                new ClientForBankPwdResetResponseRepository(ctx),
                new UnitellerPaymentsRepository(ctx),
                new OrderAttemptsRepository(ctx),
                new BankOfferRepository(ctx),
                new RegisterBankOffersRepository(ctx),
                new RegisterBankOffersResponseRepository(ctx),
                new BankSmsRepository(ctx));
        }

        public static IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();
            container.LoadConfiguration((UnityConfigurationSection)ConfigurationManager.GetSection("unity"));
            container.RegisterInstance(new EtlLogger.EtlLogger(Substitute.For<IEtlLogger>(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
            return container;
        }

		public static void CleanUpTestOrder(int orderId)
		{
			using (var uow = CreateUow())
			{
				var orderPaymentResponse = uow.OrderPaymentResponseRepository;
				var orderForPayment = uow.OrderForPaymentRepository;
                var orderItemsForPayment = uow.OrderItemsForPaymentRepository;
                var orderPaymentResponse2 = uow.OrderPaymentResponse2Repository;

                var response = orderPaymentResponse.GetAll().Where(x => x.OrderId == orderId);
				foreach (var paymentResponse in response)
				{
					orderPaymentResponse.Delete(paymentResponse);
				}

				var payments = orderForPayment.GetAll().Where(x => x.OrderId == orderId);
				foreach (var payment in payments)
				{
					orderForPayment.Delete(payment);
				}

                var paymentItems = orderItemsForPayment.GetAll().Where(x => x.OrderId == orderId);
                foreach (var paymentItem in paymentItems)
                {
                    orderItemsForPayment.Delete(paymentItem);
                }

			    var response2 = orderPaymentResponse2.GetAll().Where(x => x.OrderId == orderId);
			    foreach (var paymentResponse2 in response2)
			    {
			        orderPaymentResponse2.Delete(paymentResponse2);
			    }

				uow.Save();
			}
		}


	}
}
