namespace Vtb24.Site.Security.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Security_Otp_Antispoof_MobilePhone : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.AttemptsStatistics");
            CreateTable(
                "dbo.AttemptsStatistics",
                c => new
                {
                    PhoneNumber = c.String(nullable: false, maxLength: 100),
                    OtpType = c.String(nullable: false, maxLength: 50),
                    AttemptsToSendOtp = c.Int(nullable: false),
                    LastAttemptTime = c.DateTime(),
                })
                .PrimaryKey(t => t.PhoneNumber);
        }
        
        public override void Down()
        {
            DropTable("dbo.AttemptsStatistics");
            
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
        }
    }
}
