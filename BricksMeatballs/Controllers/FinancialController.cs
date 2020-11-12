using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BricksMeatballs.Models;
using System.Web;

namespace BricksMeatballs.Controllers
{
    public class FinancialController : Controller
    {
        // GET: FinancialController/Index
        public ActionResult Index(FinancialModel fmodel)
        {
            return View(fmodel);
        }

        // GET: FinancialController/Create
        [HttpGet]
        public IActionResult Create()
        {
            FinancialModel fmodel = new FinancialModel();
            return View(fmodel);
        }

        // POST: FinancialController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FinancialModel fmodel)
        {
            try
            {
                return View("Index", fmodel);
            }
            catch
            {
                return View();
            }
        }

        // GET: FinancialController/Edit/
        public ActionResult Edit(FinancialModel fmodel)
        {
            return View(fmodel);
        }
    }
}