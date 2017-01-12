using System.Xml.Linq;

namespace RapidSoft.Extensions
{
    public static class LinqToXmlExtensions
    {
        public static string TryGetValue(this XElement node)
        {
            return node == null ? null : node.Value;
        }

        public static string TryGetValue(this XAttribute attr)
        {
            return attr == null ? null : attr.Value;
        }
    }
}