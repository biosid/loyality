namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    public class ContextKeyProvider
    {
        public static string GetContextKey(string locationKladrCode, string audienceIds)
        {
            return string.Format("{0}/{1}", locationKladrCode, audienceIds);
        }
    }
}