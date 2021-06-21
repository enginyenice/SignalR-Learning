using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.Covid19Chart.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Covid19Chart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController : ControllerBase
    {
        private readonly CovidService _covidService;

        public CovidsController(CovidService covidService)
        {
            _covidService = covidService;
        }
        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await _covidService.SaveCovid(covid);
            IQueryable<Covid> covidList = _covidService.GetList();
            return Ok(covidList);
        }
        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random rnd = new Random();
            Enumerable.Range(1, 50).ToList().ForEach(p =>
            {
                foreach (ECity city in Enum.GetValues(typeof(ECity)))
                {
                    var newCovid = new Covid() { City = city, Count = rnd.Next(100, 1000), CovidDate = DateTime.Now.AddDays(p) };

                    _covidService.SaveCovid(newCovid).Wait();
                    System.Threading.Thread.Sleep(1000);
                }

            });

            return Ok("Covid19 dataları veritabanına kayıt edildi...");

        }


    }
}
