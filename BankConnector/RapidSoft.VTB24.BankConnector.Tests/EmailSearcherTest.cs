namespace RapidSoft.VTB24.BankConnector.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper.Email;
    using RapidSoft.VTB24.BankConnector.Tests.Helpers;

    [TestClass]
    public class EmailSearcherTest
    {
        [TestMethod]
        public void ShouldFindBySubjectTermTest()
        {
            var subjectTerm = "ShouldFindBySubjectTermTest_";
            var emailSubject = subjectTerm + new Random().Next(1000);

            EmailHelper.CleanupLoyaltyMailBox(subjectTerm);
            
            EmailHelper.UploadFileToLoyaltySmtpServer(null, emailSubject);

            var email = TryFindEmailMessage(subjectTerm, 3);

            Assert.IsNotNull(email, string.Format("Email not found subjectTerm:{1} trycount:{0} ", 3, subjectTerm));
        }

        private static EmailMessage TryFindEmailMessage(string subjectTerm, int tryCount)
        {
            while (tryCount > 0)
            {
                var email = new EmailSearcher(EmailHelper.GetLoyaltyImapSettings()).Find(subjectTerm);
                if (email != null)
                {
                    return email;
                }

                tryCount--;
            }

            return null;
        }
    }
}