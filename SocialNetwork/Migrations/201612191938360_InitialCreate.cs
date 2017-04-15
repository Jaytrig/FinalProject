namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "JamieUni.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "JamieUni.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("JamieUni.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("JamieUni.Users", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "JamieUni.Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "JamieUni.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("JamieUni.Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "JamieUni.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("JamieUni.Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("JamieUni.UserRoles", "IdentityUser_Id", "JamieUni.Users");
            DropForeignKey("JamieUni.UserLogins", "IdentityUser_Id", "JamieUni.Users");
            DropForeignKey("JamieUni.UserClaims", "IdentityUser_Id", "JamieUni.Users");
            DropForeignKey("JamieUni.UserRoles", "RoleId", "JamieUni.Roles");
            DropIndex("JamieUni.UserLogins", new[] { "IdentityUser_Id" });
            DropIndex("JamieUni.UserClaims", new[] { "IdentityUser_Id" });
            DropIndex("JamieUni.Users", "UserNameIndex");
            DropIndex("JamieUni.UserRoles", new[] { "IdentityUser_Id" });
            DropIndex("JamieUni.UserRoles", new[] { "RoleId" });
            DropIndex("JamieUni.Roles", "RoleNameIndex");
            DropTable("JamieUni.UserLogins");
            DropTable("JamieUni.UserClaims");
            DropTable("JamieUni.Users");
            DropTable("JamieUni.UserRoles");
            DropTable("JamieUni.Roles");
        }
    }
}
