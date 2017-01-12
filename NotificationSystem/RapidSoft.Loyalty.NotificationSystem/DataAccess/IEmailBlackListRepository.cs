using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

namespace Rapidsoft.Loyalty.NotificationSystem.DataAccess
{
    public interface IEmailBlackListRepository
    {
        void Add(string email);

        void Remove(string email);

        EmailBlackList GetByEmail(string email);
    }
}
