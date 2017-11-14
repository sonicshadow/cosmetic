namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DiaplayAddSpecification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Displays", "Specification", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Displays", "Specification");
        }
    }
}
