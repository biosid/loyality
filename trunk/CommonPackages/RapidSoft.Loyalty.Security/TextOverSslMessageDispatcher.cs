using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RapidSoft.Loyalty.Security
{
    using RapidSoft.Loaylty.Logging;

    [Serializable]
    public class TextOverSslMessageDispatcher : ITextMessageDispatcher
    {
        private readonly X509Certificate2 _certificate;
        private readonly Encoding _messageEncoding;

        private readonly ILog _log = LogManager.GetLogger(typeof (TextOverSslMessageDispatcher));

        public TextOverSslMessageDispatcher(X509Certificate2 certificate) : this(certificate, Encoding.UTF8)
        {
        }

        public TextOverSslMessageDispatcher(X509Certificate2 certificate, Encoding messageEncoding)
        {
            _certificate = certificate;
            _messageEncoding = messageEncoding;
        }

        public string Send(Uri serviceUri, string message)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                var req = (HttpWebRequest)WebRequest.Create(serviceUri);

                req.Method = "POST";
                req.ContentType = "text/xml";
                req.KeepAlive = false;
                req.ProtocolVersion = HttpVersion.Version11;
                if (_certificate != null)
                {
                    req.ClientCertificates.Add(_certificate);
                }

                req.PreAuthenticate = true;
                var buffer = _messageEncoding.GetBytes(message);
                req.ContentLength = buffer.Length;

                var writer = req.GetRequestStream();
                writer.Write(buffer, 0, buffer.Length);
                writer.Close();

                var rsp = req.GetResponse();
                var responseStream = new StreamReader(rsp.GetResponseStream());

                return responseStream.ReadToEnd();
            }
            catch (Exception ex)
            {
                _log.Error("Ошибка взаимодействия с партенром по адресу " + serviceUri, ex);
                throw;
            }
        }
    }
}
