namespace crown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timeline : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.timelineItems", "subThemeID", "dbo.subThemes");
            DropIndex("dbo.timelineItems", new[] { "subThemeID" });
            CreateTable(
                "dbo.subThemeItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        subThemeID = c.Int(nullable: false),
                        timelineItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.subThemes", t => t.subThemeID, cascadeDelete: true)
                .ForeignKey("dbo.timelineItems", t => t.timelineItemID, cascadeDelete: true)
                .Index(t => t.subThemeID)
                .Index(t => t.timelineItemID);
            
            DropColumn("dbo.timelineItems", "subThemeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.timelineItems", "subThemeID", c => c.Int(nullable: false));
            DropForeignKey("dbo.subThemeItems", "timelineItemID", "dbo.timelineItems");
            DropForeignKey("dbo.subThemeItems", "subThemeID", "dbo.subThemes");
            DropIndex("dbo.subThemeItems", new[] { "timelineItemID" });
            DropIndex("dbo.subThemeItems", new[] { "subThemeID" });
            DropTable("dbo.subThemeItems");
            CreateIndex("dbo.timelineItems", "subThemeID");
            AddForeignKey("dbo.timelineItems", "subThemeID", "dbo.subThemes", "ID", cascadeDelete: true);
        }
    }
}
