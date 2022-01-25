using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BricksMeatballs.Controllers
{
    /// <summary>
    /// Author: Lim Pei Yan, Huang Chaoshan
    /// Buy Controller
    /// Contains routings and an AjaxFilter to filter data
    /// </summary>
    public class BuyController : Controller
    {

        public IConfiguration Configuration { get; }

        public BuyController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Proceed to Buyer Page
        /// </summary>
        public IActionResult BuyerSearch()
        {
            PullData db = new PullData(Configuration);
            ViewBag.Location = db.BindDropDownListCustomer("select distinct 1 Id, street Name from pmiresidenceresult limit 0,500;", "Location");
            ViewBag.propertyType = db.BindDropDownListCustomer("select distinct 1 Id,propertyType Name from pmiresidenceresult limit 0,500;", "Property Type");

            return View();
        }

        /// <summary>
        /// Proceed to BuyerResult Page
        /// </summary>
        public IActionResult BuyerResult()
        {
            return View();
        }

        /// <summary>
        /// Retrieve data based on filters
        /// </summary>
        /// <returns>
        /// Return a string containing result from query
        /// </returns>
        [HttpGet]
        public string AjaxFilter(string Locations, string propertyTypes, string PriceRange, string SaleType, string LandArea, string FloorRange)
        {
            PullData db = new PullData(Configuration);
            string whrQuery = " Where 0=0";
            if (Locations != null)
            {
                whrQuery = whrQuery + " and street='" + Locations + "'";
            }
            if (propertyTypes != null)
            {
                whrQuery = whrQuery + " and propertyType='" + propertyTypes + "'";
            }
            if (SaleType != null)
            {
                whrQuery = whrQuery + " and typeOfSale='" + SaleType + "'";
            }
            if (FloorRange != null)
            {
                whrQuery = whrQuery + " and floorRange='" + FloorRange + "'";
            }
            if (PriceRange != null)
            {
                if (PriceRange == "1")
                {
                    whrQuery = whrQuery + " and CAST(price AS UNSIGNED)<" + 1000000;
                }
                else if (PriceRange == "11")
                {
                    whrQuery = whrQuery + " and CAST(price AS UNSIGNED)>" + 10000000;
                }
                else
                {
                    whrQuery = whrQuery + " and (CAST(price AS UNSIGNED)<" + Convert.ToInt32(PriceRange) * 1000000 + " and CAST(price AS UNSIGNED)>" + ((Convert.ToInt32(PriceRange) * 1000000) - 1000000) + " )";
                }

            }

            if (LandArea != null)
            {
                if (LandArea == "1")
                {
                    whrQuery = whrQuery + " and CAST(area AS UNSIGNED)<" + 100;
                }
                else
                {
                    whrQuery = whrQuery + " and (CAST(area AS UNSIGNED)<" + Convert.ToInt32(LandArea) * 100 + " and CAST(area AS UNSIGNED)>" + ((Convert.ToInt32(LandArea) * 100) - 100) + " )";
                }

            }
            whrQuery = whrQuery + " ORDER BY idtransaction DESC LIMIT 600";
            string query = " Select  * From pmiresidenceresult " + whrQuery;
            DataTable dt = db.LoadData(query, "Loaddt");
            string json = JsonConvert.SerializeObject(dt);
            return JsonConvert.SerializeObject(dt);

        }
    }
}
