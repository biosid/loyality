namespace Vtb24.Site.Services.Infrastructure
{
    public enum HtmlSanitizerPresets
    {
        Default = 0,
        DefaultWithLinks,
        ProductInfo   
    }

    public static class HtmlSanitizerPresetsExtensions
    {
        public static readonly HtmlSanitizer.Whitelist Whitelist = new HtmlSanitizer.Whitelist()
            .AllowElements("b", "strong", "i", "em", "br", "p", "ul", "ol", "li");

        public static readonly HtmlSanitizer.Whitelist WhitelistWithLinks = new HtmlSanitizer.Whitelist()
            .AllowElements("b", "strong", "i", "em", "br", "p", "ul", "ol", "li")
            .AllowElement("a", "href", "target", "title");

        public static readonly HtmlSanitizer.Whitelist WhitelistForProductInfo = new HtmlSanitizer.Whitelist()
            .AllowElements("b", "strong", "i", "em", "br", "p", "ul", "ol", "li", "h1", "h2", "h3")
            .AllowElement("a", "href", "target", "title");


        public static HtmlSanitizer.Whitelist GetWhitelist(this HtmlSanitizerPresets preset)
        {
            switch (preset)
            {
                case HtmlSanitizerPresets.DefaultWithLinks:
                    return WhitelistWithLinks;
                case HtmlSanitizerPresets.ProductInfo:
                    return WhitelistForProductInfo;
                default:
                    return Whitelist;
            }
        }
    }

    
}