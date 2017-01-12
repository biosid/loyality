namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public class GiftsImportModel
    {
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public GiftsImportHistoryRowModel[] HistoryRows { get; set; }

        public int TotalPages { get; set; }

        public GiftsQueryModel GiftsQueryModel
        {
            get
            {
                return string.IsNullOrWhiteSpace(query)
                    ? new GiftsQueryModel { supplier = SupplierId }
                    : new GiftsQueryModel().MixQuery(query);
            }
        }

        // ReSharper disable InconsistentNaming

        public string query { get; set; }

        public int? page { get; set; }
        
        // ReSharper restore InconsistentNaming
    }
}