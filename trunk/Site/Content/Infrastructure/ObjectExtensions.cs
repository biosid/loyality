using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace Vtb24.Site.Content.Infrastructure
{
    internal static class ObjectExtensions
    {
        public static string GetHash<T>(this object instance) where T : HashAlgorithm, new()
        {
            var cryptoServiceProvider = new T();
            return computeHash(instance, cryptoServiceProvider);
        }

        public static string GetHashString(this string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();

            StringBuilder sb = new StringBuilder();
            foreach (byte b in algorithm.ComputeHash(Encoding.Unicode.GetBytes(inputString)))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static string GetKeyedHash<T>(this object instance, byte[] key) where T : KeyedHashAlgorithm, new()
        {
            var cryptoServiceProvider = new T { Key = key };
            return computeHash(instance, cryptoServiceProvider);
        }

        public static string GetMD5Hash(this object instance)
        {
            return instance.GetHash<MD5CryptoServiceProvider>();
        }

        public static string GetSHA1Hash(this object instance)
        {
            return instance.GetHash<SHA1CryptoServiceProvider>();
        }

        private static string computeHash<T>(object instance, T cryptoServiceProvider) where T : HashAlgorithm, new()
        {
            var serializer = new DataContractSerializer(instance.GetType());
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, instance);
                cryptoServiceProvider.ComputeHash(memoryStream.ToArray());
                return Convert.ToBase64String(cryptoServiceProvider.Hash);
            }
        }
    }
}
