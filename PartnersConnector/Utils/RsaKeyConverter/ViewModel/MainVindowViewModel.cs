using OpenSSL.Core;
using RsaKeyConverter.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;

namespace RsaKeyConverter.ViewModel
{
    public class MainVindowViewModel : Observable
    {
        private static MainVindowViewModel instance = null;

        public static MainVindowViewModel Instance
        {
            get { return instance ?? (instance = new MainVindowViewModel()); }
        }

        private string xmlRsaPrivate;

        public string XmlRsaPrivate
        {
            get { return xmlRsaPrivate; }
            set { RaisePropertyChanged(() => XmlRsaPrivate, ref xmlRsaPrivate, value); }
        }

        private string xmlRsaPublic;

        public string XmlRsaPublic
        {
            get { return xmlRsaPublic; }
            set { RaisePropertyChanged(() => XmlRsaPublic, ref xmlRsaPublic, value); }
        }

        private string pemRsaPrivate;

        public string PemRsaPrivate
        {
            get { return pemRsaPrivate; }
            set { RaisePropertyChanged(() => PemRsaPrivate, ref pemRsaPrivate, value); }
        }

        private string pemRsaPublic;

        public string PemRsaPublic
        {
            get { return pemRsaPublic; }
            set { RaisePropertyChanged(() => PemRsaPublic, ref pemRsaPublic, value); }
        }

        private ICommand generateNewKey;
        private ICommand convertFromXmlToPem;
        private ICommand convertFromPemToXml;

        private ICommand clearKeys;

        public ICommand GenerateNewKey
        {
            get
            {
                return generateNewKey ?? (generateNewKey = new DelegateCommand(a =>
                {
                    var rsaCryptoServiceProvider = new RSACryptoServiceProvider();

                    XmlRsaPublic = rsaCryptoServiceProvider.ToXmlString(false);
                    XmlRsaPrivate = rsaCryptoServiceProvider.ToXmlString(true);

                    var rsaPrivateKey = new OpenSSL.Crypto.RSA
                    {
                        SecretPrimeFactorP = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).P),
                        SecretPrimeFactorQ = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).Q),
                        DmodP1 = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).DP),
                        DmodQ1 = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).DQ),
                        IQmodP = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).InverseQ),
                        PrivateExponent = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).D),
                        PublicExponent = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).Exponent),
                        PublicModulus = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).Modulus)
                    };

                    PemRsaPrivate = rsaPrivateKey.PrivateKeyAsPEM;
                    PemRsaPublic = rsaPrivateKey.PublicKeyAsPEM;
                }));
            }
        }

        public ICommand ConvertFromXmlToPem
        {
            get
            {
                return convertFromXmlToPem ?? (convertFromXmlToPem = new DelegateCommand(a =>
                {
                    using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
                    {
                        if (!string.IsNullOrEmpty(XmlRsaPrivate))
                        {
                            rsaCryptoServiceProvider.FromXmlString(XmlRsaPrivate);

                            XmlRsaPublic = rsaCryptoServiceProvider.ToXmlString(false);

                            var rsaPrivateKey = new OpenSSL.Crypto.RSA
                            {
                                SecretPrimeFactorP = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).P),
                                SecretPrimeFactorQ = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).Q),
                                DmodP1 = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).DP),
                                DmodQ1 = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).DQ),
                                IQmodP = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).InverseQ),
                                PrivateExponent = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).D),
                                PublicExponent = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).Exponent),
                                PublicModulus = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(true).Modulus)
                            };

                            PemRsaPrivate = rsaPrivateKey.PrivateKeyAsPEM;
                            PemRsaPublic = rsaPrivateKey.PublicKeyAsPEM;
                        }
                        else if (!string.IsNullOrEmpty(XmlRsaPublic))
                        {
                            rsaCryptoServiceProvider.FromXmlString(XmlRsaPublic);

                            var rsaPublicKey = new OpenSSL.Crypto.RSA
                            {
                                PublicExponent = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(false).Exponent),
                                PublicModulus = BigNumber.FromArray(rsaCryptoServiceProvider.ExportParameters(false).Modulus)
                            };

                            PemRsaPublic = rsaPublicKey.PublicKeyAsPEM;
                        }
                    }

                }, e => !string.IsNullOrEmpty(XmlRsaPrivate) || !string.IsNullOrEmpty(XmlRsaPublic)));
            }
        }

        public ICommand ConvertFromPemToXml
        {
            get
            {
                return convertFromPemToXml ?? (convertFromPemToXml = new DelegateCommand(a =>
                {
                    using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
                    {
                        if (!string.IsNullOrEmpty(PemRsaPrivate))
                        {
                            var rsaPrivateKey =
                                OpenSSL.Crypto.RSA.FromPrivateKey(
                                    new BIO(Encoding.ASCII.GetBytes(PemRsaPrivate)), (verify, userdata) => "12345", null);

                            PemRsaPublic = rsaPrivateKey.PublicKeyAsPEM;

                            var rsaParameters = new RSAParameters();
                            rsaParameters.P = rsaPrivateKey.SecretPrimeFactorP;
                            rsaParameters.Q = rsaPrivateKey.SecretPrimeFactorQ;
                            rsaParameters.DP = rsaPrivateKey.DmodP1;
                            rsaParameters.DQ = rsaPrivateKey.DmodQ1;
                            rsaParameters.InverseQ = rsaPrivateKey.IQmodP;
                            rsaParameters.D = rsaPrivateKey.PrivateExponent;
                            rsaParameters.Exponent = rsaPrivateKey.PublicExponent;
                            rsaParameters.Modulus = rsaPrivateKey.PublicModulus;

                            rsaCryptoServiceProvider.ImportParameters(rsaParameters);

                            XmlRsaPrivate = rsaCryptoServiceProvider.ToXmlString(true);
                            XmlRsaPublic = rsaCryptoServiceProvider.ToXmlString(false);
                        }
                        else if (!string.IsNullOrEmpty(PemRsaPublic))
                        {
                            var rsaPrivateKey = OpenSSL.Crypto.RSA.FromPublicKey(new BIO(Encoding.ASCII.GetBytes(PemRsaPublic)));

                            var rsaParameters = new RSAParameters();
                            rsaParameters.Exponent = rsaPrivateKey.PublicExponent;
                            rsaParameters.Modulus = rsaPrivateKey.PublicModulus;

                            rsaCryptoServiceProvider.ImportParameters(rsaParameters);

                            XmlRsaPublic = rsaCryptoServiceProvider.ToXmlString(false);
                        }
                    }
                }, e => !string.IsNullOrEmpty(PemRsaPrivate) || !string.IsNullOrEmpty(PemRsaPublic)));
            }
        }

        public ICommand ClearKeys
        {
            get
            {
                return clearKeys ?? (clearKeys = new DelegateCommand(a =>
                {
                    XmlRsaPrivate = null;
                    XmlRsaPublic = null;
                    PemRsaPrivate = null;
                    PemRsaPublic = null;
                }, e => true));
            }
        }
    }
}
