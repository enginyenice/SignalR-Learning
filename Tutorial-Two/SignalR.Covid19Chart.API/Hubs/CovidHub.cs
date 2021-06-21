using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Covid19Chart.API.Hubs
{
    public class CovidHub : Hub
    {
        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceiveCovidList","Covid 19 verilerini al...");
        }
    }
}
