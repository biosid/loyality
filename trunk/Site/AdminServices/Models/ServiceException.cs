using System;

namespace Vtb24.Arms.AdminServices.Models
{
    public class ServiceException : Exception
    {
        public string Description { get; set; }

        public string Component { get; set; }

        protected ServiceException(string message, Exception innerException = null)
            : base(message, innerException)
        { }

        protected ServiceException(string component, string description, Exception innerException = null)
            : base(string.Format("[{0}]: {1}", component, description), innerException)
        {
            Component = component;
            Description = description;
        }
    }
}
