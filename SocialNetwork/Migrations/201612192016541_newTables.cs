namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("JamieUni.Users", "DisplayPicture", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("JamieUni.Users", "DisplayPicture");
        }
    }
}
