using System;

namespace Vtb24.Site.Content.Models
{
    public class ServiceException : Exception
    {
        public string Description { get; set; }

        public string Component { get; set; }

        protected ServiceException(string message)
            : base(message)
        { }

        protected ServiceException(string component, string description)
            : base(string.Format("[{0}]: {1}", component, description))
        {
            Component = component;
            Description = description;
        }
    }
}
