using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.Diagnostics;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Steps
{
    //todo: add DownloadByteStatistics
    [Serializable]
    public sealed class EtlDownloadFileStep : EtlStep
    {
        #region Constants

        private const string AUTHENTICATION_TYPE = "Basic";

        #endregion

        #region Constructors

        public EtlDownloadFileStep()
        {
        }

        #endregion

        #region Properties

        [Category("2. Source")]
        public EtlResourceInfo Source
        {
            get;
            set;
        }

        [Category("3. Destination")]
        public EtlFileInfo Destination
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public override EtlStepResult Invoke(EtlContext context, IEtlLogger logger)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (this.Source == null)
            {
                throw new InvalidOperationException("Source cannot be null");
            }

            if (string.IsNullOrEmpty(this.Source.Uri))
            {
                throw new InvalidOperationException("Source.Uri cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Source.Method))
            {
                throw new InvalidOperationException("Source.Method cannot be null");
            }

            if (this.Destination != null && string.IsNullOrEmpty(this.Destination.FilePath))
            {
                throw new InvalidOperationException("Destination.FilePath cannot be empty");
            }

            if (this.Destination != null)
            {
                var destinationFolderPath = Path.GetDirectoryName(this.Destination.FilePath);

                if (!string.IsNullOrEmpty(destinationFolderPath))
                {
                    if (!Directory.Exists(destinationFolderPath))
                    {
                        Directory.CreateDirectory(destinationFolderPath);
                    }
                }
            }

            var webRequest = WebRequest.Create(this.Source.Uri);
            webRequest.Method = this.Source.Method;

            if (!string.IsNullOrEmpty(this.Source.ContentType))
            {
                webRequest.ContentType = this.Source.ContentType;
            }

            foreach (var header in this.Source.Headers)
            {
                webRequest.Headers[header.Name] = header.Value;
            }

            if (this.TimeoutMilliseconds != null)
            {
                webRequest.Timeout = this.TimeoutMilliseconds.Value == 0 ? System.Threading.Timeout.Infinite : this.TimeoutMilliseconds.Value;
            }

            if (this.Source.Credential != null && !string.IsNullOrEmpty(this.Source.Credential.UserName))
            {
                var credential = new NetworkCredential(this.Source.Credential.UserName, this.Source.Credential.Password);
                var credentialsCache = new CredentialCache();
                credentialsCache.Add(new Uri(this.Source.Uri), AUTHENTICATION_TYPE, credential);
                webRequest.Credentials = credentialsCache;
            }

            if (this.Source.AllowInvalidCertificates)
            {
                ServicePointManager.ServerCertificateValidationCallback +=
                delegate
                (
                    object sender,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors policyErrors
                )
                {
                    return true;
                };
            }

            if (!string.IsNullOrEmpty(this.Source.Request))
            {
                var bytes = Encoding.UTF8.GetBytes(this.Source.Request);
                webRequest.ContentLength = bytes.Length;

                var request = webRequest.GetRequestStream();
                request.Write(bytes, 0, bytes.Length);
                request.Close();
            }

            using (var response = webRequest.GetResponse())
            {
                if (this.Destination != null)
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var fileStream = File.Create(this.Destination.FilePath))
                        {
                            var buffer = new byte[1024];
                            var bytesRead = 0;
                            //var bytesProcessed = 0;

                            do
                            {
                                bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                                fileStream.Write(buffer, 0, bytesRead);
                                //bytesProcessed += bytesRead;
                            } while (bytesRead > 0);
                        }
                    }
                }
            }

            return new EtlStepResult(EtlStatus.Succeeded, null);
        }

        #endregion
    }
}
