namespace Vtb24.Site.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Security_User_RegistrationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "RegistrationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RegistrationDate");
        }
    }
}
