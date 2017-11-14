namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplierProductAddCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "BankAccount", c => c.String());
            AddColumn("dbo.Accounts", "BankName", c => c.String());
            AddColumn("dbo.Accounts", "BankCard", c => c.String());
            AddColumn("dbo.Orders", "BankAccount", c => c.String());
            AddColumn("dbo.Orders", "BankName", c => c.String());
            AddColumn("dbo.Orders", "BankCard", c => c.String());
            AddColumn("dbo.UserIncomes", "BankAccount", c => c.String());
            AddColumn("dbo.UserIncomes", "BankName", c => c.String());
            AddColumn("dbo.UserIncomes", "BankCard", c => c.String());
            AddColumn("dbo.SupplierProducts", "Code", c => c.String());
            AddColumn("dbo.SupplierProducts", "Send", c => c.Int(nullable: false));
            AddColumn("dbo.SupplierProducts", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SupplierProducts", "SendTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SupplierProducts", "State", c => c.Int(nullable: false));
            AddColumn("dbo.Stocks", "DataID", c => c.Int(nullable: false));
            DropColumn("dbo.Accounts", "Pay");
            DropColumn("dbo.Orders", "PayeeID");
            DropColumn("dbo.UserIncomes", "Payment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserIncomes", "Payment", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "PayeeID", c => c.Int(nullable: false));
            AddColumn("dbo.Accounts", "Pay", c => c.Int(nullable: false));
            DropColumn("dbo.Stocks", "DataID");
            DropColumn("dbo.SupplierProducts", "State");
            DropColumn("dbo.SupplierProducts", "SendTotal");
            DropColumn("dbo.SupplierProducts", "Total");
            DropColumn("dbo.SupplierProducts", "Send");
            DropColumn("dbo.SupplierProducts", "Code");
            DropColumn("dbo.UserIncomes", "BankCard");
            DropColumn("dbo.UserIncomes", "BankName");
            DropColumn("dbo.UserIncomes", "BankAccount");
            DropColumn("dbo.Orders", "BankCard");
            DropColumn("dbo.Orders", "BankName");
            DropColumn("dbo.Orders", "BankAccount");
            DropColumn("dbo.Accounts", "BankCard");
            DropColumn("dbo.Accounts", "BankName");
            DropColumn("dbo.Accounts", "BankAccount");
        }
    }
}
