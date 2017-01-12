namespace RapidSoft.Loaylty.PartnersConnector.Services.CryptoServices
{
    using System;
    using System.Security.Cryptography;

    internal class XmlPrivateKeyCryptoService : XmlKeyCryptoServiceBase
    {
        public XmlPrivateKeyCryptoService(string xmlPrivateKey)
            : base(xmlPrivateKey)
        {
        }

        public override string CreateSignature(string text)
        {
            if (text == null)
            {
                return null;
            }

            var rsa = new RSACryptoServiceProvider(1024);
            rsa.FromXmlString(this.XmlKey);

            var inputBytes = Encoding.GetBytes(text);

            byte[] encryptBytes;
            lock (SHA)
            {
                encryptBytes = rsa.SignData(inputBytes, SHA);
            }

            var result = Convert.ToBase64String(encryptBytes);
            return result;
        }

        public override string Decrypt(string text)
        {
            if (text == null)
            {
                return null;
            }

            var rsa = new RSACryptoServiceProvider(1024);
            rsa.FromXmlString(this.XmlKey);

            // var inputBytes = Encoding.GetBytes(text);
            var inputBytes = Convert.FromBase64String(text);

            var encryptBytes = rsa.Decrypt(inputBytes, false);

            var result = Encoding.GetString(encryptBytes);
            return result;
        }
    }
}