namespace Vtb24.Site.Content.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdvertisementsShowUntil : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advertisements", "ShowUntil", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advertisements", "ShowUntil");
        }
    }
}
