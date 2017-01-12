namespace RapidSoft.VTB24.BankConnector.Processors
{
	using System;
	using System.Globalization;
	using System.Linq;
	using RapidSoft.Loaylty.PromoAction.WsClients.AdminMechanicsService;
	using RapidSoft.VTB24.BankConnector.DataSource;
	using RapidSoft.VTB24.BankConnector.DataSource.Interface;
	using RapidSoft.VTB24.BankConnector.Extension;
	using RapidSoft.VTB24.BankConnector.Service;

    internal class PromoActionProcessor : ProcessorBase
    {
        private readonly IAdminMechanicsServiceProvider adminMechanicsServiceProvider;

        public PromoActionProcessor(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            IAdminMechanicsServiceProvider adminMechanicsServiceProvider)
            : base(logger, uow)
        {
            this.adminMechanicsServiceProvider = adminMechanicsServiceProvider;
        }

        public void PreparePromoAction(string dateSent, string indexSent)
        {
            var date = DateTime.ParseExact(dateSent, "yyyyMMdd", CultureInfo.InvariantCulture);
            var index = int.Parse(indexSent);

            Logger.Info("Получение данных из сервиса AdminMechanicsService.svc компонента \"Акции\"");
            var rules = adminMechanicsServiceProvider.GetPromoActions(date, true, ApproveStatus.NotApproved) ?? new Rule[0];

            var promoActions = rules.Select(x => x.BuildPromoAction(date, index, Logger.EtlSessionId)).ToList();

            Logger.Info(string.Format("Сохранение информации промоакций (count {0})", promoActions.Count));
            this.Uow.PromoActionRepository.Add(promoActions);

            this.Uow.Save();
        }

        public void ProcessResponse()
        {
            Logger.Info("Загрузка данных ответа из буферной таблицы");

            var responses = this.Uow.PromoActionResponseRepository.GetAll().Where(x => x.EtlSessionId == Logger.EtlSessionId).ToList();

            if (!responses.Any())
            {
                Logger.Info("Данные не найдены, обрабатывать нечего");
                return;
            }

            Logger.Info("Передача статусов промоакций в компонент \"Акции\"");
            adminMechanicsServiceProvider.SetPromoActionsStatus(responses);
        }
    }
}
