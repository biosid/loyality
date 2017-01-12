using System;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Models
{
    public class ActionPredicateException : Exception
    {
        public ActionPredicateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
