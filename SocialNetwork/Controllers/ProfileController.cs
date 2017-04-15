using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SocialNetwork.Database;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly SocialMediaDBQuerys _dbQuerys = new SocialMediaDBQuerys();
        
        //public const int RecordsPerPage = 20;
        public string UserId = string.Empty;
       

        // GET: Profile
        public ActionResult Index(string slug)
        {
            Session["profileEntered"] = true;


            var userFromDb = _dbQuerys.DbContext.Users.FirstOrDefault(x => x.Slug == slug);

            string userId = null;

            if (userFromDb != null)
            {
                userId = userFromDb.UserId;
            }

            if (userId != null)
            {
                var friendStatus = _dbQuerys.FriendStatus(userId, User.Identity.GetUserId());

                if (friendStatus != 2 && userId != User.Identity.GetUserId())
                {
                    if (IsUserPrivate(userId))
                    {
                        var user = GetUserInfo(userId);
                        Session["profileEntered"] = false;
                        return View("PrivateProfile", user);
                    }
                }

                Session["IsEndOfRecords"] = false;
                Session["EndBeenReturned"] = false;
                Session["UserId"] = userId;

                Session["NumberOfPages"] = GetNumberOfPostPages(userId);
                ViewBag.Posts = GetPostsByPage(0, userId);

                var userInfo = GetUserInfo(userId);

                return View("Index", userInfo);
            }
            Session["profileEntered"] = false;
            return View("UserNotFound");
        }

        public ActionResult GetPosts(int? pageNum)
        {

            bool profileEntered = Session["profileEntered"] as bool? ?? false;

            if (!profileEntered && !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", new { userId = Session["UserId"] as string });
            }

            UserId = Session["UserId"] as string;

            Session["profileEntered"] = false;

            pageNum = pageNum ?? 0;
            // if there is no more posts dont do any more work.
            if (pageNum > (Session["NumberOfPages"] as int? ?? 0))
            {
                Session["IsEndOfRecords"] = true;
            }

            if (Session["IsEndOfRecords"] as bool? ?? false)
            {
                if (Session["EndBeenReturned"] as bool? ?? false)
                {
                    return null;
                }
                Session["EndBeenReturned"] = true;
                return PartialView("_EndOfPosts");
            }

            if (Request.IsAjaxRequest())
            {
                var posts = _dbQuerys.GetPostsByPage(pageNum.Value, UserId);
                return PartialView("_Posts", posts);
            }

            return null;
        }

        public ActionResult SearchForUsers()
        {
            return View("SearchForUsers");
        }

        [HttpGet]
        public ActionResult SearchUsers(string name)
        {
            return PartialView("SearchResults", _dbQuerys.SearchUsers(name, User.Identity.GetUserId()));
        }

        public void AddFriend(string theirId)
        {
            _dbQuerys.CreatePendingFriendship(User.Identity.GetUserId(), theirId);
        }

        [HttpPost]
        public void RemoveFriend(string theirId)
        {
            _dbQuerys.DeleteFriendship(User.Identity.GetUserId(), theirId);
        }

        [HttpPost]
        public void RemoveFriendbyRow(int rowId)
        {
            _dbQuerys.DeleteFriendship(User.Identity.GetUserId(), rowId);
        }

        [HttpPost]
        public void AcceptFriend(int rowId)
        {
            _dbQuerys.AcceptFriend(User.Identity.GetUserId(), rowId);
        }

        private int GetNumberOfPostPages(string userId)
        {
            return _dbQuerys.GetNumberOfPostPages(userId);
        }

        public Post[] GetPostsByPage(int pageNum, string userId)
        {
            return _dbQuerys.GetPostsByPage(pageNum, userId);
        }

        public bool IsUserPrivate(string userId)
        {
            bool priUser = _dbQuerys.IsUserPrivate(userId) == 1;

            return priUser;
        }

        public User GetUserInfo(string userId)
        {
            return (from users in _dbQuerys.DbContext.Users
                        where users.UserId == userId
                        select users).FirstOrDefault();
        }

        [HttpPost]
        public void SubmitPost(PostViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return;
            }
            
            var post = new Post()
            {
                Message = model.Message,
                Title = model.Title ?? "",
                UserId = User.Identity.GetUserId(),
                PostedTime = model.PostedTime
            };

            _dbQuerys.DbContext.Posts.Add(post);
            
            try
            {
                _dbQuerys.DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _dbQuerys.DbContext.SaveChanges();
            }
        }


        [HttpPost]
        public void DeletePost(int postId)
        {
            if (!Request.IsAjaxRequest()) return;

            var userid = User.Identity.GetUserId();

            var postExists = _dbQuerys.DbContext.Posts.FirstOrDefault(x => x.ID == postId && x.UserId == userid);

            if (postExists == null) return;

            try
            {
                _dbQuerys.DbContext.Posts.Remove(postExists);
                _dbQuerys.DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _dbQuerys.DbContext.SaveChanges();
            }

        }


        public ActionResult FriendsPage(int page)
        {
            return PartialView("_FriendsList", new FriendsPartialModel() {Page = page, UserId = User.Identity.GetUserId()});
        }
    }
}