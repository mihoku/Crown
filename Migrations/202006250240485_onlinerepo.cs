namespace crown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class onlinerepo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.archives", "savedInOnlineRepository", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.archives", "savedInOnlineRepository");
        }
    }
}
