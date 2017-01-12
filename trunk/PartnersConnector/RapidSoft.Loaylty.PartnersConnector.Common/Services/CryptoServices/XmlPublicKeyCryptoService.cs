namespace RapidSoft.Loaylty.PartnersConnector.Services.CryptoServices
{
    using System;

    internal class XmlPublicKeyCryptoService : XmlKeyCryptoServiceBase
    {
        public XmlPublicKeyCryptoService(string xmlPublicKey)
            : base(xmlPublicKey)
        {
        }

        public override string CreateSignature(string text)
        {
            throw new NotSupportedException("Генерация подписи по public ключу не возможна");
        }

        public override string Decrypt(string text)
        {
            throw new NotSupportedException("Расшифровка по public ключу не возможна");
        }
    }
}