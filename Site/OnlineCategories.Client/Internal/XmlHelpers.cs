using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Vtb24.OnlineCategories.Client.Exceptions;

namespace Vtb24.OnlineCategories.Client.Internal
{
    internal static class XmlHelpers
    {
        public static string RemoveSignature(string xml)
        {
            return Regex.Replace(xml, "(?<=Signature=\")[^\"]*", "");
        }

        public static T Deserialize<T>(byte[] bytes, Encoding encoding)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));

                using (var stream = new MemoryStream(bytes))
                using (var reader = new StreamReader(stream, encoding))
                {
                    var obj = (T) serializer.Deserialize(reader);
                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw new FailedToDeserializeXml(ex);
            }
        }

        public static byte[] Serialize<T>(T obj, Encoding encoding)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));

                using (var stream = new MemoryStream())
                using (var writer = new StreamWriter(stream, encoding))
                {
                    serializer.Serialize(writer, obj, new XmlSerializerNamespaces());
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new FailedToSerializeXml(ex);
            }
        }
    }
}
