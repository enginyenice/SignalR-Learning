using Microsoft.AspNetCore.SignalR;
using SignalR.Covid19Chart.API.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Covid19Chart.API.Models
{
    public class CovidService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<CovidHub> _hubContext;
        public CovidService(AppDbContext context, IHubContext<CovidHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IQueryable<Covid> GetList()
        {
            return _context.Covids.AsQueryable();

        }

        public async Task SaveCovid(Covid covid)
        {
            await _context.Covids.AddAsync(covid);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList","Gidecek data....");
        }



    }
}
