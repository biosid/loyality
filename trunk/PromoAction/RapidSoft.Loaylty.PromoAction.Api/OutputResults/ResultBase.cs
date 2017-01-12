namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
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
            get
            {
                return this.ResultCode == ResultCodes.SUCCESS;
            }
            // ReSharper disable ValueParameterNotUsed
            set
            {
            }
            // ReSharper restore ValueParameterNotUsed
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

        public static ResultBase BuildSuccess()
        {
            return new ResultBase { ResultCode = ResultCodes.SUCCESS, Success = true, ResultDescription = null };
        }

        public static ResultBase BuildFail(int code, string message)
        {
            return new ResultBase { ResultCode = code, Success = false, ResultDescription = message };
        }
    }
}