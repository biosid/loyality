namespace RapidSoft.VTB24.VtbEncryption
{
    using System.Configuration;

    public static class VtbEncryptionHelper
    {
        private static readonly IVtbEncryption Encryption;

        static VtbEncryptionHelper()
        {
            var key = ConfigurationManager.AppSettings["VtbEncryption"];

            if (key == "VtbEncryptionStub")
            {
                // NOTE: Используем стаб реализацию шифровальщика
                Encryption = new VtbEncryptionStub();
            }
            else
            {
                // NOTE: Используем прод реализацию шифровальщика
                Encryption = new VtbEncryption();
            }
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
