using System.Collections.Generic;

namespace Rapidsoft.VTB24.Reports.Statistics
{
    public interface IIisLogsJobs<TItem>
        where TItem : class
    {
        IEnumerable<TItem> CurrentJobItems { get; }

        void PeekJob();

        void NotifyJobStarted();

        void NotifyJobCancelled();

        void NotifyJobSucceeded();

        void NotifyJobFailed();

        void SaveBatch(TItem[] batch);
    }
}
