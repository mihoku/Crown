namespace crown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timelineDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.timelineDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Contents = c.String(),
                        itemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.timelineItems", t => t.itemID, cascadeDelete: true)
                .Index(t => t.itemID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.timelineDetails", "itemID", "dbo.timelineItems");
            DropIndex("dbo.timelineDetails", new[] { "itemID" });
            DropTable("dbo.timelineDetails");
        }
    }
}
