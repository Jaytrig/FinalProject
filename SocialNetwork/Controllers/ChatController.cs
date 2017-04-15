using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {

        private readonly SocialMediaDBQuerys _dbQuerys = new SocialMediaDBQuerys();
        // GET: Chat
        public ActionResult Index()
        {
            //get friends
            var userID = User.Identity.GetUserId();

            var friends = _dbQuerys.GetAllFriends(userID);
            
            return View("Index", friends);
        }




        [HttpGet]
        public ActionResult GetChat(string name)
        {
            var messages = _dbQuerys.GetMessages(name, User.Identity.GetUserId());
            _dbQuerys.ReadMessages(name, User.Identity.GetUserId());
            return PartialView("_getChat", messages);

        }
    }
}