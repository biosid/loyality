namespace RapidSoft.VTB24.BankConnector.Infrastructure.Security
{
    using System.Configuration;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    public static class VtbEncryptionHelper
    {
        private static readonly IVtbEncryption Encryption;

        static VtbEncryptionHelper()
        {
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            Encryption = (new UnityContainer().LoadConfiguration(section)).Resolve<IVtbEncryption>();
        }

        public static void Encrypt(string workspace)
        {
            Encryption.Encrypt(workspace);
        }

        public static void Decrypt(string workspace)
        {
            Encryption.Decrypt(workspace);
        }
    }
}
