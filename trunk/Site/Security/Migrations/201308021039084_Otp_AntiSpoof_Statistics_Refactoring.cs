namespace Vtb24.Site.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Otp_AntiSpoof_Statistics_Refactoring : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.AttemptsStatistics");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AttemptsStatistics",
                c => new
                    {
                        PhoneNumber = c.String(nullable: false, maxLength: 100),
                        OtpType = c.String(nullable: false, maxLength: 50),
                        AttemptsToSendOtp = c.Int(nullable: false),
                        LastAttemptTime = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.PhoneNumber, t.OtpType });
            
        }
    }
}
