using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.Xml;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.Helpers
{
    public static class PredicateHelpers
    {
        #region JSON

        public static Predicate JsonToPredicate(string predicateJson)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(Predicate));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(predicateJson)))
            {
                try
                {
                    return jsonSerializer.ReadObject(stream) as Predicate;
                }
                catch (SerializationException ex)
                {
                    throw new PredicateMappingException("Json -> Predicate deserialization is failed", ex.InnerException);
                }
            }
        }

        public static string PredicateToJson(Predicate predicate)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(Predicate));
            using (var stream = new MemoryStream())
            {
                try
                {
                    jsonSerializer.WriteObject(stream, predicate);
                }
                catch (SerializationException ex)
                {
                    throw new PredicateMappingException("Predicate -> Json serialization is failed", ex.InnerException);
                }
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        #endregion

        #region XML

        public static Predicate XmlToPredicate(string predicateXml)
        {
            if (string.IsNullOrEmpty(predicateXml))
                return new Predicate();

            var xmlSerializer = new XmlSerializer(typeof(filter));
            using (var reader = new StringReader(predicateXml))
            {
                try
                {
                    return MappingsFromService.ToPredicate(xmlSerializer.Deserialize(reader) as filter);
                }
                catch (PredicateMappingException)
                {
                    throw;
                }
                catch (InvalidOperationException ex)
                {
                    throw new PredicateMappingException("Xml -> Predicate deserialization is failed", ex);
                }
            }
        }

        public static string PredicateToXml(Predicate predicate)
        {
            if (predicate == null || (predicate.Operation == null && predicate.Union == null))
                return null;

            var xmlSerializer = new XmlSerializer(typeof(filter));
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                try
                {
                    xmlSerializer.Serialize(xmlWriter, MappingsToService.ToXmlFilter(predicate));
                }
                catch (PredicateMappingException)
                {
                    throw;
                }
                catch (InvalidOperationException ex)
                {
                    throw new PredicateMappingException("Predicate -> Xml serialization is failed", ex);
                }
                return writer.ToString();
            }
        }

        public static ConditionalFactor[] XmlToConditionalFactors(string conditionalFactorsXml)
        {
            if (string.IsNullOrEmpty(conditionalFactorsXml))
                return new ConditionalFactor[0];

            var xmlSerializer = new XmlSerializer(typeof(ConditionalFactors));
            using (var reader = new StringReader(conditionalFactorsXml))
            {
                try
                {
                    var xmlConditionalFactors = xmlSerializer.Deserialize(reader) as ConditionalFactors;
                    return xmlConditionalFactors != null
                               ? xmlConditionalFactors.Factors.Select(MappingsFromService.ToConditionalFactor).ToArray()
                               : new ConditionalFactor[0];
                }
                catch (PredicateMappingException)
                {
                    throw;
                }
                catch (InvalidOperationException ex)
                {
                    throw new PredicateMappingException("Xml -> ConditionalFactors deserialization is failed", ex);
                }
            }
        }

        public static string ConditionalFactorsToXml(ConditionalFactor[] conditionalFactors)
        {
            if (conditionalFactors == null || conditionalFactors.Length == 0)
                return null;

            var xmlSerializer = new XmlSerializer(typeof(ConditionalFactors));
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                try
                {
                    var xmlConditionalFactors = new ConditionalFactors
                    {
                        Factors = conditionalFactors.Select(MappingsToService.ToXmlConditionalFactor).ToArray()
                    };

                    xmlSerializer.Serialize(xmlWriter, xmlConditionalFactors);
                }
                catch (PredicateMappingException)
                {
                    throw;
                }
                catch (InvalidOperationException ex)
                {
                    throw new PredicateMappingException("ConditionalFactors -> Xml serialization is failed", ex);
                }
                return writer.ToString();
            }
        }

        #endregion
    }
}
