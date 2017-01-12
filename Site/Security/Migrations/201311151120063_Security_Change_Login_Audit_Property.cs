namespace Vtb24.Site.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Security_Change_Login_Audit_Property : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PhoneNumberChanges", "ChangedBy", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PhoneNumberChanges", "ChangedBy");
        }
    }
}
