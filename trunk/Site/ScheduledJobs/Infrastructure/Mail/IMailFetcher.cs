using System;
using System.Collections.Generic;

namespace ScheduledJobs.Infrastructure.Mail
{
    public interface IMailFetcher : IDisposable
    {
        IEnumerable<FetchedMessage> GetAll();

        void Delete(params FetchedMessage[] message);
    }
}