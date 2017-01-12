namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    public class ImportProductsFromYAMLResult : ResultBase
    {
        public DocumentError[] TopErrors
        {
            get;
            set;
        }
    }
}