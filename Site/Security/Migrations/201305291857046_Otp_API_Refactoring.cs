using System.Data.Entity.Migrations;

namespace Vtb24.Site.Security.Migrations
{   
    public partial class Otp_API_Refactoring : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tokens", "OtpType", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Tokens", "Otp", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Tokens", "IsConfirmed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tokens", "ClientId");
            DropColumn("dbo.Tokens", "IsConfired");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tokens", "IsConfired", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tokens", "ClientId", c => c.String(maxLength: 50));
            DropColumn("dbo.Tokens", "IsConfirmed");
            DropColumn("dbo.Tokens", "Otp");
            DropColumn("dbo.Tokens", "OtpType");
        }
    }
}
