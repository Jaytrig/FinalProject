using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SocialNetwork.Database;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public readonly SocialMediaDBQuerys DbQuerys = new SocialMediaDBQuerys();


        [OutputCache(Duration = 5)]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            Session["IsEndOfRecords"] = false;
            Session["EndBeenReturned"] = false;

            Session["NumberOfPages"] = GetNumberOfPostPages(userId);
            ViewBag.Posts = GetPostsByPage(0, userId);

            return View("Index");

        }

        public ActionResult GetPosts(int? pageNum)
        {

            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index");
            }

            var userId = User.Identity.GetUserId();

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
                return PartialView("../Profile/_EndOfPosts");
            }

            if (Request.IsAjaxRequest())
            {
                var posts = GetPostsByPage(pageNum.Value, userId);
                return PartialView("../Profile/_Posts", posts);
            }

            return null;
        }

        private int GetNumberOfPostPages(string userId)
        {
            return DbQuerys.GetNumberOfPostPagesHomePage(userId);
        }

        public Post[] GetPostsByPage(int pageNum, string userId)
        {
            return DbQuerys.HomePagePosts(pageNum, userId);
        }

        [HttpGet]
        public ActionResult LikePost(int postId)
        {
            using (var cxt = new SocialMediaEntities())
            {
                string userID = User.Identity.GetUserId();
                var postRep = cxt.PostReps
                                .Where(r => r.PostID == postId)
                                .FirstOrDefault(r => r.UserID == userID);

                if (postRep != null)
                {
                    postRep.Rep = 1;
                }
                else
                {
                    cxt.PostReps.Add(new PostRep()
                    {
                        Rep = 1,
                        UserID = userID,
                        PostID = postId
                    });
                }

                cxt.SaveChanges();
            }

            return PartialView("../Profile/_postRep", DbQuerys.DbContext.Posts.FirstOrDefault(x => x.ID == postId));
        }

        [HttpGet]
        public ActionResult DislikePost(int postId)
        {
            using (var cxt = new SocialMediaEntities())
            {
                string userID = User.Identity.GetUserId();
                var postRep = cxt.PostReps
                                .Where(r => r.PostID == postId)
                                .FirstOrDefault(r => r.UserID == userID);

                if (postRep != null)
                {
                    postRep.Rep = -1;
                }
                else
                {
                    cxt.PostReps.Add(new PostRep()
                    {
                        Rep = -1,
                        UserID = userID,
                        PostID = postId
                    });
                }

                cxt.SaveChanges();
            }


            return PartialView("../Profile/_postRep", DbQuerys.DbContext.Posts.FirstOrDefault(x => x.ID == postId));

        }


    }
}