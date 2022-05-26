namespace crown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class origin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.archives", "origin", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.archives", "origin");
        }
    }
}
