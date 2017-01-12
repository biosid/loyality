using System.Data.Entity.Migrations;

namespace Rapidsoft.VTB24.Reports.Statistics.DataAccess.Migrations
{
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PixelReportItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        IpAddress = c.String(),
                        Agent = c.String(),
                        PixelReportId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PixelReports", t => t.PixelReportId, cascadeDelete: true)
                .Index(t => t.PixelReportId);
            
            CreateTable(
                "dbo.PixelReports",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Pixel = c.String(nullable: false, maxLength: 1024),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        CreateTimestamp = c.DateTime(nullable: false),
                        StartTimestamp = c.DateTime(),
                        FinishTimestamp = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PixelReportItems", "PixelReportId", "dbo.PixelReports");
            DropIndex("dbo.PixelReportItems", new[] { "PixelReportId" });
            DropTable("dbo.PixelReports");
            DropTable("dbo.PixelReportItems");
        }
    }
}
