using System.Data.Entity.Migrations;

namespace Vtb24.Site.Security.Migrations
{
    
    public partial class Otp_AntiSpoof_And_Refactorings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OtpTokens", "AttemptsToRenew", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OtpTokens", "AttemptsToRenew");
        }
    }
}
