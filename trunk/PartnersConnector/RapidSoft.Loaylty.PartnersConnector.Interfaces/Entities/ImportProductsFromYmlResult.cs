namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    public class ImportProductsFromYmlResult : ResultBase
    {
        public int? TaskId { get; set; }

        public static ImportProductsFromYmlResult BuildSuccess(int taskId)
        {
            return new ImportProductsFromYmlResult
                       {
                           TaskId = taskId,
                           ResultCode = (int)ResultCodes.Success,
                           ResultDescription = null
                       };
        }
    }
}
