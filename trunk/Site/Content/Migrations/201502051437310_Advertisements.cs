namespace Vtb24.Site.Content.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Advertisements : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientAdvertisements",
                c => new
                    {
                        Advertisement_Id = c.Long(nullable: false),
                        ClientId = c.String(nullable: false, maxLength: 255),
                        ShowCounter = c.Int(nullable: false, defaultValue: 0),
                    })
                .PrimaryKey(t => new { t.Advertisement_Id, t.ClientId })
                .ForeignKey("dbo.Advertisements", t => t.Advertisement_Id, cascadeDelete: true)
                .Index(t => t.Advertisement_Id);

            CreateTable(
                "dbo.Advertisements",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Url = c.String(nullable: false, maxLength: 2048),
                        PictureUrl = c.String(nullable: false, maxLength: 512),
                        CssClass = c.String(maxLength: 256),
                        MaxDisplayCount = c.Int(),
                    })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropIndex("dbo.ClientAdvertisements", new[] { "Advertisement_Id" });
            DropForeignKey("dbo.ClientAdvertisements", "Advertisement_Id", "dbo.Advertisements");
            DropTable("dbo.Advertisements");
            DropTable("dbo.ClientAdvertisements");
        }
    }
}
