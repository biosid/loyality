using RapidSoft.VTB24.BankConnector.DataSource;
using RapidSoft.VTB24.BankConnector.Tests.Helpers;

namespace RapidSoft.VTB24.BankConnector.Tests
{
    using System;
    using System.Configuration;
    using System.IO;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;

    public class TestBase
    {
        private TestContext testContextInstance;

        private string encryptionTempFolder;

        private string decryptionTempFolder;

        public TestContext TestContext
        {
            get
            {
                return this.testContextInstance;
            }
            set
            {
                this.testContextInstance = value;
            }
        }

        public string EncryptionTempFolder 
        {   
            get
            {
                if (string.IsNullOrEmpty(encryptionTempFolder))
                {
                    encryptionTempFolder = CreateTempDir();
                }
                return encryptionTempFolder;
            } 
        }

        protected string CreateTempDir()
        {
            var folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }
        
        public string DecryptionTempFolder
        {
            get
            {
                if (string.IsNullOrEmpty(decryptionTempFolder))
                {
                    decryptionTempFolder = CreateTempDir();
                }
                return decryptionTempFolder;
            }
            set
            {
                decryptionTempFolder = value;
            }
        }

        protected static IUnitOfWork CreateUow()
        {
            return DataSourceHelper.CreateUow();
        }

        public virtual void Init()
        {
        }
    }
}