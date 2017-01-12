using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime
{
    public sealed class EtlPackageXmlSerializer
    {
        #region Fields

        private static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(EtlPackage), XmlOverridesProvider.Overrides);

        #endregion

        #region Methods

        public static string Serialize(EtlPackage package)
        {
            return Serializer.SerializePackage(package);
        }

        public static EtlPackage Deserialize(string xml)
        {
            bool wasUnknownElements;

            var package = Deserializer.DeserializePackage(xml, out wasUnknownElements);
            if (wasUnknownElements)
            {
                xml = EtlPackageVersionConverter.Convert(xml);
                package = Deserializer.DeserializePackage(xml, out wasUnknownElements);
                //todo: handle _deserializer.WasUnknownElements and log errors
            }

            return package;
        }

        #endregion

        #region Nested classes

        private static class Serializer
        {
            public static string SerializePackage(EtlPackage package)
            {
                if (package == null)
                {
                    return null;
                }

                using (var writer = new StringWriter())
                {
                    XmlSerializer.Serialize(writer, package);

                    return writer.ToString();
                }
            }
        }

        private static class Deserializer
        {
            public static EtlPackage DeserializePackage(string xml, out bool wasUnknownElements)
            {
                wasUnknownElements = false;

                if (string.IsNullOrEmpty(xml))
                {
                    return null;
                }

                var hasUnknownElements = false;
                XmlElementEventHandler handleUnknownElement = (sender, e) => { hasUnknownElements = true; };

                EtlPackage etlPackage;

                XmlSerializer.UnknownElement += handleUnknownElement;

                using (var reader = new StringReader(xml))
                {
                    etlPackage = (EtlPackage)XmlSerializer.Deserialize(reader);
                }

                XmlSerializer.UnknownElement -= handleUnknownElement;
                wasUnknownElements = hasUnknownElements;

                return etlPackage;
            }
        }

        private static class XmlOverridesProvider
        {
            public static XmlAttributeOverrides Overrides { get; private set; }

            static XmlOverridesProvider()
            {
                Overrides = new XmlAttributeOverrides();

                Overrides.Add(typeof(EtlPackage), "Steps", GetXmlAttributes(typeof(EtlStep), ToStepXmlElementName));
                Overrides.Add(typeof(EtlPackage), "Functions", GetXmlAttributes(typeof(EtlValueFunction), ToFunctionXmlElementName));
            }

            private static IEnumerable<Type> EnumerateTypes(Type baseType)
            {
                return baseType.Assembly
                               .GetTypes()
                               .Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsGenericTypeDefinition);
            }

            private static XmlAttributes GetXmlAttributes(Type type, Func<Type, string> toXmlElementName)
            {
                var attributes = new XmlAttributes();

                foreach (var attribute in EnumerateTypes(type).Select(t => new XmlArrayItemAttribute(toXmlElementName(t), t)))
                {
                    attributes.XmlArrayItems.Add(attribute);
                }

                return attributes;
            }

            private static string ToStepXmlElementName(Type stepType)
            {
                return TrimName(stepType.Name, "Etl", "Step");
            }

            private static string ToFunctionXmlElementName(Type stepType)
            {
                return TrimName(stepType.Name, "Etl", "Function");
            }

            private static string TrimName(string name, string prefix, string suffix)
            {
                if (name.StartsWith(prefix))
                {
                    name = name.Remove(0, prefix.Length);
                }

                if (name.EndsWith(suffix))
                {
                    name = name.Remove(name.Length - suffix.Length, suffix.Length);
                }

                return name;
            }
        }

        #endregion
    }
}
