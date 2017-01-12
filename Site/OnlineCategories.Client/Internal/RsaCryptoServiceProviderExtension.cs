using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Vtb24.OnlineCategories.Client.Internal
{
    /// <summary>Extension method for initializing a RSACryptoServiceProvider from PEM data string.</summary>
    public static class RsaCryptoServiceProviderExtension
    {
        #region Methods

        /// <summary>Extension method which initializes an RSACryptoServiceProvider from a DER public key blob.</summary>
        public static void LoadPublicKeyDer(this RSACryptoServiceProvider provider, byte[] der)
        {
            var rsa = GetRsaFromDer(der);
            var publicKeyBlob = GetPublicKeyBlobFromRsa(rsa);
            provider.ImportCspBlob(publicKeyBlob);
        }

        /// <summary>Extension method which initializes an RSACryptoServiceProvider from a DER private key blob.</summary>
        public static void LoadPrivateKeyDer(this RSACryptoServiceProvider provider, byte[] der)
        {
            var privateKeyBlob = GetPrivateKeyDer(der);
            provider.ImportCspBlob(privateKeyBlob);
        }

        /// <summary>Extension method which initializes an RSACryptoServiceProvider from a PEM public key string.</summary>
        public static void LoadPublicKeyPem(this RSACryptoServiceProvider provider, string pem)
        {
            var der = GetDerFromPem(pem);
            LoadPublicKeyDer(provider, der);
        }

        /// <summary>Extension method which initializes an RSACryptoServiceProvider from a PEM private key string.</summary>
        public static void LoadPrivateKeyPem(this RSACryptoServiceProvider provider, string pem)
        {
            var der = GetDerFromPem(pem);
            LoadPrivateKeyDer(provider, der);
        }

        /// <summary>Returns a public key blob from an RSA public key.</summary>
        internal static byte[] GetPublicKeyBlobFromRsa(byte[] rsa)
        {
            UInt32 dwCertPublicKeyBlobSize = 0;

            if (!CryptDecodeObject(CryptEncodingFlags.X509AsnEncoding | CryptEncodingFlags.Pkcs7AsnEncoding,
                                   new IntPtr((int) CryptOutputTypes.RsaCspPublickeyblob), rsa, (UInt32) rsa.Length,
                                   CryptDecodeFlags.None, null, ref dwCertPublicKeyBlobSize))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            var data = new byte[dwCertPublicKeyBlobSize];

            if (!CryptDecodeObject(CryptEncodingFlags.X509AsnEncoding | CryptEncodingFlags.Pkcs7AsnEncoding,
                                   new IntPtr((int) CryptOutputTypes.RsaCspPublickeyblob), rsa, (UInt32) rsa.Length,
                                   CryptDecodeFlags.None, data, ref dwCertPublicKeyBlobSize))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return data;
        }

        /// <summary>Converts DER binary format to a CAPI CRYPT_PRIVATE_KEY_INFO structure.</summary>
        internal static byte[] GetPrivateKeyDer(byte[] der)
        {
            UInt32 dwRsaPrivateKeyBlobSize = 0;

            if (!CryptDecodeObject(CryptEncodingFlags.X509AsnEncoding | CryptEncodingFlags.Pkcs7AsnEncoding,
                                   new IntPtr((int) CryptOutputTypes.PkcsRsaPrivateKey),
                                   der, (UInt32) der.Length, CryptDecodeFlags.None, null, ref dwRsaPrivateKeyBlobSize))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            var data = new byte[dwRsaPrivateKeyBlobSize];

            if (!CryptDecodeObject(CryptEncodingFlags.X509AsnEncoding | CryptEncodingFlags.Pkcs7AsnEncoding,
                                   new IntPtr((int) CryptOutputTypes.PkcsRsaPrivateKey),
                                   der, (UInt32) der.Length, CryptDecodeFlags.None, data, ref dwRsaPrivateKeyBlobSize))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return data;
        }

        /// <summary>Converts DER binary format to a CAPI CERT_PUBLIC_KEY_INFO structure containing an RSA key.</summary>
        internal static byte[] GetRsaFromDer(byte[] der)
        {
            UInt32 dwCertPublicKeyInfoSize = 0;
            if (!CryptDecodeObject(CryptEncodingFlags.X509AsnEncoding | CryptEncodingFlags.Pkcs7AsnEncoding,
                                   new IntPtr((int) CryptOutputTypes.X509PublicKeyInfo),
                                   der, (UInt32) der.Length, CryptDecodeFlags.None, null, ref dwCertPublicKeyInfoSize))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            var data = new byte[dwCertPublicKeyInfoSize];

            if (!CryptDecodeObject(CryptEncodingFlags.X509AsnEncoding | CryptEncodingFlags.Pkcs7AsnEncoding,
                                   new IntPtr((int) CryptOutputTypes.X509PublicKeyInfo),
                                   der, (UInt32) der.Length, CryptDecodeFlags.None, data, ref dwCertPublicKeyInfoSize))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            byte[] publicKey;

            try
            {
                var info = (CertPublicKeyInfo) Marshal.PtrToStructure(handle.AddrOfPinnedObject(),
                                                                      typeof (CertPublicKeyInfo));
                publicKey = new byte[info.PublicKey.cbData];
                Marshal.Copy(info.PublicKey.pbData, publicKey, 0, publicKey.Length);
            }
            finally
            {
                handle.Free();
            }

            return publicKey;
        }

        /// <summary>Extracts the binary data from a PEM file.</summary>
        internal static byte[] GetDerFromPem(string pem)
        {
            UInt32 dwSkip, dwFlags;
            UInt32 dwBinarySize = 0;

            if (!CryptStringToBinary(pem, (UInt32) pem.Length, CryptStringFlags.CryptStringBase64Header,
                                     null, ref dwBinarySize, out dwSkip, out dwFlags))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            var der = new byte[dwBinarySize];

            if (!CryptStringToBinary(pem, (UInt32) pem.Length, CryptStringFlags.CryptStringBase64Header,
                                     der, ref dwBinarySize, out dwSkip, out dwFlags))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return der;
        }

        #endregion Methods

        #region P/Invoke Constants

        /// <summary>Enumeration derived from Crypto API.</summary>
        internal enum CryptAcquireContextFlags : uint
        {
            CryptNewkeyset = 0x8,
            CryptDeletekeyset = 0x10,
            CryptMachineKeyset = 0x20,
            CryptSilent = 0x40,
            CryptDefaultContainerOptional = 0x80,
            CryptVerifycontext = 0xF0000000
        }

        /// <summary>Enumeration derived from Crypto API.</summary>
        internal enum CryptProviderType : uint
        {
            ProvRsaFull = 1
        }

        /// <summary>Enumeration derived from Crypto API.</summary>
        internal enum CryptDecodeFlags : uint
        {
            None = 0,
            CryptDecodeAllocFlag = 0x8000
        }

        /// <summary>Enumeration derived from Crypto API.</summary>
        [Flags]
        internal enum CryptEncodingFlags : uint
        {
            Pkcs7AsnEncoding = 0x00010000,
            X509AsnEncoding = 0x00000001,
        }

        /// <summary>Enumeration derived from Crypto API.</summary>
        internal enum CryptOutputTypes
        {
            X509PublicKeyInfo = 8,
            RsaCspPublickeyblob = 19,
            PkcsRsaPrivateKey = 43,
            PkcsPrivateKeyInfo = 44
        }

        /// <summary>Enumeration derived from Crypto API.</summary>
        internal enum CryptStringFlags : uint
        {
            CryptStringBase64Header = 0,
            CryptStringBase64 = 1,
            CryptStringBinary = 2,
            CryptStringBase64Requestheader = 3,
            CryptStringHex = 4,
            CryptStringHexascii = 5,
            CryptStringBase64Any = 6,
            CryptStringAny = 7,
            CryptStringHexAny = 8,
            CryptStringBase64X509Crlheader = 9,
            CryptStringHexaddr = 10,
            CryptStringHexasciiaddr = 11,
            CryptStringHexraw = 12,
            CryptStringNocrlf = 0x40000000,
            CryptStringNocr = 0x80000000
        }

        #endregion P/Invoke Constants

        #region P/Invoke Structures

        /// <summary>Structure from Crypto API.</summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct CryptObjidBlob
        {
            internal UInt32 cbData;
            internal IntPtr pbData;
        }

        /// <summary>Structure from Crypto API.</summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct CryptAlgorithmIdentifier
        {
            internal IntPtr pszObjId;
            internal CryptObjidBlob Parameters;
        }

        /// <summary>Structure from Crypto API.</summary>
        [StructLayout(LayoutKind.Sequential)]
        struct CryptBitBlob
        {
            internal readonly UInt32 cbData;
            internal readonly IntPtr pbData;
            private readonly UInt32 cUnusedBits;
        }

        /// <summary>Structure from Crypto API.</summary>
        [StructLayout(LayoutKind.Sequential)]
        struct CertPublicKeyInfo
        {
            private readonly CryptAlgorithmIdentifier Algorithm;
            internal CryptBitBlob PublicKey;
        }

        #endregion P/Invoke Structures

        #region P/Invoke Functions

        /// <summary>Function for Crypto API.</summary>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptDestroyKey(IntPtr hKey);

        /// <summary>Function for Crypto API.</summary>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptImportKey(IntPtr hProv, byte[] pbKeyData, UInt32 dwDataLen, IntPtr hPubKey, UInt32 dwFlags, ref IntPtr hKey);

        /// <summary>Function for Crypto API.</summary>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptReleaseContext(IntPtr hProv, Int32 dwFlags);

        /// <summary>Function for Crypto API.</summary>
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptAcquireContext(ref IntPtr hProv, string pszContainer, string pszProvider, CryptProviderType dwProvType, CryptAcquireContextFlags dwFlags);

        /// <summary>Function from Crypto API.</summary>
        [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptStringToBinary(string sPem, UInt32 sPemLength, CryptStringFlags dwFlags, [Out] byte[] pbBinary, ref UInt32 pcbBinary, out UInt32 pdwSkip, out UInt32 pdwFlags);

        /// <summary>Function from Crypto API.</summary>
        [DllImport("crypt32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptDecodeObjectEx(CryptEncodingFlags dwCertEncodingType, IntPtr lpszStructType, byte[] pbEncoded, UInt32 cbEncoded, CryptDecodeFlags dwFlags, IntPtr pDecodePara, ref byte[] pvStructInfo, ref UInt32 pcbStructInfo);

        /// <summary>Function from Crypto API.</summary>
        [DllImport("crypt32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptDecodeObject(CryptEncodingFlags dwCertEncodingType, IntPtr lpszStructType, byte[] pbEncoded, UInt32 cbEncoded, CryptDecodeFlags flags, [In, Out] byte[] pvStructInfo, ref UInt32 cbStructInfo);

        #endregion P/Invoke Functions
    }
}
