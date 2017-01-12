namespace Vtb24.Site.Content.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsMessages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 256),
                        Description = c.String(nullable: false, maxLength: 512),
                        Url = c.String(nullable: false, maxLength: 2048),
                        Priority = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        IsPublished = c.Boolean(nullable: false),
                        Picture = c.String(nullable: false, maxLength: 512),
                        Author = c.String(nullable: false, maxLength: 256),
                        Segment = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSnapshots",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 32),
                        EntityId = c.Long(nullable: false),
                        SerializedContent = c.String(nullable: false),
                        Type = c.String(nullable: false, maxLength: 256),
                        Author = c.String(nullable: false, maxLength: 100),
                        CreationDate = c.DateTime(nullable: false),
                        ContentHash = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        IsBuiltin = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        History_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PageHistories", t => t.History_Id, cascadeDelete: true)
                .Index(t => t.History_Id);
            
            CreateTable(
                "dbo.PageHistories",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CurrentVersion_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PageSnapshots", t => t.CurrentVersion_Id, cascadeDelete: true)
                .Index(t => t.CurrentVersion_Id);
            
            CreateTable(
                "dbo.PageSnapshots",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Data_Url = c.String(nullable: false, maxLength: 256),
                        Data_Title = c.String(nullable: false, maxLength: 256),
                        Data_Keywords = c.String(maxLength: 1024),
                        Data_Description = c.String(maxLength: 1024),
                        Data_Layout = c.Int(nullable: false),
                        Data_Content = c.String(),
                        Author = c.String(maxLength: 256),
                        When = c.DateTime(nullable: false),
                        PageHistory_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PageHistories", t => t.PageHistory_Id)
                .Index(t => t.PageHistory_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PageSnapshots", new[] { "PageHistory_Id" });
            DropIndex("dbo.PageHistories", new[] { "CurrentVersion_Id" });
            DropIndex("dbo.Pages", new[] { "History_Id" });
            DropForeignKey("dbo.PageSnapshots", "PageHistory_Id", "dbo.PageHistories");
            DropForeignKey("dbo.PageHistories", "CurrentVersion_Id", "dbo.PageSnapshots");
            DropForeignKey("dbo.Pages", "History_Id", "dbo.PageHistories");
            DropTable("dbo.PageSnapshots");
            DropTable("dbo.PageHistories");
            DropTable("dbo.Pages");
            DropTable("dbo.DbSnapshots");
            DropTable("dbo.NewsMessages");
        }
    }
}
