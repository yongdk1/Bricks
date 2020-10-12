using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BricksMeatballs.Models;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace BricksMeatballs.Controllers
{
    public class FinancialController : Controller
    {
        public IActionResult FinancialCal()
        {
            return View();
        }
        // GET: Financial
        public ActionResult Index()
        {
            return View();
        }

        // GET: Financial/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Financial/Create
        public ActionResult Calculator()
        {
            return View();
        }

        // POST: Financial/Create
        [HttpPost]
        public ActionResult Calculator(FinancialModel fmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewData["monthlyFixedIncome"] = fmodel.monthlyFixedIncome;
                    ViewData["monthlyVariableIncome"] = fmodel.monthlyVariableIncome;
                    ViewData["cashTowardsDownPayment"] = fmodel.cashTowardsDownPayment;

                    return View("Index");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Financial/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Financial/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Financial/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Financial/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
