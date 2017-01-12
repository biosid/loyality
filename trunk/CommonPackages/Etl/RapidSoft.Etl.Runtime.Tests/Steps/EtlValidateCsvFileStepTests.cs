using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime.Steps;

namespace RapidSoft.Etl.Runtime.Tests.Steps
{
    [TestClass]
    public class EtlValidateCsvFileStepTests
    {
        #region Initialization

        [TestInitialize]
        public void Initialize()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        [DeploymentItem(@"Files\BadAllDataTypes.csv")]
        public void CanValidateAllDataTypesCsv()
        {
            var stepName = "Validate";

            var package = new EtlPackage
            {
                Id = Guid.NewGuid().ToString(),
                Steps =
                {
                    new EtlValidateCsvFileStep
                    {
                        Name = stepName,
                        Source = new EtlCsvFileInfo
                        {
                            FilePath = "BadAllDataTypes.csv",
                            CodePage = 1251,
                            FieldDelimiter = ";",
                            HasHeaders = true,
                        },
                        ErrorBehavior = EtlValidationErrorBehavior.ValidateAll,
                        BadFormatMessage = "Файл не является корректным CSV-файлом",
                        FieldValidationRules = 
                        {
                            new EtlFieldValidationRule
                            {
                                SourceName = "Byte",
                                MinLength = 1,
                                MaxLength = 3,
                                Regex = @"^\d+$",
                                ErrorMessage = "Столбец Byte должен содержать 3-хзначное число",
                            },
                            new EtlFieldValidationRule
                            {
                                SourceName = "Double",
                                MinLength = 1,
                                MaxLength = 15,
                                Regex = @"^\d+(\.\d+)?$",
                                ErrorMessage = "Столбец Double должен содержать число",
                            },
                            new EtlFieldValidationRule
                            {
                                SourceName = "Null",
                                CanBeNull = true,
                                CanBeEmpty = true,
                                MinLength = 4, //this must be ignored
                                MaxLength = 4, //this must be ignored
                                ErrorMessage = "Столбец Null должен быть пустым",
                            },
                        }
                    }
                }
            };

            var logger = new MemoryEtlLogger();
            var session = package.Invoke(logger);
            Assert.IsNotNull(session);

            Assert.AreEqual(1, logger.EtlSessions.Count);
            Assert.AreEqual(EtlStatus.Failed, logger.EtlSessions[0].Status);
            Assert.AreEqual(package.Id, logger.EtlSessions[0].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, logger.EtlSessions[0].EtlSessionId);

            var validationMessages = GetErrorMessages(logger.EtlMessages, stepName);
            Assert.AreEqual(3, validationMessages.Length);
            
            Assert.AreEqual(package.Id, validationMessages[0].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, validationMessages[0].EtlSessionId);
            Assert.AreEqual(stepName, validationMessages[0].EtlStepName);
            Assert.AreEqual(EtlMessageType.Error, validationMessages[0].MessageType);
            Assert.AreEqual("Столбец Double должен содержать число", validationMessages[0].Text);

            Assert.AreEqual(package.Id, validationMessages[1].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, validationMessages[1].EtlSessionId);
            Assert.AreEqual(stepName, validationMessages[1].EtlStepName);
            Assert.AreEqual(EtlMessageType.Error, validationMessages[1].MessageType);
            Assert.AreEqual("Столбец Byte должен содержать 3-хзначное число", validationMessages[1].Text);

            Assert.AreEqual(package.Id, validationMessages[2].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, validationMessages[2].EtlSessionId);
            Assert.AreEqual(stepName, validationMessages[2].EtlStepName);
            Assert.AreEqual(EtlMessageType.Error, validationMessages[2].MessageType);
            Assert.AreEqual("Файл не является корректным CSV-файлом", validationMessages[2].Text);
        }

        private static EtlMessage[] GetErrorMessages(IEnumerable<EtlMessage> messages, string stepName)
        {
            var errorMessages = new List<EtlMessage>();

            foreach (var msg in messages)
            {
                if (msg.EtlStepName == stepName && msg.MessageType == EtlMessageType.Error)
                {
                    errorMessages.Add(msg);
                }
            }

            return errorMessages.ToArray();
        }

        #endregion
    }
}
