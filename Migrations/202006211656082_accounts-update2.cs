namespace crown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class accountsupdate2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "isGuest", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "isGuest", c => c.Int(nullable: false));
        }
    }
}
