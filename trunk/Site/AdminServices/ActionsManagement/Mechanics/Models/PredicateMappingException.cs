using System;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models
{
    public class PredicateMappingException : InvalidOperationException
    {
        public PredicateMappingException(string message)
            : base(message)
        {
        }

        public PredicateMappingException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
