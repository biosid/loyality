using Vtb24.Site.Content.Models.Exceptions;

namespace Vtb24.Site.Content.Pages.Models.Exceptions
{
    public class PageInvalidException : ContentManagementServiceException
    {
        public PageInvalidException()
            : base("Страница невалидна")
        {
        }
    }
}
