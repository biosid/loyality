using System;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Models
{
    public class ActionConditionalFactorsException : Exception
    {
        public ActionConditionalFactorsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
