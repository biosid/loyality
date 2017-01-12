using System.Data.Entity.Migrations;

namespace Vtb24.Site.Security.Migrations
{
    public partial class Security_Password_Otp_Antispoof : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LastPasswordChangeOtpTime", c => c.DateTime());
            AddColumn("dbo.Users", "LastPasswordChangeOtpAttempts", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LastPasswordChangeOtpAttempts");
            DropColumn("dbo.Users", "LastPasswordChangeOtpTime");
        }
    }
}
