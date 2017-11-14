namespace Cosmetic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStaff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Work = c.String(),
                        PhoneNumber = c.String(),
                        IDCard = c.String(),
                        Address = c.String(),
                        BasicSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PlusSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeductSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Staffs", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.Staffs", new[] { "DepartmentID" });
            DropTable("dbo.Staffs");
            DropTable("dbo.Departments");
        }
    }
}
