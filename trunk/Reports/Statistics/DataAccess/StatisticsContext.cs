using System.Data.Entity;
using Rapidsoft.VTB24.Reports.Statistics.DataAccess.Migrations;
using Rapidsoft.VTB24.Reports.Statistics.Entities.PixelReports;
using Rapidsoft.VTB24.Reports.Statistics.Entities.ProductViewEvents;

namespace Rapidsoft.VTB24.Reports.Statistics.DataAccess
{
    public class StatisticsContext : DbContext
    {
        public DbSet<PixelReport> Reports { get; set; }

        public DbSet<PixelReportItem> ReportItems { get; set; }

        public DbSet<ViewEventsDay> ViewEventsDays { get; set; }

        static StatisticsContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<StatisticsContext, Configuration>());
        }
    }
}
