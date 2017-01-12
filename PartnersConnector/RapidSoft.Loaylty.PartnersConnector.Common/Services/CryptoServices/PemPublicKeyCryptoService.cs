namespace RapidSoft.Loaylty.PartnersConnector.Services.CryptoServices
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PartnersConnector.Common.Services.CryptoServices;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;

    internal class PemPublicKeyCryptoService : ICryptoService
    {
        protected static readonly Encoding Encoding = Encoding.UTF8;

        protected static readonly SHA1 SHA = new SHA1CryptoServiceProvider();

        public PemPublicKeyCryptoService(string pemKey)
        {
            this.PemKey = pemKey;
        }

        protected string PemKey { get; private set; }

        public string CreateSignature(string text)
        {
            throw new NotSupportedException("Генерация подписи по public ключу не возможна");
        }

        public bool VerifyData(string signature, string checkValue)
        {
            checkValue.ThrowIfNull("checkValue");
            signature.ThrowIfNull("signature");

            var rsa = new RSACryptoServiceProvider(1024);
            rsa.LoadPublicKeyPEM(this.PemKey);

            var inputBytes = Encoding.GetBytes(checkValue);
            var singBytes = Convert.FromBase64String(signature);

            bool result;
            lock (SHA)
            {
                result = rsa.VerifyData(inputBytes, SHA, singBytes);
            }

            return result;
        }

        public string Encrypt(string text)
        {
            if (text == null)
            {
                return null;
            }

            var rsa = new RSACryptoServiceProvider(1024);
            rsa.LoadPublicKeyPEM(this.PemKey);

            var inputBytes = Encoding.GetBytes(text);

            var encryptBytes = rsa.Encrypt(inputBytes, false);

            var result = Convert.ToBase64String(encryptBytes);

            return result;
        }

        public string Decrypt(string text)
        {
            throw new NotSupportedException("Расшифровка по public ключу не возможна");
        }
    }
}