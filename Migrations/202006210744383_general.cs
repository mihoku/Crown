namespace crown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class general : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.timelineItems", "isGeneral", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.timelineItems", "isGeneral");
        }
    }
}
