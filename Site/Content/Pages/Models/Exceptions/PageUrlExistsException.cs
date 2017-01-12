using System;
using Vtb24.Site.Content.Models.Exceptions;

namespace Vtb24.Site.Content.Pages.Models.Exceptions
{
    public class PageUrlExistsException : ContentManagementServiceException
    {
        public PageUrlExistsException(Guid pageId)
            : base(string.Format("Страница с данным URL уже существует"))
        {
            PageId = pageId;
        }

        public Guid PageId { get; private set; }
    }
}
