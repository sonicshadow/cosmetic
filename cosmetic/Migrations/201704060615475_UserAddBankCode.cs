namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddBankCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "BankCode", c => c.String());
            AddColumn("dbo.UserIncomes", "Payment", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserIncomes", "Payment");
            DropColumn("dbo.AspNetUsers", "BankCode");
        }
    }
}
