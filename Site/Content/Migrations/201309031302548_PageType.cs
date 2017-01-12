namespace Vtb24.Site.Content.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PageType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Pages", "ExternalId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pages", "ExternalId");
            DropColumn("dbo.Pages", "Type");
        }
    }
}
