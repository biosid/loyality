using System;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.AdminFeedback.Models.Exceptions
{
    public class AdminFeedbackException : ComponentException
    {
        public AdminFeedbackException(int resultCode, string codeDescription, Exception innerException = null) :
            base("Сервисы для переписки с клиентами", resultCode, codeDescription, innerException)
        {
        }
    }
}