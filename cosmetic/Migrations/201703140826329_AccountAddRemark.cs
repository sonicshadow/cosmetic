namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountAddRemark : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Remark", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Remark");
        }
    }
}
