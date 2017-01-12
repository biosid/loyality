namespace RapidSoft.GeoPoints
{
    using System;

    using RapidSoft.GeoPoints.Entities;
    using RapidSoft.GeoPoints.OutputResults;

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
                               ResultDescription = opex.ResultDescription,
                               Success = false
                           };
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