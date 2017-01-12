namespace Vtb24.Site.Content.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MyPointImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MyPointImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        ImagePath = c.String(nullable: false)
                    })
                .PrimaryKey(t => t.Id);
            Sql("ALTER TABLE dbo.MyPointImages ADD [HashCode]  AS (CONVERT([varchar](32),hashbytes('MD5',[Description]),(2)))");
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MyPointImages");
        }
    }
}
