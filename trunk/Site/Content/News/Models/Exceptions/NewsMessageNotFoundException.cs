using Vtb24.Site.Content.Models.Exceptions;

namespace Vtb24.Site.Content.News.Models.Exceptions
{
    public class NewsMessageNotFoundException : ContentManagementServiceException
    {
        public long Id { get; set; }

        public NewsMessageNotFoundException(long id) : 
            base(string.Format("Новость с id: {0} не найдена", id))
        {
            Id = id;
        }
    }
}
