using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Database;
using SocialNetwork.Models;

namespace SocialNetwork
{
    public class SocialMediaDBQuerys
    {

        public SocialMediaEntities DbContext;
        int pageSize = 10;

        public SocialMediaDBQuerys()
        {
            DbContext = new SocialMediaEntities();
        }

        /// <summary>
        /// Check a friend Status
        /// </summary>
        /// <param name="myId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int FriendStatus(string myId, string userId)
        {
            var status = (from friendsStatus in DbContext.Friends
                          where (friendsStatus.SentID == myId && friendsStatus.ToID == userId)
                                 || (friendsStatus.SentID == userId && friendsStatus.ToID == myId)
                          select friendsStatus.Status).Take(1);

            return status.FirstOrDefault();
        }

        /// <summary>
        /// Delete a friendship status
        /// </summary>
        /// <param name="myId"></param>
        /// <param name="userId"></param>
        public void DeleteFriendship(string myId, string userId)
        {
            var friends = (from friendsStatus in DbContext.Friends
                          where (friendsStatus.SentID == myId && friendsStatus.ToID == userId)
                                 || (friendsStatus.SentID == userId && friendsStatus.ToID == myId)
                          select friendsStatus).FirstOrDefault();

            DbContext.Friends.Remove(friends);

            try
            {
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DbContext.SaveChanges();
            }
        }

        public void DeleteFriendship(string myId, int rowId)
        {
            var friends = (from friendsStatus in DbContext.Friends
                           where friendsStatus.ToID == myId && friendsStatus.ID == rowId
                           select friendsStatus).FirstOrDefault();

            DbContext.Friends.Remove(friends);

            try
            {
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DbContext.SaveChanges();
            }
        }

        public void AcceptFriend(string myId, int rowId)
        {
            var friends = (from friendsStatus in DbContext.Friends
                           where friendsStatus.ToID == myId && friendsStatus.ID == rowId
                           select friendsStatus).FirstOrDefault();

            if (friends != null) friends.Status = 2;

            try
            {
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Create a friendship status
        /// </summary>
        /// <param name="myId"></param>
        /// <param name="userId"></param>
        public void CreatePendingFriendship(string myId, string userId)
        {
            Friend friendship = new Friend()
            {
                SentID = myId,
                ToID = userId,
                Status = 1
            };


            DbContext.Friends.Add(friendship);


            try
            {
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DbContext.SaveChanges();
            }
        }

        public string WhoSentFriendrequest(string myId, string userId)
        {
            var friends = (from friendsStatus in DbContext.Friends
                           where (friendsStatus.SentID == myId && friendsStatus.ToID == userId)
                                  || (friendsStatus.SentID == userId && friendsStatus.ToID == myId)
                           select friendsStatus.SentID).FirstOrDefault();

            return friends;
        }

        public Post[] GetPostsByPage(int Value, string UserId)
        {
            int pageSize = 10;
            var Posts = (from posts in DbContext.Posts
                where posts.UserId == UserId
                orderby posts.PostedTime descending 
                select posts).Skip(Value*pageSize).Take(pageSize).ToArray();

            return Posts;

        }

        public int GetNumberOfPostPages(string UserId)
        {
            var Posts = (from posts in DbContext.Posts
                where posts.UserId == UserId
                select posts).Count();

            return Posts/pageSize;
        }

        public int IsUserPrivate(string UserId)
        {
            var privateUser = (from User in DbContext.Users
                            where User.UserId == UserId
                            select User.Private).FirstOrDefault() ?? 0;

            return privateUser;
        }

        public int GetNumberOfPostPagesHomePage(string UserId)
        {
            var select1 = from posts in DbContext.Posts
                          where posts.UserId == UserId
                          select posts;


            var select2 = from posts in DbContext.Posts
                          join fri in DbContext.Friends on posts.UserId equals fri.SentID
                          where fri.Status == 2 && fri.ToID == UserId
                          select posts;

            var select3 = from posts in DbContext.Posts
                          join fri in DbContext.Friends on posts.UserId equals fri.ToID
                          where fri.Status == 2 && fri.SentID == UserId
                          select posts;

            var unsortedPosts = select1.Union(select2).Union(select3);

            return unsortedPosts.Count() / pageSize; ;
        }

        public Post[] HomePagePosts(int Value, string UserId)
        {

            var select1 =   from posts in DbContext.Posts
                            where posts.UserId == UserId
                            select posts;


            var select2 =   from posts in DbContext.Posts
                            join fri in DbContext.Friends on posts.UserId equals fri.SentID
                            where fri.Status == 2 && fri.ToID == UserId
                            select posts;

            var select3 = from posts in DbContext.Posts
                          join fri in DbContext.Friends on posts.UserId equals fri.ToID
                          where fri.Status == 2 && fri.SentID == UserId
                          select posts;

            var unsortedPosts = select1.Union(select2).Union(select3);

            var sortedPosts = unsortedPosts.OrderByDescending(post => post.PostedTime).Skip(Value * pageSize).Take(pageSize).ToArray();

            return sortedPosts.ToArray();
            
        }

        public string GetProfilePicture(string userId) 
        {

            var select1 = from user in DbContext.Users 
                          where user.UserId == userId
                          select user.DisplayPicture;

            return select1.FirstOrDefault();

        }

        public List<NotificationListItemModel> GetNotificationListItems(string thisUser)
        {
            var select1 = from requests in DbContext.Friends 
                          where requests.ToID == thisUser
                          where requests.Status == 1
                          select requests;

            List<NotificationListItemModel> notificatons = new List<NotificationListItemModel>();

            foreach (var notif in select1)
            {
                notificatons.Add(new NotificationListItemModel()
                {
                    User = notif.UserSendID,
                    FriendRowId = notif.ID
                });
            }

            return notificatons;

        }

        public int NumberOfFriends(string thisUser)
        {
            var select1 = from friend in DbContext.Friends
                          join user in DbContext.Users on friend.ToID equals user.UserId
                          where friend.Status == 2 && friend.SentID == thisUser
                          select user;


            var select2 = from friend in DbContext.Friends
                          join user in DbContext.Users on friend.SentID equals user.UserId
                          where friend.Status == 2 && friend.ToID == thisUser
                          select user;

            var list = select1.Union(select2);

            return list.ToList().Count;
        }

        public List<FriendsChatListItemModel> GetFriendsListItems(string thisUser, int page)
        {
            var select1 = from friend in DbContext.Friends
                          join user in DbContext.Users on friend.ToID equals user.UserId
                          where friend.Status == 2 && friend.SentID == thisUser
                          select user;


            var select2 = from friend in DbContext.Friends
                join user in DbContext.Users on friend.SentID equals user.UserId
                where friend.Status == 2 && friend.ToID == thisUser
                select user;
            
            var list = select1.Union(select2).ToList();

            int count = list.Count;

            if (count > 5)
            {

                list = list.Skip(page*5).Take(5).ToList();
            }

            return list.Select(u => new FriendsChatListItemModel()
            {
                UserId = u.UserId, DisplayPicture = u.DisplayPicture, Name = u.Forename + ' ' + u.Surname
            }).ToList();
        }

        public List<FriendsChatListItemModel> SearchUsers(string inputName, string id)
        {

            var select1 = DbContext.SearchForUsersNotMe(inputName, id);

            var list = select1.ToList();

            List<FriendsChatListItemModel> users = new List<FriendsChatListItemModel>();

            foreach (var cur in list)
            {
                users.Add(new FriendsChatListItemModel()
                {
                    DisplayPicture = cur.DisplayPicture,
                    Name = cur.Forename + " " + cur.Surname,
                    UserId = cur.UserId
                });
            }



            return users;
        }

        /// <summary>
        /// Friends For Chat
        /// </summary>
        /// <param name="thisUser"></param>
        /// <returns></returns>
        public List<FriendsChatListItemModel> GetAllFriends(string thisUser)
        {
            var select1 = from friend in DbContext.Friends
                          join user in DbContext.Users on friend.ToID equals user.UserId
                          where friend.Status == 2 && friend.SentID == thisUser
                          select user;


            var select2 = from friend in DbContext.Friends
                          join user in DbContext.Users on friend.SentID equals user.UserId
                          where friend.Status == 2 && friend.ToID == thisUser
                          select user;

            var list = select1.Union(select2).ToList();

            return list.OrderByDescending(person => person.Surname).Select(u => new FriendsChatListItemModel()
            {
                UserId = u.UserId,
                DisplayPicture = u.DisplayPicture,
                Name = u.Forename + ' ' + u.Surname,
                Surname = u.Surname
                

            }).ToList();
        }

        public ChatViewMessageModel GetMessages(string UserID, string myID)
        {
            var select1 = from mess in DbContext.Messages
                          where (mess.FromID == UserID && mess.ToID == myID)
                          || (mess.FromID == myID && mess.ToID == UserID)
                          select mess;

            var messageModel = new ChatViewMessageModel()
            {
                messages = select1.ToList(),
                userID = UserID
            };

            return messageModel;
        }

        public int UnreadMessageCount(string userID, string myID)
        {

            return (from mess in DbContext.Messages
                    where (mess.FromID == userID && mess.ToID == myID && mess.BeenRead == "0")
                    select mess.BeenRead).Count();
        }

        public void ReadMessages(string userID, string myID)
        {
            using (DbContext)
            {
                DbContext.Messages
                .Where(x => x.FromID == userID)
                .Where(x => x.ToID == myID)
                .Where(x => x.BeenRead == "0")
                .ToList()
                .ForEach(x => x.BeenRead = "1");

                DbContext.SaveChanges();
            }
        }

        public FriendsChatListItemModel GetFirstName(string userID)
        {
            var select = from user in DbContext.Users
                where user.UserId == userID
                select user;

            var currUser = select.First();
            return new FriendsChatListItemModel()
            {
                Name = currUser.Forename,
                UserId = currUser.UserId,
                Surname = currUser.Surname,
                DisplayPicture = currUser.DisplayPicture
            };
        }

        public PostRep MyRepForPost(int postId, string userID)
        {
            return DbContext.PostReps.Where(x => x.PostID == postId)
                            .FirstOrDefault(x => x.UserID == userID);
        }

        public string GetSlug(string firstName, string surname)
        {
            var i = 1;
            var output = firstName + '-' + surname;

            var person = DbContext.Users.FirstOrDefault(x => x.Slug == output);
            var found = person == null;

            while (!found)
            {
                output = firstName + '-' + surname + '-' + i;
                person = DbContext.Users.FirstOrDefault(x => x.Slug == output);
                found = person == null;
                i++;
            }

            return output;
        }
    }
}