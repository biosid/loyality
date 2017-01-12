using Vtb24.Site.Security.OneTimePasswordService.Models;

namespace Vtb24.Site.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OtpViaEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "OtpViaSmsCount", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.OtpTokens", "DeliveryMeans", c => c.Int(nullable: false, defaultValue: (int?) OtpDeliveryMeans.Sms));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OtpTokens", "DeliveryMeans");
            DropColumn("dbo.Users", "OtpViaSmsCount");
        }
    }
}
