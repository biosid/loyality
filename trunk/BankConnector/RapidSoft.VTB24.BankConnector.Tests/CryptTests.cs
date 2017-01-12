using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.VTB24.BankConnector.Tests.Helpers;

namespace RapidSoft.VTB24.BankConnector.Tests
{
    using System.Reflection;

    using RapidSoft.VTB24.VtbEncryption;

    [TestClass]
    [DeploymentItem(@"Security", "Security")]
    public class CryptTests
    {
        private const string DecryptTestFolder = "DecryptTestFolder";
        private const string EncryptTestFolder = "EncryptTestFolder";

        private const string FileToDecrypt = "fileToDecrypt";
        private const string FileToDecryptExpected = "fileToDecryptExpected";

        private const string FileToEncrypt = "fileToEncrypt";
        private const string FileToEncryptExpected = "fileToEncryptExpected";

        private readonly IVtbEncryption encryption = new VtbEncryption();

        [TestMethod]
        [DeploymentItem(@"TestFiles\" + FileToDecrypt, DecryptTestFolder)]
        [DeploymentItem(@"TestFiles\" + FileToDecryptExpected)]        
        public void ShouldDecryptTargetedFolder()
        {
            var testFolder = Path.Combine(Environment.CurrentDirectory, DecryptTestFolder);
            var actualFile = Path.Combine(testFolder, FileToDecrypt);

            var expectedFile = Path.Combine(Environment.CurrentDirectory, FileToDecryptExpected);

            WriteInfo(testFolder);

            try
            {                
                encryption.Decrypt(testFolder);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Ошибка расшифровки {0}", testFolder), e);
            }

            Assert.IsTrue(
                FileReaderHelper.IsFilesEquals(expectedFile, actualFile),
                string.Format("Файл расшифровывается не верно. Ожидается ({0}) Фактически ({1})", expectedFile, actualFile));
        }

        private void WriteInfo(string testFolder)
        {
            string executingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var runnerDir = Path.Combine(Path.Combine(executingDir, "Security"));
            var runner = Path.Combine(runnerDir, "filepro.exe");
            var keysDir = Path.Combine(runnerDir, "decrypt");

            Console.WriteLine("testFolder:{0} exist:{1}", testFolder, Directory.Exists(testFolder));
            Console.WriteLine("executingDir:{0} exist:{1}", executingDir, Directory.Exists(executingDir));
            Console.WriteLine("runnerDir:{0} exist:{1}", runnerDir, Directory.Exists(runnerDir));
            Console.WriteLine("runner:{0} exist:{1}", runner, File.Exists(runner));
            Console.WriteLine("keysDir:{0} exist:{1}", keysDir, Directory.Exists(keysDir));
        }

        [TestMethod]
        [DeploymentItem(@"TestFiles\\" + FileToEncrypt, EncryptTestFolder)]
        [DeploymentItem(@"TestFiles\" + FileToEncryptExpected)]
        public void ShoultEncryptTargetedFolder()
        {
            var toEncryptFolder = Path.Combine(Environment.CurrentDirectory, EncryptTestFolder);
            var actualFile = Path.Combine(toEncryptFolder, FileToEncrypt);

            var expectedFile = Path.Combine(Environment.CurrentDirectory, FileToEncryptExpected);

            this.WriteInfo(toEncryptFolder);

            try
            {
                this.encryption.Encrypt(toEncryptFolder);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Ошибка шифрования {0}", toEncryptFolder), e);
            }

            Assert.IsFalse(FileReaderHelper.IsFilesEquals(expectedFile, actualFile), string.Format("Файл не шифруется "));
        }
    }
}
