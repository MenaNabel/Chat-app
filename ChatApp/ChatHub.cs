using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatApp
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        private static readonly string methodName = "receiveMessage";
        public void SendMessage(string message)
        {
            Clients.All.receiveMessage(message);
            ((IClientProxy)Clients.All).Invoke(methodName, message);

            Clients.Caller.receiveMessage("Sent");
            Clients.Client(Context.ConnectionId).receiveMessage("Sent");
            
            Clients.Others.receiveMessage("New Message");
            Clients.AllExcept(Context.ConnectionId).receiveMessage("New Message");
        }


        public async Task JoinGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
            Clients.Group(groupName).receiveMessage($"{Context.ConnectionId} join");
        }
        public void SendGroupMessage(string groupName, string message)
        {
            Clients.Group(groupName).receiveMessage(message);
        }




        public override Task OnConnected()
        {
            Clients.All.receiveMessage($"{Context.ConnectionId}: Connected");
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            Clients.All.receiveMessage($"{Context.ConnectionId}: ReConnected");

            return base.OnReconnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            Clients.All.receiveMessage($"{Context.ConnectionId}: DisConnected");

            return base.OnDisconnected(stopCalled);
        }

    }
}