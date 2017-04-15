using System;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Web.WebSockets;

namespace SocialNetwork
{
    /// <summary>
    /// Summary description for test
    /// </summary>
    public class LiveChat : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Diagnostics.Debug.WriteLine(context.User.Identity.GetUserId());
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(new Socket(context.User.Identity.GetUserId()));
            }

        }


        //I had this set to true, could be causing the issue.
        public bool IsReusable => false;
    }
}
