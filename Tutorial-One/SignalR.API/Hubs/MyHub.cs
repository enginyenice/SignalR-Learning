using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.API.Hubs
{
    public class MyHub : Hub
    {
        private static List<String> Names { get; set; } = new List<string>();
        private static int ClientCount { get; set; } = 0;
        public async Task SetName(string name)
        {
            MyHub.Names.Add(name);
            await Clients.All.SendAsync(method:"ReceiveName",arg1:name);
        }
        public async Task GetNames()
        {
            await Clients.All.SendAsync(method: "ReceiveNames", arg1: Names);
        }
        public async override Task OnConnectedAsync()
        {
            ClientCount++;
            await Clients.All.SendAsync(method:"ReceiveClientCount",ClientCount);
            await base.OnConnectedAsync();
        }
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            ClientCount--;
            await Clients.All.SendAsync(method: "ReceiveClientCount", ClientCount);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
