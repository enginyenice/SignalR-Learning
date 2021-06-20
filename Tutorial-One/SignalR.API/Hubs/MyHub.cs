using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.API.Hubs
{
    public class MyHub : Hub
    {
        public static List<String> Names { get; set; } = new List<string>();
        public async Task SetName(string name)
        {
            MyHub.Names.Add(name);
            await Clients.All.SendAsync(method:"ReceiveName",arg1:name);
        }
        public async Task GetNames()
        {
            await Clients.All.SendAsync(method: "ReceiveNames", arg1: Names);
        }
    }
}
