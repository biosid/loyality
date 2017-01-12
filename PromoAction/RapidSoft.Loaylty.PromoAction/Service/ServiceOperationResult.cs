namespace RapidSoft.Loaylty.PromoAction.Service
{
    using System;

    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;

    public class ServiceOperationResult
    {
        public static T BuildErrorResult<T>(Exception e) where T : ResultBase, new()
        {
            var opex = e as OperationException;
            if (opex != null)
            {
                return new T
                           {
                               ResultCode = opex.ResultCode,
                               ResultDescription = opex.Message,
                               Success = false
                           };
            }

            if (e is ArgumentNullException || e is ArgumentException)
            {
                return new T { ResultCode = ResultCodes.INVALID_PARAMETER_VALUE, ResultDescription = e.ToString() };
            }

            return new T
                       {
                           ResultCode = ResultCodes.UNKNOWN_ERROR, 
                           ResultDescription = e.ToString(), 
                           Success = false
                       };
        }
    }
}