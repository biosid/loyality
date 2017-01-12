namespace Vtb24.Site.Content.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class PageScript : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PageSnapshots", "Data_Script", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PageSnapshots", "Data_Script");
        }
    }
}
