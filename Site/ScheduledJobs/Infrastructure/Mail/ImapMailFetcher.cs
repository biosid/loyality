using System.Collections.Generic;
using System.Linq;
using AE.Net.Mail;

namespace ScheduledJobs.Infrastructure.Mail
{
    public class ImapMailFetcher : IMailFetcher
    {
        public ImapMailFetcher(string host, int port, string user, string password)
        {
            _server = new ImapClient(host, user, password, port: port);
        }

        private readonly ImapClient _server;

        public IEnumerable<FetchedMessage> GetAll()
        {
            var messages = _server.SearchMessages(SearchCondition.Undeleted()).ToArray();

            return messages.Select(m => new FetchedMessage(m.Value.Uid, m.Value));
        }

        public void Delete(params FetchedMessage[] message)
        {
            foreach (var fetched in message)
            {
                _server.DeleteMessage(fetched.Id);
            }
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}