using System;

namespace Vtb24.Site.Services.Models.Exceptions
{
    public class ServiceException : Exception
    {
        public string Description { get; set; }

        public string Component { get; set; }

        public ServiceException(string message) : base(message)
        {}

        public ServiceException(string component, string description)
            : base (string.Format("[{0}]: {1}", component, description))
        {
            Component = component;
            Description = description;
        }
    }
}
