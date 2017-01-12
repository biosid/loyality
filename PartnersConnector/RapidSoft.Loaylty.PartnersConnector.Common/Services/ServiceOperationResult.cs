using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;

namespace RapidSoft.Loaylty.PartnersConnector.Services
{
    using System;

    using PartnersConnector.Interfaces.Entities;

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
                           ResultCode = (int)ResultCodes.UnknownError, 
                           ResultDescription = e.ToString(), 
                           Success = false
                       };
        }
    }
}