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
        // GET: FinancialController
        public ActionResult Index()
        {
            return View(ViewBag.fmodel);
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
                ViewBag.fmodel = fmodel;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FinancialController/Edit/
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FinancialController/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
