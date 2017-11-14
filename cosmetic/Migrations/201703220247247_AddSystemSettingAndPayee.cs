namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSystemSettingAndPayee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payees",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Bank = c.String(),
                        BankCard = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SystemSettings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Key = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemSettings");
            DropTable("dbo.Payees");
        }
    }
}
