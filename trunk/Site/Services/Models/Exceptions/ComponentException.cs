
namespace Vtb24.Site.Services.Models.Exceptions
{
    public abstract class ComponentException : ServiceException
    {
        public int ResultCode { get; set; }

        protected ComponentException (string component, int resultCode, string codeDescription)
            : base(string.Format("[{0}]: {1} - {2}", component, resultCode, codeDescription))
        {
            ResultCode = resultCode;
            
            Component = component;
            
            Description = codeDescription;
        }
    }
}
