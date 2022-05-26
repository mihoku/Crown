namespace crown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subtheme : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.archives",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        description = c.String(),
                        archiveTypeID = c.Int(nullable: false),
                        fileName = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.archiveTypes", t => t.archiveTypeID, cascadeDelete: true)
                .Index(t => t.archiveTypeID);
            
            CreateTable(
                "dbo.archiveItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        archiveID = c.Int(nullable: false),
                        itemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.archives", t => t.archiveID, cascadeDelete: true)
                .ForeignKey("dbo.timelineItems", t => t.itemID, cascadeDelete: true)
                .Index(t => t.archiveID)
                .Index(t => t.itemID);
            
            CreateTable(
                "dbo.timelineItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Contents = c.String(),
                        EventDate = c.DateTime(nullable: false),
                        subThemeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.subThemes", t => t.subThemeID, cascadeDelete: true)
                .Index(t => t.subThemeID);
            
            CreateTable(
                "dbo.subThemes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        code = c.String(),
                        description = c.String(),
                        icons = c.String(),
                        themeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.themes", t => t.themeID, cascadeDelete: true)
                .Index(t => t.themeID);
            
            CreateTable(
                "dbo.themes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.archiveTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.archives", "archiveTypeID", "dbo.archiveTypes");
            DropForeignKey("dbo.timelineItems", "subThemeID", "dbo.subThemes");
            DropForeignKey("dbo.subThemes", "themeID", "dbo.themes");
            DropForeignKey("dbo.archiveItems", "itemID", "dbo.timelineItems");
            DropForeignKey("dbo.archiveItems", "archiveID", "dbo.archives");
            DropIndex("dbo.subThemes", new[] { "themeID" });
            DropIndex("dbo.timelineItems", new[] { "subThemeID" });
            DropIndex("dbo.archiveItems", new[] { "itemID" });
            DropIndex("dbo.archiveItems", new[] { "archiveID" });
            DropIndex("dbo.archives", new[] { "archiveTypeID" });
            DropTable("dbo.archiveTypes");
            DropTable("dbo.themes");
            DropTable("dbo.subThemes");
            DropTable("dbo.timelineItems");
            DropTable("dbo.archiveItems");
            DropTable("dbo.archives");
        }
    }
}
