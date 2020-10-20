﻿using System;
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
        public ActionResult Index(FinancialModel fmodel)
        {
            ViewBag.TDSRLimit = 3000;
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
        public ActionResult Edit()
        {
            return View();
        }

        // POST: FinancialController/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FinancialModel fmodel)
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
    }
}
