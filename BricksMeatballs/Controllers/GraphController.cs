using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Helpers;
//using System.Web.Mvc;

//// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace BricksMeatballs.Controllers
//{
//    public class GraphController : Controller
//    {
//        // GET: /<controller>/
//        public ActionResult Index(int? page)
//        {
//            var model = newChartModel{
//                Chart = Display()
//            };
//            return View(model);
//        }

//        private Chart Display()
//        {
//            return new Chart(600, 400)
//                    .AddTitle("Average Price")
//                    .AddLegend()
//                    .AddSeries(
//                        name: "Price",
//                        chartType: "Line",
//                        xValue: new[] { "Digg", "DZone", "DotNetKicks", "StumbleUpon" },
//                        yValues: new[] { "150000", "180000", "120000", "250000" });
//        }
//    }
//}

using BricksMeatballs.Models;
using Microsoft.AspNetCore.Mvc;

namespace BricksMeatballs.Controllers
{
    public class GraphController : Controller
    {
        // GET: /<controller>/  
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GraphChart()
        {
            //String to be placed in GetAvgPriceList
            var graphData = ChartDataAcess.GetAvgPriceList("04");
            return Json(graphData);
        }
    }
}
