namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplierAddContacts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Suppliers", "Contacts", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Suppliers", "Contacts");
        }
    }
}
