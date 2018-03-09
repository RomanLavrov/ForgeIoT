using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace forgeViewerTest
{
    public class MicrosoftWebSocket : WebSocketHandler
    {
        private static WebSocketCollection clients = new WebSocketCollection();
        private string name;

        public override void OnOpen()
        {            
            clients.Add(this);
            clients.Broadcast(" " + clients.Count + " connected");
        }

        public override void OnMessage(byte[] message)
        {                      
            clients.Broadcast("New message: " + message);
        }

    }
}