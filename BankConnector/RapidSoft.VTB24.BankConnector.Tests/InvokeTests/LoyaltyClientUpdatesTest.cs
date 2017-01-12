namespace RapidSoft.VTB24.BankConnector.Tests.InvokeTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
    using RapidSoft.VTB24.BankConnector.Tests.Helpers;
    using RapidSoft.VTB24.VtbEncryption;

    [TestClass]
    public class LoyaltyClientUpdatesTest : TestBase
    {
        private const string EmailSubject = "Update_";

        [TestMethod]
        public void LoyaltyClientUpdates()
        {
            // NOTE: Создаем отправляемые данные
            var ids = InsertTestData().ToArray();

            // Выполнение задачи
            var job = TestHelper.GetWrapper(EtlPackageIds.LoyaltyClientUpdatesJob);
            var sessionId = Guid.Parse(job.Execute());

            // Проверяем что нет критичных ошибок выполнения
            if (!job.IsSuccess())
            {
                Assert.Fail(job.GetMessagesText());
            }

            // Проверяем что записи помечены сессий
            var uow = CreateUow();
            var repo = uow.LoyaltyClientUpdateRepository;

            var datas = repo.GetAll().Where(x => ids.Contains(x.SeqId)).ToArray();

            Assert.IsTrue(datas.All(x => x.SendEtlSessionId == sessionId));

            // Проверяем выгруженный файл
            var tempFolder = Path.Combine(Path.GetTempPath(), EmailSubject);

            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            EmailHelper.DownloadFilesFromTeradata(EmailSubject, tempFolder);

            VtbEncryptionHelper.Decrypt(tempFolder);

            int filesCount;
            var targetFileContent = FileReaderHelper.ReadAllLinesFromFolderFiles(tempFolder, out filesCount);
            Assert.IsTrue(filesCount > 0, "С почтового ящика не было загружено ни одного файла");

            Assert.IsTrue(targetFileContent.Count > 1);

            Assert.IsTrue(
                !targetFileContent.All(x => x.Contains("FirstName1")),
                "Не должно быть отправки данных FirstName = 'FirstName1' так как для это clientId есть более поздний FirstName = FirstName3");
            Assert.IsTrue(targetFileContent.Any(x => x.Contains("FirstName2")));
            Assert.IsTrue(targetFileContent.Any(x => x.Contains("FirstName3")));
        }

        private static IEnumerable<int> InsertTestData()
        {
            var uow = CreateUow();
            var repo = uow.LoyaltyClientUpdateRepository;

            foreach (var value in uow.ProfileCustomFieldsValuesRepository.GetAll())
            {
                uow.ProfileCustomFieldsValuesRepository.Delete(value);
            }

            foreach (var value in uow.ProfileCustomFieldsRepository.GetAll())
            {
                uow.ProfileCustomFieldsRepository.Delete(value);
            }

            uow.Save();

            var customField = new ProfileCustomField { Name = "Вес-" };
            uow.ProfileCustomFieldsRepository.Add(customField);

            uow.Save();

            var clientId = Guid.NewGuid().ToString();

            // NOTE: Это изменение НЕ должно пойти в файл
            var data1 = new LoyaltyClientUpdate
                        {
                            ClientId = clientId,
                            FirstName = "FirstName1",
                            LastName = "LastName",
                            MiddleName = "MiddleName",
                            MobilePhone = "1234567890",
                            MobilePhoneId = null,
                            Email = "email@email.ru",
                            Gender = Gender.Male,
                            BirthDate = DateTime.Now.AddYears(-18),
                            UpdateStatus = LoyaltyClientUpdateStatuses.Success,
                            InsertedDate = DateTime.Now.AddSeconds(-5)
                        };
            repo.Add(data1);
            uow.ProfileCustomFieldsValuesRepository.Add(
                new ProfileCustomFieldsValue { FieldId = customField.Id, ClientId = clientId, Value = "100-200 кг" });

            // NOTE: Это изменение должно пойти в файл
            var data2 = new LoyaltyClientUpdate
                        {
                            ClientId = Guid.NewGuid().ToString(),
                            FirstName = "FirstName2",
                            LastName = "LastName",
                            MiddleName = "MiddleName",
                            MobilePhone = "1234567890",
                            MobilePhoneId = null,
                            Email = "email@email.ru",
                            Gender = Gender.Male,
                            BirthDate = DateTime.Now.AddYears(-18),
                            UpdateStatus = LoyaltyClientUpdateStatuses.Success,
                            InsertedDate = DateTime.Now.AddSeconds(-3)
                        };
            repo.Add(data2);

            // NOTE: Это изменение должно пойти в файл
            var data3 = new LoyaltyClientUpdate
                        {
                            ClientId = clientId,
                            FirstName = "FirstName3",
                            LastName = "LastName",
                            MiddleName = "MiddleName",
                            MobilePhone = "1234567890",
                            MobilePhoneId = null,
                            Email = "email@email.ru",
                            Gender = Gender.Male,
                            BirthDate = DateTime.Now.AddYears(-18),
                            UpdateStatus = LoyaltyClientUpdateStatuses.Success,
                            InsertedDate = DateTime.Now.AddSeconds(-1)
                        };
            repo.Add(data3);

            uow.Save();

            yield return data1.SeqId;
            yield return data2.SeqId;
            yield return data3.SeqId;
        }
    }
}