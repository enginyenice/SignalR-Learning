using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.API.Hubs
{
    /*
        All => Tüm clientlara mesaj göndermek
        Caller => Sadece istek yapan cliente mesaj göndermek
        Group => Group olarak clienlara mesaj göndermek
        ---------------------------------------------------

     */
    public class MyHub : Hub
    {
        private static List<String> Names { get; set; } = new List<string>();
        private static int ClientCount { get; set; } = 0;
        public static int TeamCount { get; set; } = 7;
        private readonly AppDbContext _context;

        public MyHub(AppDbContext context)
        {
            _context = context;
        }


        public async Task SendProduct(Product p)
        {
            await Clients.All.SendAsync("ReseiveProduct", p);

        }

        public async Task SetName(string name)
        {
            if (Names.Count >= MyHub.TeamCount)
            {
                await Clients.Caller.SendAsync(method: "Error", $"Takım en fazla {MyHub.TeamCount} kişi olabilir");
            }
            else
            {
                MyHub.Names.Add(name);
                await Clients.All.SendAsync(method: "ReceiveName", arg1: name);
            }


        }
        public async Task GetNames()
        {
            await Clients.All.SendAsync(method: "ReceiveNames", arg1: Names);
        }
        public async override Task OnConnectedAsync()
        {
            ClientCount++;
            await Clients.All.SendAsync(method: "ReceiveClientCount", ClientCount);
            await base.OnConnectedAsync();
        }
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            ClientCount--;
            await Clients.All.SendAsync(method: "ReceiveClientCount", ClientCount);
            await base.OnDisconnectedAsync(exception);
        }

        //Groups
        public async Task AddToGroup(string teamName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamName);

        }
        public async Task RemoveToGroup(string teamName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, teamName);
        }
        public async Task SendNameByGroup(string name, string teamName)
        {
            var team = _context.Teams.Where(p => p.Name == teamName).FirstOrDefault();
            if (team != null)
            {
                team.Users.Add(new User { Name = name });
            }
            else
            {
                var newTeam = new Team { Name = teamName };
                _context.Teams.Add(newTeam);
                newTeam.Users.Add(new User { Name = name });
            }

            await _context.SaveChangesAsync();
            await Clients.Group(teamName).SendAsync("ReceiveMessageByGroup",name,team.Id);

        }
        public async Task GetNamesByGroup()
        {
            var teams = _context.Teams.Include(p => p.Users).Select(p => new
            {
                teamId = p.Id,
                users = p.Users.ToList()
            });
            await Clients.All.SendAsync("ReceiveNamesByGroup",teams);
        }
    }
}
