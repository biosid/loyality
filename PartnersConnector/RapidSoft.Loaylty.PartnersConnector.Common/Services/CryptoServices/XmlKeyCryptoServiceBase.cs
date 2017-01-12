namespace RapidSoft.Loaylty.PartnersConnector.Services.CryptoServices
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;

    internal abstract class XmlKeyCryptoServiceBase : ICryptoService
    {
        private readonly string xmlKey;

        protected static readonly Encoding Encoding = Encoding.UTF8;

        protected static readonly SHA1 SHA = new SHA1CryptoServiceProvider();

        protected XmlKeyCryptoServiceBase(string xmlKey)
        {
            this.xmlKey = xmlKey;
        }

        protected string XmlKey
        {
            get
            {
                return this.xmlKey;
            }
        }

        public bool VerifyData(string signature, string checkValue)
        {
            checkValue.ThrowIfNull("checkValue");
            signature.ThrowIfNull("signature");

            var rsa = new RSACryptoServiceProvider(1024);
            rsa.FromXmlString(this.xmlKey);

            var inputBytes = Encoding.GetBytes(checkValue);
            var singBytes = Convert.FromBase64String(signature);

            bool result;
            lock (SHA)
            {
                result = rsa.VerifyData(inputBytes, SHA, singBytes);
            }

            return result;
        }

        public abstract string CreateSignature(string text);

        public string Encrypt(string text)
        {
            if (text == null)
            {
                return null;
            }

            var rsa = new RSACryptoServiceProvider(1024);
            rsa.FromXmlString(this.XmlKey);

            var inputBytes = Encoding.GetBytes(text);

            var encryptBytes = rsa.Encrypt(inputBytes, false);

            // var result = Encoding.GetString(encryptBytes);
            var result = Convert.ToBase64String(encryptBytes);

            return result;
        }

        public abstract string Decrypt(string text);
    }
}