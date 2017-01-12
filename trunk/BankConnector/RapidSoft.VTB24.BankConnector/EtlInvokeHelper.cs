namespace RapidSoft.VTB24.BankConnector
{
    using System;
    using System.Configuration;
    using System.Diagnostics.Contracts;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime;
    using RapidSoft.VTB24.BankConnector.Processors;

    public static class EtlInvokeHelper
    {
        private static readonly IUnityContainer Container = new UnityContainer();

        static EtlInvokeHelper()
        {
            Container.LoadConfiguration((UnityConfigurationSection)ConfigurationManager.GetSection("unity"));
        }

        public static TResult ResolveAndInvoke<TArgument, TResult>(Func<TArgument, TResult> invoker)
        {
            using (var childContainer = Container.CreateChildContainer())
            {
                var arg = childContainer.Resolve<TArgument>();

                return invoker(arg);
            }
        }

        #region 3.1. Регистрация клиентов в Системе лояльности (ReceiveRegistrationClients)

        public static EtlStepResult RegisterClients(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<ClientRegistrator>(context, logger, p => p.RegisterClients());
        }

        #endregion

        #region 3.2. Регистрация клиентов на стороне Банка (RegisterBankClients)

        public static EtlStepResult RegisterBankClients(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<BankClientRegistrator>(context, logger, p => p.RegisterBankClients());
        }

        #endregion

        #region 3.3. Активация клиентов в Системе лояльности

        public static EtlStepResult ActivateClients(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<ActivateClientsService>(context, logger, p => p.Execute());
        }

        #endregion

        #region 3.4. Отключение клиентов от Системы лояльности

        // Отправка на отключение
        public static EtlStepResult PrepareClientsToDelete(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<ClientDetachProcessor>(context, logger, p => p.PrepareClientsToDelete());
        }

        // Обработка ответа банка
        public static EtlStepResult ReceiveDetachClients(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<ClientDetachProcessor>(context, logger, p => p.DetachClients());
        }

        #endregion

        #region 3.5. Изменение анкетных данных клиентов

        public static EtlStepResult UpdateClients(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<ClientUpdater>(context, logger, p => p.UpdateClients());
        }

        #endregion
        
        #region 3.6. Начисление бонусов на бонусные счета клиентов

        public static EtlStepResult ReceiveAccrualsExecute(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<AccrualProcessor>(context, logger, p => p.ReceiveAccrualsExecute());
        }

        #endregion

        #region 3.7. Формирование кампаний

        // Метод подготавливает данные для отправки: загружает из компонента список актуальных промоакци и сохраняет в таблице PromoAction
        public static EtlStepResult PreparePromoAction(EtlContext context, IEtlLogger logger, string dateSent, string indexSent)
        {
            return InvokeProcessorStep<PromoActionProcessor>(context, logger, p => p.PreparePromoAction(dateSent, indexSent));
        }

        public static EtlStepResult ProcessPromoActionResponse(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<PromoActionProcessor>(context, logger, p => p.ProcessResponse());
        }

        #endregion

        #region 3.8. Формирование списка участников целевых кампаний

        public static EtlStepResult AssignClientTargetAudiences(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<TargetAudienceProcessor>(context, logger, p => p.AssignClientTargetAudiences());
        }

        #endregion
        
        #region 3.9. Формирование персональных сообщений

        public static EtlStepResult RegisterMessages(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<PersonalMessageProcessor>(context, logger, p => p.Process());
        }

        #endregion

        #region 3.10. Отправка реестра совершенных заказов

        // Отправка заказов в банк
        public static EtlStepResult SaveOrdersForPayment(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<SendOrdersProcessor>(context, logger, p => p.Execute());
        }

        // Обработка ответа банка
        public static EtlStepResult OrdersPaymentExecute(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<OrdersPaymentProcessor>(context, logger, p => p.Execute());
        }

        #endregion

        #region 3.12. Изменение номера мобильного телефона клиента Банком

        // Изменение логинов клиентов присланных банков
        public static EtlStepResult ClientLoginBankUpdates(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<ClientLoginBankUpdatesProcessor>(context, logger, p => p.Execute());
        }
        
        #endregion

        #region 3.13 Сброс пароля клиента

        public static EtlStepResult ResetClientsPasswords(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<BankPwdResetProcessor>(context, logger, p => p.Execute());
        }

        #endregion

        #region Повторное проведение оплаты отмененных заказов

        public static EtlStepResult RetryErrorPayments(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<ErrorPaymentsProcessor>(context, logger, p => p.Execute());
        }

        #endregion

        #region Рассылка напоминаний о незавершенных заказах

        public static EtlStepResult NotifyIncompleteOrders(EtlContext context, IEtlLogger logger, DateTime fromDate)
        {
            return InvokeProcessorStep<OrderNotificationsProcessor>(context, logger, p => p.NotifyIncompleteOrders(fromDate));
        }

        #endregion

        #region Рассылка предложений оценить сервис

        public static EtlStepResult NotifyExecutedOrders(EtlContext context, IEtlLogger logger, DateTime fromDateTime, DateTime toDateTime, string surveyUrl)
        {
            return InvokeProcessorStep<OrderNotificationsProcessor>(context, logger, p => p.NotifyExecutedOrders(fromDateTime, toDateTime, surveyUrl));
        }

        #endregion

        #region Регистрация продуктов банка
        public static EtlStepResult ProcessBankOffers(EtlContext context, IEtlLogger logger)
        {
            return InvokeProcessorStep<BankOffersProcessor>(context, logger, p => p.ProcessBankOffers());
        }
        #endregion

        private static EtlStepResult InvokeProcessorStep<TProcessor>(EtlContext context, IEtlLogger logger, Action<TProcessor> invoker)
        {
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(logger != null);

            var etlLogger = new EtlLogger.EtlLogger(logger, context.EtlPackageId, context.EtlSessionId);

            using (var childContainer = Container.CreateChildContainer())
            {
                childContainer.RegisterInstance(etlLogger);

                var processor = childContainer.Resolve<TProcessor>();

                invoker(processor);

                return new EtlStepResult
                {
                    Status = etlLogger.EtlStatus
                };
            }
        }
    }
}