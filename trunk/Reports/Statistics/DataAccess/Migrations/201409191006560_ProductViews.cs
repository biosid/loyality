namespace Rapidsoft.VTB24.Reports.Statistics.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductViews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ViewEventsDays",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        StartTimestamp = c.DateTime(),
                        FinishTimestamp = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        KeysCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ViewEventsDays");
        }
    }
}
