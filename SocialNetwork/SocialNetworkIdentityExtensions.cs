using System.Linq;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using SocialNetwork.Database;

namespace SocialNetwork
{
    //
    // Summary:
    //     Extensions making it easier to get the user name/user id claims off of an identity
    public static class IdentityExtensions
    {
        public static string GetSlug(this IIdentity identity)
        {
            var db = new SocialMediaEntities();
            var thisUser = identity.GetUserId();
            var firstOrDefault = db.Users.FirstOrDefault(x => x.UserId == thisUser);

            return firstOrDefault != null ? firstOrDefault.Slug : "";
        }

        public static User GetUser(this IIdentity identity)
        {
            var db = new SocialMediaEntities();
            var thisUser = identity.GetUserId();
            var firstOrDefault = db.Users.FirstOrDefault(x => x.UserId == thisUser);

            return firstOrDefault;
        }
    }
}