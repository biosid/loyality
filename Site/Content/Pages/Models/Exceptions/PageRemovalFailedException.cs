using Vtb24.Site.Content.Models.Exceptions;

namespace Vtb24.Site.Content.Pages.Models.Exceptions
{
    public class PageRemovalFailedException : ContentManagementServiceException
    {
        public PageRemovalFailedException()
            : base("Ошибка при удалении страницы")
        {
        }
    }
}
