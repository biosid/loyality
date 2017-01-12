using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.Processing.Models.Exceptions
{
    public class ProcessingException : ComponentException
    {
        public ProcessingException(int statusCode, string codeDescription)
            : base("Процессинг", statusCode, codeDescription)
        {
        }
    }
}