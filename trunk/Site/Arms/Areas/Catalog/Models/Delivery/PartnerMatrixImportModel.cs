namespace Vtb24.Arms.Catalog.Models.Delivery
{
    public class PartnerMatrixImportModel
    {
        public string Title { get; set; }

        public string MenuId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string BackUrl { get; set; }

        public string PostController { get; set; }

        public string PostAction { get; set; }

        public PartnerMatrixImportTaskModel[] ImportTasks { get; set; }

        public int TotalPages { get; set; }

        // ReSharper disable InconsistentNaming

        public int? page { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
