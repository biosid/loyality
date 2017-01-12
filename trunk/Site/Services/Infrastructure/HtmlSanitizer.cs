using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Vtb24.Site.Services.Infrastructure
{
    public static class HtmlSanitizer
    {
        #region API

        public static string StripHtmlTags(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(text);
            var noContent = doc.DocumentNode.SelectNodes("//style|//script|//noscript|//comment()");
            if (noContent != null)
            {
                foreach (var node in noContent.ToArray())
                {
                    node.Remove();
                }
            }
            return doc.DocumentNode.InnerText.Trim();
        }

        public static string SanitizeHtml(string text, Whitelist whitelist)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(text);

            const string childSelector = "./*|./text()";

            var nodes = new Queue<HtmlNode>(doc.DocumentNode.SelectNodes(childSelector));
            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                var parentNode = node.ParentNode;

                if (whitelist.Contains(node.Name))
                {
                    var allowedTag = whitelist[node.Name];
                    var invalidAttributes = node
                        .Attributes
                        .Where(a => !allowedTag.IsValidAttribute(a.Name, a.Value))
                        .ToArray();
                    foreach (var attr in invalidAttributes)
                    {
                        attr.Remove();
                    }
                    continue;
                }

                // теги STYLE, SCRIPT и NOSCRIPT необходимо удалять целиком
                if (node.NodeType == HtmlNodeType.Element && (node.Name == "style" || node.Name == "script" || node.Name == "noscript"))
                {
                    node.Remove();
                    continue;
                }

                var childNodes = node.SelectNodes(childSelector);

                if (childNodes != null)
                {
                    foreach (var child in childNodes)
                    {
                        nodes.Enqueue(child);
                        parentNode.InsertBefore(child, node);
                    }
                }

                node.Remove();
            }

            return doc.DocumentNode.InnerHtml.Trim();
        }

        #endregion


        #region Models

        public class Whitelist
        {
            private readonly Dictionary<string, WhitelistElement> _index = new Dictionary<string, WhitelistElement>
            {
                { "#text", new WhitelistElement("#text") }
            };

            public bool DontSanitizeHref { get; set; }

            public bool DontSanitizeStyle { get; set; }

            public WhitelistElement this[string name]
            {
                get { return _index[name.ToLower()]; }
            }

            public Whitelist AllowElements(params WhitelistElement[] elements)
            {
                foreach (var tag in elements)
                {
                    _index[tag.Name.ToLower()] = tag;
                }
                return this;
            }

            public Whitelist AllowElements(params string[] names)
            {
                foreach (var tag in names)
                {
                    _index[tag.ToLower()] = new WhitelistElement(tag) { Whitelist = this };
                }
                return this;
            }

            public Whitelist AllowElement(string name, params string[] attributes)
            {
                _index[name.ToLower()] = new WhitelistElement(name, attributes) { Whitelist = this };
                return this;
            }
            

            public bool Contains(string name)
            {
                return _index.ContainsKey(name.ToLower());
            }
        }

        public class WhitelistElement
        {
            public WhitelistElement(string name, params string[] attrs)
            {
                Name = name;
                AllowedAttributes = new List<string>(attrs);
            }

            internal Whitelist Whitelist { get; set; }

            public string Name { get; set; }


            public IList<string> AllowedAttributes { get; private set; }

            public bool IsValidAttribute(string name, string value = "")
            {
                var normalizedName = name.ToLower();
                var allowed = AllowedAttributes.Contains(normalizedName);

                if (!allowed || value == null)
                {
                    return allowed;
                }

                // дополнительные проверки
                var normalizedValue = value.Trim().ToLower();
                switch (normalizedName)
                {
                    case "href":
                        return Whitelist.DontSanitizeHref || !normalizedValue.StartsWith("javascript:");
                    case "style":
                        return Whitelist.DontSanitizeStyle || !normalizedValue.Contains("expression(");
                    default:
                        return true;
                }
            }
        }

        #endregion
    }
}