namespace Vtb24.Site.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Otp_AntiSpoof_Key_Refactoring : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AttemptsStatistics", new[] { "PhoneNumber" });
            AddPrimaryKey("dbo.AttemptsStatistics", new[] { "PhoneNumber", "OtpType" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AttemptsStatistics", new[] { "PhoneNumber", "OtpType" });
            AddPrimaryKey("dbo.AttemptsStatistics", "PhoneNumber");
        }
    }
}
