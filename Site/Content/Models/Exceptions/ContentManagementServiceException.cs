namespace Vtb24.Site.Content.Models.Exceptions
{
    public class ContentManagementServiceException : ServiceException
    {
        public ContentManagementServiceException(string description)
            : base("Контент-редактор", description)
        {
        }
    }
}
