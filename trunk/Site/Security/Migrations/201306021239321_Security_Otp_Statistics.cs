using System.Data.Entity.Migrations;

namespace Vtb24.Site.Security.Migrations
{    
    public partial class Security_Otp_Statistics : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttemptsStatistics",
                c => new
                    {
                        ExternalId = c.String(nullable: false, maxLength: 255),
                        OtpType = c.String(nullable: false, maxLength: 50),
                        AttemptsToSendOtp = c.Int(nullable: false),
                        LastAttemptTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.ExternalId);
            
            AddColumn("dbo.Users", "IsPasswordSet", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tokens", "AttemptsToConfirm", c => c.Int(nullable: false));
            AddColumn("dbo.Tokens", "IsFake", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "IsPaswordSet");
            DropColumn("dbo.Users", "LastPasswordChangeOtpTime");
            DropColumn("dbo.Users", "LastPasswordChangeOtpAttempts");
            DropColumn("dbo.Tokens", "Attempts");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tokens", "Attempts", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "LastPasswordChangeOtpAttempts", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "LastPasswordChangeOtpTime", c => c.DateTime());
            AddColumn("dbo.Users", "IsPaswordSet", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tokens", "IsFake");
            DropColumn("dbo.Tokens", "AttemptsToConfirm");
            DropColumn("dbo.Users", "IsPasswordSet");
            DropTable("dbo.AttemptsStatistics");
        }
    }
}
