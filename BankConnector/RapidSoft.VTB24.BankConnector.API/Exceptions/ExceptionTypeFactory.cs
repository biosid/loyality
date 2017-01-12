using System;

namespace RapidSoft.VTB24.BankConnector.API.Exceptions
{
    internal static class ExceptionTypeFactory
    {
        // TODO: Modify when required
        public static ExceptionType GetExceptionType(Exception exception)
        {
            var exceptionType = exception.GetType();
            
            if (exceptionType == typeof(ClientAlreadyExistsException))
            {
                return ExceptionType.ClientAlreadyExistsException;
            }

            if (exceptionType == typeof(CardRegistrationException))
            {
                return ExceptionType.CardRegistrationException;
            }

            if (exceptionType == typeof(ProfileCustomFieldAlreadyExistsException))
            {
                return ExceptionType.ProfileCustomFieldAlreadyExists;
            }

            if (exceptionType == typeof(ClientNotFoundException))
            {
                return ExceptionType.ClientNotFound;
            }

            return ExceptionType.GeneralException;
        }
    }
}
