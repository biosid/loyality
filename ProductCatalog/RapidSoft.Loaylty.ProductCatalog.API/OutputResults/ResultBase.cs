namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ResultBase
    {
        /// <summary>
        /// Признак успешного выполнения операции.
        /// </summary>
        [DataMember]
        public bool Success
        {
            get { return ResultCode == ResultCodes.SUCCESS; }
            set {}
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

        public static ResultBase BuildFail(int code, string description)
        {
            return new ResultBase
                   {
                       ResultCode = code,
                       ResultDescription = description,
                       Success = false
                   };
        }

        public static ResultBase BuildNotFound(string description)
        {
            return BuildFail(ResultCodes.NOT_FOUND, description);
        }

        public static ResultBase BuildSuccess()
        {
            return new ResultBase
                   {
                       ResultCode = ResultCodes.SUCCESS,
                       ResultDescription = null,
                       Success = true
                   };
        }
    }
}