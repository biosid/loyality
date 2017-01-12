namespace Rapidsoft.Loyalty.NotificationSystem.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Rapidsoft.Loyalty.NotificationSystem.Entities;

    public class ClientInboxProfileRepository : IClientInboxProfileRepository
    {
        public ClientProfile Get(string clientId)
        {
            using (var ctx = new InboxContext())
            {
                var profile = ctx.ClientProfiles.SingleOrDefault(x => x.ClientId == clientId);

                return profile;
            }
        }

        public void Save(ClientProfile profile)
        {
            using (var ctx = new InboxContext())
            {
                var exists = ctx.ClientProfiles.SingleOrDefault(x => x.ClientId == profile.ClientId);

                if (exists == null)
                {
                    ctx.ClientProfiles.Add(profile);
                }
                else
                {
                    exists.TotalCount = profile.TotalCount;
                    exists.UnreadCount = profile.UnreadCount;

                    ctx.ClientProfiles.Attach(exists);
                    ctx.Entry(exists).State = EntityState.Modified;
                }

                ctx.SaveChanges();
            }
        }

        public bool Exist(string clientId)
        {
            using (var ctx = new InboxContext())
            {
                return ctx.ClientProfiles.Any(c => c.ClientId == clientId);
            }
        }
    }
}