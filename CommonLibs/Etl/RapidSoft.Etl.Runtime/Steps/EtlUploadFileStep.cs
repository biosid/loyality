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
    //todo: add UploadByteStatistics
    [Serializable]
    public sealed class EtlUploadFileStep : EtlStep
    {
        #region Constants

        private const string AUTHENTICATION_TYPE = "Basic";

        #endregion

        #region Constructors

        public EtlUploadFileStep()
        {
        }

        #endregion

        #region Properties

        [Category("2. Source")]
        public EtlFileInfo Source
        {
            get;
            set;
        }

        [Category("3. Destination")]
        public EtlResourceInfo Destination
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

            if (string.IsNullOrEmpty(this.Source.FilePath))
            {
                throw new InvalidOperationException("Source.FilePath cannot be empty");
            }

            if (this.Destination == null)
            {
                throw new InvalidOperationException("Destination cannot be null");
            }

            if (string.IsNullOrEmpty(this.Destination.Uri))
            {
                throw new InvalidOperationException("Destination.Uri cannot be empty");
            }

            if (string.IsNullOrEmpty(this.Destination.Method))
            {
                throw new InvalidOperationException("Destination.Method cannot be null");
            }

            if (!File.Exists(this.Source.FilePath))
            {
                //throw exception
            }

            var webRequest = WebRequest.Create(this.Destination.Uri);
            webRequest.Method = this.Destination.Method;

            if (!string.IsNullOrEmpty(this.Destination.ContentType))
            {
                webRequest.ContentType = this.Destination.ContentType;
            }

            foreach (var header in this.Destination.Headers)
            {
                webRequest.Headers[header.Name] = header.Value;
            }

            if (this.TimeoutMilliseconds != null)
            {
                webRequest.Timeout = this.TimeoutMilliseconds.Value == 0 ? System.Threading.Timeout.Infinite : this.TimeoutMilliseconds.Value;
            }

            if (this.Destination.Credential != null && !string.IsNullOrEmpty(this.Destination.Credential.UserName))
            {
                var credential = new NetworkCredential(this.Destination.Credential.UserName, this.Destination.Credential.Password);
                var credentialsCache = new CredentialCache();
                credentialsCache.Add(new Uri(this.Destination.Uri), AUTHENTICATION_TYPE, credential);
                webRequest.Credentials = credentialsCache;
            }

            if (this.Destination.AllowInvalidCertificates)
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

            byte[] fileBytes;

            using (var sourceStream = new StreamReader(this.Source.FilePath))
            {
                var encoding = Encoding.GetEncoding(this.Source.CodePage);
                fileBytes = encoding.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
            }

            webRequest.ContentLength = fileBytes.Length;

            var requestStream = webRequest.GetRequestStream();
            requestStream.Write(fileBytes, 0, fileBytes.Length);
            requestStream.Close();

            return new EtlStepResult(EtlStatus.Succeeded, null);
        }

        #endregion
    }
}
