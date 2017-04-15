namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class slug : DbMigration
    {
        public override void Up()
        {
            AddColumn("JamieUni.Users", "Slug", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("JamieUni.Users", "Slug");
        }
    }
}
