namespace crown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class accountsupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CompleteName", c => c.String());
            AddColumn("dbo.AspNetUsers", "isGuest", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "ResetPasswordCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ResetPasswordCode");
            DropColumn("dbo.AspNetUsers", "isGuest");
            DropColumn("dbo.AspNetUsers", "CompleteName");
        }
    }
}
