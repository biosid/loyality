using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime;
using RapidSoft.Loaylty.PromoAction.WsClients.AdminMechanicsService;
using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource;
using RapidSoft.VTB24.BankConnector.DataSource.Interface;
using RapidSoft.VTB24.BankConnector.Processors;

namespace RapidSoft.VTB24.BankConnector.Tests.ProcessorTest
{
    [TestClass]
    public class PromoActionEtlInvokeHelperTests
    {
        [TestMethod]
        public void ShouldPreparePromoActionForSend()
        {
            var etlContext = new EtlContext(new EtlSession { EtlSessionId = "Test Etl EtlSessionSession" });

            var etlLoggerMock = new Mock<IEtlLogger>();

            var logger = new EtlLogger.EtlLogger(etlLoggerMock.Object, etlContext);

            var dateSent = "20450528"; // NOTE: Заведомо большая дата
            var indexSent = "1";

            var mechMock = new Mock<IAdminMechanicsServiceProvider>();
            var retRules = new[] { new Rule { Id = 105 }, new Rule { Id = 106 } };
            mechMock.Setup(x => x.GetPromoActions(It.IsAny<DateTime>(), true, It.IsAny<ApproveStatus[]>())).Returns(retRules);

            var promoActionRepositorymock = new Mock<IPromoActionRepository>();
            var getAll = (new[] { new PromoAction() }).AsQueryable();
            promoActionRepositorymock.Setup(x => x.GetAll()).Returns(getAll);
            promoActionRepositorymock.Setup(x => x.Add(It.IsAny<IEnumerable<PromoAction>>()));

            var mechUof = new Mock<IUnitOfWork>();
            mechUof.Setup(x => x.PromoActionRepository).Returns(promoActionRepositorymock.Object);

            var processor = new PromoActionProcessor(logger, mechUof.Object, mechMock.Object);

            processor.PreparePromoAction(dateSent, indexSent);

            mechMock.Verify(x => x.GetPromoActions(It.IsAny<DateTime>(), true, It.IsAny<ApproveStatus[]>()), Times.Once(), "Загрузка правил из компонента \"Акции\" должна выполниться 1 раз.");

            promoActionRepositorymock.Verify(x => x.Add(It.IsAny<IEnumerable<PromoAction>>()), Times.Once(), "Сохранение должно выполниться 1 раз для пачки");
        }
        
        [TestMethod]
        public void ShouldProcessPromoActionReponse()
        {
            var etlContext = new EtlContext(new EtlSession { EtlSessionId = "Test Etl EtlSessionSession" });

            var etlLoggerMock = new Mock<IEtlLogger>();

            var logger = new EtlLogger.EtlLogger(etlLoggerMock.Object, etlContext);

            var mechMock = new Mock<IAdminMechanicsServiceProvider>();
            mechMock.Setup(x => x.SetPromoActionsStatus(It.IsAny<IList<PromoActionResponse>>()));

            var promoActionResponseRepositoryMock = new Mock<IPromoActionResponseRepository>();

            var getAll =
                new[]
                    {
                        new PromoActionResponse { EtlSessionId = etlContext.EtlSessionId },
                        new PromoActionResponse { EtlSessionId = etlContext.EtlSessionId }
                    }.AsQueryable();
            promoActionResponseRepositoryMock.Setup(x => x.GetAll()).Returns(getAll);

            var mechUof = new Mock<IUnitOfWork>();
            mechUof.Setup(x => x.PromoActionResponseRepository).Returns(promoActionResponseRepositoryMock.Object);

            var processor = new PromoActionProcessor(logger, mechUof.Object, mechMock.Object);

            processor.ProcessResponse();

            promoActionResponseRepositoryMock.Verify(
                x => x.GetAll(), Times.Once(), "Загрузка ответа должна выполниться 1 раз для пачки");
            mechMock.Verify(
                x => x.SetPromoActionsStatus(It.IsAny<IList<PromoActionResponse>>()),
                Times.Once(),
                "Должны сохранить статусы в компоненте \"Акции\" 1 раз.");
        }
    }
}
