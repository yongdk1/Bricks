using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BricksMeatballs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BricksMeatballs.Controllers
{
    public class GraphController : Controller
    {
        public IConfiguration Configuration { get; }

        public GraphController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // GET: /<controller>/  
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GraphChart(string Location)
        {
            ChartDataAcess CVM = new ChartDataAcess(Configuration);

            var graphData = CVM.GetAvgPriceList(Location);
            return JsonConvert.SerializeObject(graphData);
        }
    }
}
