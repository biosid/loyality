namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;

    using API.OutputResults;

    public class ServiceOperationResult
    {
        public static T BuildErrorResult<T>(Exception e) where T : ResultBase, new()
        {
            var exception = e as OperationException;
            if (exception != null)
            {
                var opex = exception;
                var description = string.Format("{0}{1}Internal Info:{2}", opex.Message, Environment.NewLine, opex);
                return BuildFailResult<T>(opex.ResultCode, description);
            }

            if (e is ArgumentOutOfRangeException)
            {
                return BuildFailResult<T>(ResultCodes.ARGUMENT_OUT_OF_RANGE, e.ToString());
            }

            if (e is ArgumentNullException || e is ArgumentException)
            {
                return BuildFailResult<T>(ResultCodes.INVALID_PARAMETER_VALUE, e.ToString());
            }

            return BuildFailResult<T>(ResultCodes.UNKNOWN_ERROR, e.ToString());
        }

        public static T BuildFailResult<T>(int resultCode, string description) where T : ResultBase, new()
        {
            return new T { ResultCode = resultCode, ResultDescription = description };
        }
    }
}