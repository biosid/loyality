using System.Data.Entity.Migrations;

namespace Vtb24.Site.Security.Migrations
{
   
    public partial class Security_Token_MessageTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tokens", "MessageTemplate", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tokens", "MessageTemplate");
        }
    }
}
