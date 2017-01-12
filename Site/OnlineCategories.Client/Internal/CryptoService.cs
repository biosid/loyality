using System;
using System.Security.Cryptography;
using System.Text;
using Vtb24.OnlineCategories.Client.Exceptions;

namespace Vtb24.OnlineCategories.Client.Internal
{
    internal class CryptoService
    {
        public string CresteRequestSignature(byte[] requestBytes)
        {
            try
            {
                var signatureBytes = PrivateRsa.SignData(requestBytes, Sha1);

                var signature = Convert.ToBase64String(signatureBytes);

                return signature;
            }
            catch (Exception ex)
            {
                throw new FailedToSignRequest(ex);
            }
        }

        public bool ValidateResponseSignature(byte[] responseBytes, string signature)
        {
            try
            {
                var signatureBytes = Convert.FromBase64String(signature);

                var isValid = PublicRsa.VerifyData(responseBytes, Sha1, signatureBytes);

                return isValid;
            }
            catch (Exception ex)
            {
                throw new FailedToValidateResponse(ex);
            }
        }

        public string DecryptString(string encryptedStr, Encoding encoding)
        {
            if (encryptedStr == null)
            {
                return null;
            }

            try
            {
                var encryptedBytes = Convert.FromBase64String(encryptedStr);

                var decryptedBytes = PrivateRsa.Decrypt(encryptedBytes, false);

                var decryptedStr = encoding.GetString(decryptedBytes);

                return decryptedStr;
            }
            catch (Exception ex)
            {
                throw new FailedToDecrypt(ex);
            }
        }

        private RSACryptoServiceProvider PrivateRsa
        {
            get { return _privateRsa ?? (_privateRsa = CreatePrivateRsa()); }
        }

        private RSACryptoServiceProvider PublicRsa
        {
            get { return _publicRsa ?? (_publicRsa = CreatePublicRsa()); }
        }

        private SHA1CryptoServiceProvider Sha1
        {
            get { return _sha1 ?? (_sha1 = new SHA1CryptoServiceProvider()); }
        }

        private RSACryptoServiceProvider _privateRsa;
        private RSACryptoServiceProvider _publicRsa;
        private SHA1CryptoServiceProvider _sha1;

        private static RSACryptoServiceProvider CreatePrivateRsa()
        {
            try
            {
                var rsa = new RSACryptoServiceProvider(1024);
                rsa.LoadPrivateKeyPem(Configuration.PrivateKeyPem);
                return rsa;
            }
            catch (Exception ex)
            {
                throw new FailedToInitializePrivateRsa(ex);
            }
        }

        private static RSACryptoServiceProvider CreatePublicRsa()
        {
            try
            {
                var rsa = new RSACryptoServiceProvider(1024);
                rsa.LoadPublicKeyPem(Configuration.GatewayPublicKeyPem);
                return rsa;
            }
            catch (Exception ex)
            {
                throw new FailedToInitializePublicRsa(ex);
            }
        }
    }
}
