namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("JamieUni.Users", "BrithDate", c => c.DateTime());
            AddColumn("JamieUni.Users", "Forename", c => c.String());
            AddColumn("JamieUni.Users", "Surname", c => c.String());
            AddColumn("JamieUni.Users", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("JamieUni.Users", "Location");
            DropColumn("JamieUni.Users", "Surname");
            DropColumn("JamieUni.Users", "Forename");
            DropColumn("JamieUni.Users", "BrithDate");
        }
    }
}
