using System.Data.Entity.Migrations;

namespace Vtb24.Site.Security.Migrations
{   
    public partial class Security_Tokens_CategoryId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SecurityTokens", "CategoryId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SecurityTokens", "CategoryId");
        }
    }
}
