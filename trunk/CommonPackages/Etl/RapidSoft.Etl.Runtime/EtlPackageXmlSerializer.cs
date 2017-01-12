using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace RapidSoft.Etl.Runtime
{
    public sealed class EtlPackageXmlSerializer
    {
        #region Constructors

        public EtlPackageXmlSerializer()
        {
            var overridesProvider = new XmlOverridesProvider();
            _serializer = new Serializer(overridesProvider);
            _deserializer = new Deserializer(overridesProvider);
        }

        #endregion

        #region Fields

        private readonly Serializer _serializer;
        private readonly Deserializer _deserializer;

        #endregion

        #region Methods

        public string Serialize(EtlPackage package)
        {
            return _serializer.Serialize(package);
        }

        public EtlPackage Deserialize(string xml)
        {
            var package = _deserializer.Deserialize(xml);
            if (_deserializer.WasUnknownElements)
            {
                xml = EtlPackageVersionConverter.Convert(xml);
                package = _deserializer.Deserialize(xml);
                //todo: handle _deserializer.WasUnknownElements and log errors
            }

            return package;
        }

        #endregion

        #region Nested classes

        private sealed class Serializer
        {
            #region Constructors

            public Serializer(XmlOverridesProvider overridesProvider)
            {
                if (overridesProvider == null)
                {
                    throw new ArgumentNullException("overridesProvider");
                }

                _overridesProvider = overridesProvider;
            }

            #endregion

            #region Fields

            private readonly XmlOverridesProvider _overridesProvider;

            #endregion

            #region Methods

            public string Serialize(EtlPackage package)
            {
                if (package == null)
                {
                    return null;
                }

                using (var writer = new StringWriter())
                {
                    var ser = new XmlSerializer(typeof(EtlPackage), _overridesProvider.GetXmlOverrides());
                    ser.Serialize(writer, package);

                    return writer.ToString();
                }
            }

            #endregion
        }

        private sealed class Deserializer
        {
            #region Constructors

            public Deserializer(XmlOverridesProvider overridesProvider)
            {
                if (overridesProvider == null)
                {
                    throw new ArgumentNullException("overridesProvider");
                }

                _overridesProvider = overridesProvider;
            }

            #endregion

            #region Fields

            private readonly XmlOverridesProvider _overridesProvider;
            private bool _wasUnknownElements;

            #endregion

            #region Properties

            public bool WasUnknownElements
            {
                [DebuggerStepThrough]
                get
                {
                    return _wasUnknownElements;
                }
            }

            #endregion

            #region Methods

            public EtlPackage Deserialize(string xml)
            {
                Reset();

                if (string.IsNullOrEmpty(xml))
                {
                    return null;
                }

                var ser = new XmlSerializer(typeof(EtlPackage), _overridesProvider.GetXmlOverrides());
                ser.UnknownElement += new XmlElementEventHandler(HandleUnknownElement);

                using (var reader = new StringReader(xml))
                {
                    var result = (EtlPackage)ser.Deserialize(reader);
                    return result;
                }
            }

            private void Reset()
            {
                _wasUnknownElements = false;
            }

            private void HandleUnknownElement(object sender, XmlElementEventArgs e)
            {
                _wasUnknownElements = true;
            }

            #endregion
        }

        private class XmlOverridesProvider
        {
            #region Constructors

            public XmlOverridesProvider()
            {
                _registeredStepTypes = GetStepTypes(GetKnownLibraries());
            }

            #endregion

            #region Fields

            private readonly Type[] _registeredStepTypes;

            #endregion

            #region Methods

            private static Assembly[] GetKnownLibraries()
            {
                var knownLibraries = new Assembly[]
                {
                    typeof(EtlStep).Assembly,
                };

                return knownLibraries;
            }

            private static Type[] GetStepTypes(Assembly[] assemblies)
            {
                var types = new List<Type>();
                var baseType = typeof(EtlStep);

                foreach (var asm in assemblies)
                {
                    foreach (var type in asm.GetTypes())
                    {
                        if (baseType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsGenericTypeDefinition)
                        {
                            if (!types.Contains(type))
                            {
                                types.Add(type);
                            }
                        }
                    }
                }

                return types.ToArray();
            }

            public XmlAttributeOverrides GetXmlOverrides()
            {
                var overrides = new XmlAttributeOverrides();
                AddPackageStepOverrides(overrides);
                AddFunctionOverrides(overrides);
                
                return overrides;
            }

            private void AddPackageStepOverrides(XmlAttributeOverrides overrides)
            {
                var attributes = new XmlAttributes();
                foreach (var stepType in _registeredStepTypes)
                {
                    attributes.XmlArrayItems.Add(new XmlArrayItemAttribute(GetStepXmlElementName(stepType), stepType));
                }

                overrides.Add(typeof(EtlPackage), "Steps", attributes);
            }

            private static void AddFunctionOverrides(XmlAttributeOverrides overrides)
            {
                var attributes = new XmlAttributes();
                var asm = typeof(EtlValueFunction).Assembly;
                
                foreach (var functionType in asm.GetTypes())
                {
                    if (functionType.IsClass && !functionType.IsAbstract && typeof(EtlValueFunction).IsAssignableFrom(functionType))
                    {
                        attributes.XmlArrayItems.Add(new XmlArrayItemAttribute(GetFunctionXmlElementName(functionType), functionType));
                    }
                }

                overrides.Add(typeof(EtlValueTranslation), "Functions", attributes);
            }

            private static string GetStepXmlElementName(Type stepType)
            {
                return TrimName(stepType.Name, "Etl", "Step");
            }

            private static string GetFunctionXmlElementName(Type stepType)
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

            #endregion
        }

        #endregion
    }
}
