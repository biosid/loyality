namespace RapidSoft.Extensions
{
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public static class XmlSerializer
    {
        public static XElement SerializeToElement<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }

            var serialize = Serialize(obj);
            if (string.IsNullOrEmpty(serialize))
            {
                return null;
            }

            return XElement.Parse(serialize);
        }

        public static XmlDocument SerializeToXmlDocument<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }

            var serialize = Serialize(obj);
            if (string.IsNullOrEmpty(serialize))
            {
                return null;
            }

            var doc = new XmlDocument();
            doc.LoadXml(serialize);
            return doc;
        }

        public static string Serialize<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj.Equals(default(T)))
            {
                return null;
            }

            using (var sw = new StringWriter())
            {
                var ser = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                ser.Serialize(sw, obj);
                return sw.ToString();
            }
        }

        public static string SerializeWithNoNamespace<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj.Equals(default(T)))
            {
                return null;
            }

            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            using (var stream = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    var ser = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                    var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    ser.Serialize(writer, obj, emptyNamepsaces);
                    return stream.ToString();
                }
            }
        }

        public static T Deserialize<T>(XElement element) where T : class
        {
            if (element == null)
            {
                return null;
            }

            using (var sr = element.CreateReader())
            {
                var ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T)ser.Deserialize(sr);
            }
        }

        public static T Deserialize<T>(string xmlString) where T : class
        {
            if (string.IsNullOrEmpty(xmlString))
            {
                return null;
            }

            if (string.IsNullOrEmpty(xmlString))
            {
                return default(T);
            }

            using (var sr = new StringReader(xmlString))
            {
                var ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T)ser.Deserialize(sr);
            }
        }
    }
}