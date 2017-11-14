namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIncomeAddIsPay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PayeeID", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Rank", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Parent", c => c.String());
            AddColumn("dbo.AspNetUsers", "Recommend", c => c.String());
            AddColumn("dbo.UserIncomes", "RecommendID", c => c.String());
            AddColumn("dbo.UserIncomes", "IsPay", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserIncomes", "DateID", c => c.Int(nullable: false));
            AddColumn("dbo.UserIncomes", "ReceiptDateTime", c => c.DateTime());
            AddColumn("dbo.Products", "No", c => c.String());
            DropColumn("dbo.Orders", "Bank");
            DropColumn("dbo.Orders", "BankCard");
            DropColumn("dbo.UserProducts", "Type");
            DropColumn("dbo.UserProducts", "Parent");
            DropColumn("dbo.UserProducts", "Recommend");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProducts", "Recommend", c => c.String());
            AddColumn("dbo.UserProducts", "Parent", c => c.String());
            AddColumn("dbo.UserProducts", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "BankCard", c => c.String());
            AddColumn("dbo.Orders", "Bank", c => c.String());
            DropColumn("dbo.Products", "No");
            DropColumn("dbo.UserIncomes", "ReceiptDateTime");
            DropColumn("dbo.UserIncomes", "DateID");
            DropColumn("dbo.UserIncomes", "IsPay");
            DropColumn("dbo.UserIncomes", "RecommendID");
            DropColumn("dbo.AspNetUsers", "Recommend");
            DropColumn("dbo.AspNetUsers", "Parent");
            DropColumn("dbo.AspNetUsers", "Rank");
            DropColumn("dbo.Orders", "PayeeID");
        }
    }
}
