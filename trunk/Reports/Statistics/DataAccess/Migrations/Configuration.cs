using System.Data.Entity.Migrations;

namespace Rapidsoft.VTB24.Reports.Statistics.DataAccess.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<StatisticsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Vtb24Statistics";
            MigrationsDirectory = "DataAccess\\Migrations";
        }

        protected override void Seed(StatisticsContext context)
        {
        }
    }
}
