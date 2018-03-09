using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;

namespace forgeViewerTest
{
    public class WebSocketServer : IHttpHandler
    {
        WebSocketCollection clients = new WebSocketCollection();
        
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(new MicrosoftWebSocket());
                //context.AcceptWebSocketRequest(ProcessSocket);
            }
        }

        //public async Task ProcessSocket(AspNetWebSocketContext context)
        //{
        //    WebSocket websocket = context.WebSocket;
            
        //    const int maxMessageSize = 1024;
        //    var receiveDataBuffer = new ArraySegment<Byte>(new Byte[maxMessageSize]);
        //    var cancellationTocken = new CancellationToken();
        //    while (websocket.State == WebSocketState.Open)
        //    {
        //        WebSocketReceiveResult websocketResult = await websocket.ReceiveAsync(receiveDataBuffer, cancellationTocken);

        //        if (websocketResult.MessageType == WebSocketMessageType.Close)
        //        {
        //            await websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, cancellationTocken);
        //        }

        //        else
        //        {
        //            byte[] payloadData = receiveDataBuffer.Array.Where(b => b != 0).ToArray();
        //            string receiveString = System.Text.Encoding.UTF8.GetString(payloadData, 0, payloadData.Length);

        //            var newString =
        //              String.Format("WebSocketServer, " + receiveString + " ! Time {0}", DateTime.Now.ToString());
        //            Byte[] bytes = System.Text.Encoding.UTF8.GetBytes(newString);

        //            //Sends data back. 
        //            await websocket.SendAsync(new ArraySegment<byte>(bytes),
        //              WebSocketMessageType.Text, true, cancellationTocken);
        //        }
        //    }
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}