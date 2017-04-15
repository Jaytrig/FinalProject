using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Microsoft.Web.WebSockets;
using SocialNetwork.Database;

namespace SocialNetwork
{
    public class Socket : WebSocketHandler
    {
        public static WebSocketCollection Clients = new WebSocketCollection();
        public string Name { get; set; }

        public Socket(string getUserId)
        {
            Name = getUserId;
        }

        public override void OnOpen()
        {
            Clients.Add(this);
        }

        public override void OnClose()
        {
        }

        public override void OnError()
        {
            Send("Error Sockets");
        }

        public override void OnMessage(String message)
        {

            if (message.StartsWith("|online|"))
            {
                string id = message.Substring(8);
                var found = Clients.SingleOrDefault(r => ((Socket) r).Name == id);

                Send(found != null ? "|online|true|"+ id : "|online|false|"+id);
            }
            else if(message.StartsWith("|message|"))
            {
                string afterType = message.Substring(9);
                string ID = afterType.Substring(0, afterType.IndexOf('|'));

                //+ 1 to get past the :'s index
                int startOfAfterId = afterType.IndexOf('|') + 1;

                string afterId = afterType.Substring(startOfAfterId);

                DateTime? time = DateTime.Parse(afterId.Substring(0, afterId.IndexOf('|')));

                string fullMessage = afterId.Substring(afterId.IndexOf('|') + 1);

                var webSocketHandler = Clients.SingleOrDefault(r => ((Socket)r).Name == ID);

                string toSend = "|message|" + Name + "|";

                
                using (var cxt = new SocialMediaEntities())
                {
                    var messageForDb = new Message()
                    {
                        BeenRead = "0",
                        FromID = Name,
                        ToID = ID,
                        MessageType = 1,
                        Message1 = fullMessage,
                        Time = time
                    };


                    cxt.Messages.Add(messageForDb);
                    cxt.SaveChanges();
                }


                webSocketHandler?.Send(toSend);
            }
        }
    }
}



