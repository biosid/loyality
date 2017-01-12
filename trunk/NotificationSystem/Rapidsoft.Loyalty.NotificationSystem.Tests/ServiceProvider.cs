namespace Rapidsoft.Loyalty.NotificationSystem.Tests.Services
{
    using System;
    using System.IO;

    using API;

    using Moq;

    using NotificationSystem.Services;

    using RapidSoft.VTB24.ArmSecurity.Interfaces;
using Rapidsoft.Loyalty.NotificationSystem.Email;

    public class ServiceProvider
    {
        private Mock<ISecurityChecker> securityMock;

        public Mock<ISecurityChecker> GetSecurityMock()
        {
            return securityMock;
        }

        public IAdminFeedbackService GetAdminFeedbackService()
        {
            securityMock = new Mock<ISecurityChecker>();
            securityMock.Setup(m => m.CheckPermissions(It.IsAny<string>(), It.IsAny<string[]>()));

            var fileProvider = new Mock<IFileProvider>();

            fileProvider.Setup(m => m.MapPath(It.IsAny<string>())).Returns((string filePath) => GetFullPath(filePath));

            var service = new AdminFeedbackService(
                securityChecker: GetSecurityMock().Object,
                fileProvider:fileProvider.Object);
            return service;
        }

        private static string GetFullPath(string filePath)
        {
            return Path.GetFullPath(filePath.TrimStart(new[] {'/', '\\'}));
        }

        public IMessageNotificationService GetMessageNotificationService(IProfile profile, ISender sender)
        {
            var fileProvider = new Mock<IFileProvider>();

            fileProvider.Setup(m => m.MapPath(It.IsAny<string>())).Returns((string filePath) => GetFullPath(filePath));

            var service = new MessageNotificationService(
                    fileProvider: fileProvider.Object,
                    profileService: profile ?? new Profile(),
                    sender: sender ?? new Sender()
                );
            return service;
        }

        public IClientMessageService GetClientMessageService(IMessageNotificationService messageNotificationService = null)
        {
            var service = new ClientMessageService(
                    messageNotificationService:messageNotificationService ?? new MessageNotificationService()
                );
            return service;
        }
    }
}