using System;

namespace Vtb24.Arms.AdminServices.Models
{
    public abstract class ComponentException : ServiceException
    {
        public int ResultCode { get; set; }

        protected ComponentException(string component, int resultCode, string codeDescription, Exception innerException = null)
            : base(string.Format("[{0}]: {1} - {2}", component, resultCode, codeDescription), innerException)
        {
            ResultCode = resultCode;

            Component = component;

            Description = codeDescription;
        }
    }
}