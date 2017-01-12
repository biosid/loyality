using System;
using System.Linq;
using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

namespace Rapidsoft.Loyalty.NotificationSystem.DataAccess.Repositories
{
    public class EmailBlackListRepository : IEmailBlackListRepository
    {
        public void Add(string email)
        {
            using (var ctx = new NotificationSystemContext())
            {
                var emailBlackList = ctx.EmailBlackList.SingleOrDefault(e => e.ClientEmail == email);

                if (emailBlackList != null)
                {
                    return;
                }

                ctx.EmailBlackList.Add(new EmailBlackList
                {
                    ClientEmail = email,
                    InsertedDate = DateTime.Now
                });
                ctx.SaveChanges();
            }
        }

        public void Remove(string email)
        {
            using (var ctx = new NotificationSystemContext())
            {
                var emailBlackList = ctx.EmailBlackList.SingleOrDefault(e => e.ClientEmail == email);

                if (emailBlackList == null)
                {
                    return;
                }

                ctx.EmailBlackList.Remove(emailBlackList);
                ctx.SaveChanges();
            }
        }

        public EmailBlackList GetByEmail(string email)
        {
            using (var ctx = new NotificationSystemContext())
            {
                return ctx.EmailBlackList.SingleOrDefault(e => e.ClientEmail == email);
            }
        }
    }
}
