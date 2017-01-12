namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ResultBase
    {
        /// <summary>
        /// Признак успешного выполнения операции.
        /// </summary>
        public bool Success
        {
            get { return ResultCode == ResultCodes.Common.SUCCESS; }
        }

        /// <summary>
        /// Код возврата.
        /// </summary>
        [DataMember]
        public int ResultCode { get; set; }

        /// <summary>
        /// Описание ошибки.
        /// </summary>
        [DataMember]
        public string ResultDescription { get; set; }
    }
}
