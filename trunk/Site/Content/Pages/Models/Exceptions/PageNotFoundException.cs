using Vtb24.Site.Content.Models.Exceptions;

namespace Vtb24.Site.Content.Pages.Models.Exceptions
{
    public class PageNotFoundException : ContentManagementServiceException
    {
        public PageNotFoundException()
            : base("Страница не найдена")
        {
        }
    }
}
