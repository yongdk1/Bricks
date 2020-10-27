using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BricksMeatballs.Services;

namespace BricksMeatballs.Controllers
{
    public class DBController : Controller { 
    private readonly UraService _uraService;

    public DBController(UraService uraService)
    {
        _uraService = uraService;
    }

    public async Task<IActionResult> Index()
    {
        await _uraService.GetPMI_Resi_Transaction(batch: 1);

        return View();
    }
}
}
