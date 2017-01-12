namespace Vtb24.Site.Security.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Security_Change_Login : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhoneNumberChanges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ChangeTime = c.DateTime(nullable: false),
                        OldPhoneNumber = c.String(nullable: false, maxLength: 11),
                        NewPhoneNumber = c.String(nullable: false, maxLength: 11),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PhoneNumberChanges", new[] { "UserId" });
            DropForeignKey("dbo.PhoneNumberChanges", "UserId", "dbo.Users");
            DropTable("dbo.PhoneNumberChanges");
        }
    }
}
