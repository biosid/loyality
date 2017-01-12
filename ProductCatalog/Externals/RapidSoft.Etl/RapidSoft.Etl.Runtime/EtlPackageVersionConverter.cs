using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace RapidSoft.Etl.Runtime
{
    internal static class EtlPackageVersionConverter
    {
        #region Constants

        private const string COMPATIBILITY_FOLDER_NAME = "Compatibility";
        private const string DEFAULT_ENCODING_NAME = "UTF-8";

        #endregion

        #region Methods

        public static string Convert(string xml)
        {
            return Convert(xml, Encoding.GetEncoding(DEFAULT_ENCODING_NAME));
        }

        public static string Convert(string xml, Encoding encoding)
        {
            var thisType = typeof(EtlPackageVersionConverter);
            var converters = GetConverters(thisType.Assembly);

            foreach (var converter in converters)
            {
                xml = Convert(xml, converter, encoding);
            }

            return xml;
        }

        private static string Convert(string xml, XslCompiledTransform converter, Encoding encoding)
        {
            using (var inputStream = new StringReader(xml))
            {
                using (var reader = new XmlTextReader(inputStream))
                {
                    using (var writer = new StringWriter())
                    //using (var fileWriter = new EncodingStringWriter(encoding))
                    {
                        converter.Transform(reader, null, writer);
                        return writer.ToString();
                    }
                }
            }
        }

        private static List<XslCompiledTransform> GetConverters(Assembly resourceAssembly)
        {
            var converterResourceNames = GetConverterResourceNames(resourceAssembly);
            var converters = new List<XslCompiledTransform>();

            foreach (var converterResourceName in converterResourceNames)
            {
                using (var stream = resourceAssembly.GetManifestResourceStream(converterResourceName))
                {
                    using (var reader = new XmlTextReader(stream))
                    {
                        var converter = new XslCompiledTransform();
                        converter.Load(reader);

                        converters.Add(converter);
                    }
                }
            }

            return converters;
        }

        private static List<string> GetConverterResourceNames(Assembly resourceAssembly)
        {
            var resourceNamespace = resourceAssembly.GetName().Name + "." + COMPATIBILITY_FOLDER_NAME;
            var resourceNames = resourceAssembly.GetManifestResourceNames();
            var resourcePrefix = resourceNamespace + ".";

            var converterResourceNames = new List<string>();

            foreach (var resourceName in resourceNames)
            {
                if (resourceName.StartsWith(resourceNamespace))
                {
                    converterResourceNames.Add(resourceName);
                }
            }

            return converterResourceNames;
        }

        #endregion
    }
}
