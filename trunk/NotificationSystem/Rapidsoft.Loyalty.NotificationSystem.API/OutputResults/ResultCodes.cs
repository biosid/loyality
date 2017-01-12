namespace Rapidsoft.Loyalty.NotificationSystem.API.OutputResults
{
    public sealed class ResultCodes
    {
        #region Common

        public const int ARGUMENT_OUT_OF_RANGE = -2;
        public const int INVALID_PARAMETER_VALUE = -1;

        public const int INVALID_PARAM = -1;
        public const int SUCCESS = 0;
        public const int UNKNOWN_ERROR = 1;
        public const int NOT_FOUND = 2;

        public const int NOT_HAVE_PERMISSION = 1000;

        public const int THREAD_IS_CLOSED = 201;
        public const int THREAD_SINCE_UNTIL_MISMATCH = 202;

        #endregion
    }
}